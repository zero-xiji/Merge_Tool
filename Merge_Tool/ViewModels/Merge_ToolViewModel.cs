using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using Merge_Tool.Resource;
using System.Collections.ObjectModel;
using Merge_Tool.Models;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Data;

namespace Merge_Tool.ViewModels
{
    class Merge_ToolViewModel : NotifyObject
    {
        #region Properties
        private int _countFileSelected;
        private int _countFile;
        private int _countCultureSelected;
        private int _countCulture;
        private bool _isUnSelectAllFiles;
        private int _fileType;
        private ConfigFile _configFile;
        private string _zipPath;
        #endregion Properties

        #region Data Binding

        private ObservableCollection<Files> _filesData;
        public ObservableCollection<Files> FilesData
        {
            get { return _filesData; }
            set
            {
                if (_filesData != value)
                {
                    _filesData = value;
                    RaisePropertyChanged("FilesData");
                }
            }
        }

        private ObservableCollection<ConfigFile> _configFileData;
        public ObservableCollection<ConfigFile> ConfigFileData
        {
            get { return _configFileData; }
            set
            {
                if (_configFileData != value)
                {
                    _configFileData = value;
                    RaisePropertyChanged("ConfigFileData");
                }
            }
        }

        private ObservableCollection<Culture> _cultureData;
        public ObservableCollection<Culture> CultureData
        {
            get { return _cultureData; }
            set
            {
                if (_cultureData != value)
                {
                    _cultureData = value;
                    RaisePropertyChanged("CultureData");
                }
            }
        }

        private string _TargetPath_Data;
        public string TargetPath_Data
        {
            get { return _TargetPath_Data; }
            set
            {
                if (_TargetPath_Data != value)
                {
                    _TargetPath_Data = value;
                    RaisePropertyChanged("TargetPath_Data");
                }
            }
        }

        private string _SourcePath_Data;
        public string SourcePath_Data
        {
            get { return _SourcePath_Data; }
            set
            {
                if (_SourcePath_Data != value)
                {
                    _SourcePath_Data = value;
                    RaisePropertyChanged("SourcePath_Data");
                }
            }
        }

        private string _selectedFileNumber_Data;
        public string SelectedFileNumber_Data
        {
            get { return _selectedFileNumber_Data; }
            set
            {
                if (_selectedFileNumber_Data != value)
                {
                    _selectedFileNumber_Data = value;
                    RaisePropertyChanged("SelectedFileNumber_Data");
                }
            }
        }

        private string _selectedCultureNumber_Data;
        public string SelectedCultureNumber_Data
        {
            get { return _selectedCultureNumber_Data; }
            set
            {
                if (_selectedCultureNumber_Data != value)
                {
                    _selectedCultureNumber_Data = value;
                    RaisePropertyChanged("SelectedCultureNumber_Data");
                }
            }
        }

        private Files _selectedFilesItem_Data;
        public Files SelectedFilesItem_Data
        {
            get
            {
                return _selectedFilesItem_Data;
            }
            set
            {
                if (_selectedFilesItem_Data != value)
                {
                    _selectedFilesItem_Data = value;
                    setIsChecked("Files");
                    switch (_fileType)
                    {
                        case 0:
                            //OpenExcel();
                            break;
                        case 1:
                            break;
                        case 2:
                            //OpenConfig();
                            break;
                        default:
                            break;

                    }
                    RaisePropertyChanged("SelectedFilesItem_Data");
                }
            }
        }

        private Culture _selectedCultureItem_Data;
        public Culture SelectedCultureItem_Data
        {
            get
            {
                return _selectedCultureItem_Data;
            }
            set
            {
                if (_selectedCultureItem_Data != value)
                {
                    _selectedCultureItem_Data = value;
                    setIsChecked("Culture");
                    RaisePropertyChanged("SelectedCultureItem_Data");
                }
            }
        }

        #endregion

        #region Command Binding

