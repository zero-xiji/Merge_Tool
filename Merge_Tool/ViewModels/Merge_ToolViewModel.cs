using Merge_Tool.Models;
using Merge_Tool.Resource;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Merge_Tool.ViewModels
{
    class Merge_ToolViewModel : NotifyObject
    {
        #region Properties
        /// <summary>
        /// 所有文件数量
        /// </summary>
        public int _countFile { get; set; }

        /// <summary>
        /// 已选中文件数量
        /// </summary>
        public int _countFileSelected { get; set; }


        public bool _isUnSelectAllFiles { get; set; }
        #endregion Properties

        #region Data Binding
        /// <summary>
        /// 源地址
        /// </summary>
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
        /// <summary>
        /// 对比地址
        /// </summary>
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
        
        /// <summary>
        /// Source文件列表
        /// </summary>
        private ObservableCollection<Files> _filesData;
        public ObservableCollection<Files> FilesData
        {
            get { return _filesData; }
            set
            {
                if (_filesData != value)
                {
                    _filesData = value;
                    _countFile = _filesData.Count;
                    RaisePropertyChanged("FilesData");
                }
            }
        }

        /// <summary>
        /// 已选中文件数量/所有文件数量
        /// </summary>
        private string _mergedFileNumber_Data;
        public string MergedFileNumber_Data
        {
            get { return _mergedFileNumber_Data; }
            set
            {
                if (_mergedFileNumber_Data != value)
                {
                    _mergedFileNumber_Data = value;
                    RaisePropertyChanged("MergedFileNumber_Data");
                }
            }
        }
        
        /// <summary>
        /// 选中文件
        /// </summary>
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
                    setIsMerged();
                    RaisePropertyChanged("SelectedFilesItem_Data");
                }
            }
        }
        #endregion

        #region Command Binding
        //Source 文件夹
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
                                                       setFileList(dataFolderDialog.SelectedPath);
                                                   }
                                               }));
                } return _button_SourcePath_Command;
            }
        }

        //对比 文件夹
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

        //刷新
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
                                                   {
                                                       setFileList(SourcePath_Data);
                                                   }
                                                   else
                                                   {
                                                       FilesData = new ObservableCollection<Files>();
                                                       MergedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
                                                   }
                                               }));
                }
                return _button_FileRefresh_Command;
            }
        }

        //Merge
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
                                                   if (SourcePath_Data != null && SourcePath_Data != "")
                                                   {
                                                       setFileList(SourcePath_Data);
                                                   }
                                                   if (SourcePath_Data != null && SourcePath_Data != ""
                                                       && TargetPath_Data != null && TargetPath_Data != "")
                                                   {
                                                       string InstallVersionD = GetInstallVersionD();
                                                       string CmdMergeStr = string.Empty;
                                                       if (InstallVersionD != null && !InstallVersionD.Equals(string.Empty))
                                                       {
                                                           string SourceFilePath_ToMerge = "";
                                                           string TargetFilePath_ToMerge = "";
                                                           foreach (Files FilePath in FilesData)
                                                           {
                                                               SourceFilePath_ToMerge = SourcePath_Data.Trim() + FilePath.FileName;
                                                               TargetFilePath_ToMerge = TargetPath_Data.Trim() + FilePath.FileName;
                                                               if (File.Exists(TargetFilePath_ToMerge))
                                                               {
                                                                   CmdMergeStr = "\"" + InstallVersionD + " \" \"" + SourceFilePath_ToMerge + "\" \"" + TargetFilePath_ToMerge + "\"";
                                                                   string RunCmdResult = RunCmd(CmdMergeStr);
                                                                   FilePath.MergeSource = FilePath.Merge_ComplateOk16;
                                                               }
                                                               else
                                                               {
                                                                   FilePath.MergeSource = FilePath.Merge_SeriousWarning16;
                                                               }
                                                               Application.Current.Dispatcher.Invoke(new Action(() =>
                                                               {
                                                                   FilePath.IsMerged = true;
                                                                   _countFileSelected++;
                                                                   MergedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
                                                               }));
                                                           }
                                                       }
                                                       else
                                                       {
                                                           MessageBox.Show("未安装Merge!", "错误");
                                                       }
                                                   }
                                                   else
                                                   {
                                                       MessageBox.Show("要对比文件夹不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Information);
                                                   }
                                               }));
                }
                return _button_MergeAllFile_Command;
            }
        }

        #endregion
        

        //获取WinMergeU注册表信息
        public static string GetInstallVersionD()
        {
            string RegPath64 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\WinMergeU.exe";
            string RegPath32 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\WinMergeU.exe";
            string strRtn = string.Empty;
            try
            {
                string strKeyName = string.Empty;
                RegistryKey regKey = Registry.LocalMachine;
                RegistryKey regSubKey = regKey.OpenSubKey(RegPath64, false);
                if (regSubKey == null)
                {
                    // 64bitのレジストリで取得出来ない場合は、32bitのレジストリで試す
                    regSubKey = Registry.LocalMachine.OpenSubKey(RegPath32, false);
                }
                object objResult = regSubKey.GetValue(strKeyName);
                RegistryValueKind regValueKind = regSubKey.GetValueKind(strKeyName);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    strRtn = objResult.ToString();
                }
            }
            catch
            {
                Console.Write("朋友，获取程序路径失败！");
            }
            return strRtn;
        }

        //运行CMD
        public string RunCmd(string cmd)
        {      
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.WriteLine("exit");
            string outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();
            return outStr;
        }

        #region 获取相对路径，设置FileList
        //获取文件夹中文件列表
        private void setFileList(string SelectedPath)
        {
            DirectoryInfo tempExcelFolder = new DirectoryInfo(SelectedPath);
            FileInfo[] allFile = tempExcelFolder.GetFiles();
            DirectoryInfo[] AllFolder = tempExcelFolder.GetDirectories();

            FilesData = new ObservableCollection<Files>();

            getPath(SelectedPath);

            _countFile = FilesData.Count;
            _countFileSelected = 0;

            //set Label
            MergedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
        }

        /// <summary>
        /// 获取子文件夹内的文件列表，递归遍历
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void getPath(string path)
        {
            DirectoryInfo tempExcelFolder = new DirectoryInfo(path);
            FileInfo[] allFile = tempExcelFolder.GetFiles();
            DirectoryInfo[] AllFolder = tempExcelFolder.GetDirectories();
            foreach (FileInfo File in allFile)
            {
                string[] Relative_FileData = File.FullName.Split(new string[] { SourcePath_Data }, StringSplitOptions.None);
                FilesData.Add(new Files(Relative_FileData[1]));
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo Folder in AllFolder)
            {
                getPath(Folder.FullName);
                //list.Add(Folder.FullName);//添加文件夹的路径到列表
            }
        }
        #endregion 获取相对路径

        private void setIsMerged()
        {
            if (_isUnSelectAllFiles)
            {
                _selectedFilesItem_Data.IsMerged = false;
            }
            else
            {
                if (_selectedFilesItem_Data != null)
                {
                    if (_selectedFilesItem_Data.IsMerged == true)
                    {
                        _selectedFilesItem_Data.IsMerged = false;
                        _countFileSelected--;
                    }
                    else
                    {
                        _selectedFilesItem_Data.IsMerged = true;
                        _countFileSelected++;
                    }
                    MergedFileNumber_Data = string.Format("Selected {0}/{1}", _countFileSelected, _countFile);
                }
            }
        }
    }
}