        private MyCommand _button_SourcePath_Command;
        public MyCommand Button_SourcePath_Command
        {
            get
            {
                if (_button_SourcePath_Command == null)
                {
                    _button_SourcePath_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   System.Windows.Forms.FolderBrowserDialog dataFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
                                                   dataFolderDialog.Description = "选择文件夹";
                                                   dataFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
                                                   if (dataFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                                   {
                                                       this.SourcePath_Data = dataFolderDialog.SelectedPath;
                                                       setExcelFileList(dataFolderDialog.SelectedPath);
                                                   }
                                               }));
                } return _button_SourcePath_Command;
            }
        }

        private MyCommand _button_TargetPath_Command;
        public MyCommand Button_TargetPath_Command
        {
            get
            {
                if (_button_TargetPath_Command == null)
                {
                    _button_TargetPath_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   System.Windows.Forms.FolderBrowserDialog saveFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
                                                   saveFolderDialog.Description = "选择文件夹";
                                                   saveFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
                                                   if (saveFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                                   {
                                                       this.TargetPath_Data = saveFolderDialog.SelectedPath;
                                                   }
                                               }));
                }
                return _button_TargetPath_Command;
            }
        }

        private MyCommand _button_MergeAllFile_Command;
        public MyCommand Button_MergeAllFile_Command
        {
            get
            {
                if (_button_MergeAllFile_Command == null)
                {
                    _button_MergeAllFile_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   for (int i = 0; i < _countFile; i++)
                                                   {
                                                       SelectedFilesItem_Data = FilesData[i];
                                                       FilesData[i].IsChecked = true;
                                                   }
                                                   _countFileSelected = _countFile;
                                                   SelectedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);

                                               }));
                }
                return _button_MergeAllFile_Command;
            }
        }

        private MyCommand _button_UnSelectAllFile_Command;
        public MyCommand Button_UnSelectAllFile_Command
        {
            get
            {
                if (_button_UnSelectAllFile_Command == null)
                {
                    _button_UnSelectAllFile_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   _isUnSelectAllFiles = true;
                                                   for (int i = 0; i < _countFile; i++)
                                                   {
                                                       FilesData[i].IsChecked = false;
                                                   }
                                                   //clear Culture
                                                   CultureData.Clear();
                                                   SelectedFilesItem_Data = new Files();
                                                   SelectedCultureItem_Data = new Culture();
                                                   _countCultureSelected = 0;
                                                   _countCulture = 0;
                                                   SelectedCultureNumber_Data = string.Format("Selected {0}/{1}", _countCultureSelected, _countCulture);
                                                   _isUnSelectAllFiles = false;

                                                   _countFileSelected = 0;
                                                   SelectedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
                                               }));
                }
                return _button_UnSelectAllFile_Command;
            }
        }

        private MyCommand _button_FileRefresh_Command;
        public MyCommand Button_FileRefresh_Command
        {
            get
            {
                if (_button_FileRefresh_Command == null)
                {
                    _button_FileRefresh_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   if (SourcePath_Data != null && SourcePath_Data != "")
                                                       setExcelFileList(SourcePath_Data);
                                               }));
                }
                return _button_FileRefresh_Command;
            }
        }
        #endregion

        

        private void setIsChecked(string ModelType)
        {
            if (ModelType.Equals("Files"))
            {
                if (_isUnSelectAllFiles)
                {
                    _selectedFilesItem_Data.IsChecked = false;
                }
                else
                {
                    if (_selectedFilesItem_Data != null)
                    {
                        if (_selectedFilesItem_Data.IsChecked == true)
                        {
                            _selectedFilesItem_Data.IsChecked = false;
                            _countFileSelected--;
                        }
                        else
                        {
                            _selectedFilesItem_Data.IsChecked = true;
                            _countFileSelected++;
                        }
                        SelectedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
                    }
                }
            }
            else
            {
                if (_selectedCultureItem_Data != null)
                {
                    if (_selectedCultureItem_Data.IsChecked == true)
                    {
                        _selectedCultureItem_Data.IsChecked = false;
                        _countCultureSelected--;
                    }
                    else
                    {
                        _selectedCultureItem_Data.IsChecked = true;
                        _countCultureSelected++;
                    }
                    SelectedCultureNumber_Data = string.Format("Selected {0}/{1}", _countCultureSelected, _countCulture);
                }
            }
        }
        private void setExcelFileList(string SelectedPath)
        {
            DirectoryInfo tempExcelFolder = new DirectoryInfo(SelectedPath);
            FileInfo[] allFile = tempExcelFolder.GetFiles();
            DirectoryInfo[] AllFolder = tempExcelFolder.GetDirectories();

            FilesData = new ObservableCollection<Files>();
            FileData_PathList = new List<string>();

            getPath(SelectedPath);
            foreach (string FileData_Path in FileData_PathList)
            {
                FilesData.Add(new Files(FileData_Path));
            }
            _countFile = FilesData.Count;
            _countFileSelected = 0;

            //set Label
            SelectedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
        }

        static List<string> FileData_PathList = new List<string>();//定义list变量，存放获取到的路径
        static List<string> Source_FileData_PathList = new List<string>();//定义list变量，存放获取到的路径
        static List<string> Target_FileData_PathList = new List<string>();//定义list变量，存放获取到的路径

        public static List<string> getPath(string path)
        {
            DirectoryInfo tempExcelFolder = new DirectoryInfo(path);
            FileInfo[] allFile = tempExcelFolder.GetFiles();
            DirectoryInfo[] AllFolder = tempExcelFolder.GetDirectories();
            foreach (FileInfo File in allFile)
            {
                FileData_PathList.Add(File.FullName);//添加文件的路径到列表
                Source_FileData_PathList.Add(File.DirectoryName);
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo Folder in AllFolder)
            {
                getPath(Folder.FullName);
                //list.Add(Folder.FullName);//添加文件夹的路径到列表
            }
            return FileData_PathList;
        }

        #region Event Binding
        #endregion
    }
}
