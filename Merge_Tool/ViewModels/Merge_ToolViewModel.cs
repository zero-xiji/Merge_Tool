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
        public Merge_ToolViewModel()
        {
            MergeInstallVersion = GetInstallVersionD();
            if (MergeInstallVersion.Equals(string.Empty))
            {
                MessageBox.Show("Merge not installed!", "Warn");
            }
        }

        #region Properties
        /// <summary>
        /// 所有文件数量
        /// </summary>
        public int _countFile { get; set; }

        /// <summary>
        /// 已选中文件数量
        /// </summary>
        public int _countFileSelected { get; set; }

        /// <summary>
        /// Merge安装路径
        /// </summary>
        string MergeInstallVersion = "";

        public bool _isUnSelectAllFiles { get; set; }

        private string Merging_cmd = string.Empty;

        private bool isMergeStop = false;
        private bool isThisMergedCompleted = false;

        /// <summary>
        /// 目标地址中不存在的文件个数
        /// </summary>
        private int countExistFile = 0;
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
                    if (countExistFile != 0)
                    {
                        Is_Missing_File_Exists_Show = Visibility.Visible.ToString();
                    }
                    else
                    {
                        Is_Missing_File_Exists_Show = Visibility.Hidden.ToString();
                    }
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
                    RaisePropertyChanged("SelectedFilesItem_Data");
                }
            }
        }

        /// <summary>
        /// 是否开始MergeAll
        /// </summary>
        private string _isMergeStart = Visibility.Hidden.ToString();
        public string IsMergeStart
        {
            get
            {
                return _isMergeStart;
            }
            set
            {
                if (_isMergeStart != value)
                {
                    _isMergeStart = value;
                    RaisePropertyChanged("IsMergeStart");
                }
            }
        }

        /// <summary>
        /// 是否显示添加全部missing文件按钮
        /// </summary>
        private string _is_Missing_File_Exists_Show = Visibility.Hidden.ToString();
        public string Is_Missing_File_Exists_Show
        {
            get
            {
                return _is_Missing_File_Exists_Show;
            }
            set
            {
                if (_is_Missing_File_Exists_Show != value)
                {
                    _is_Missing_File_Exists_Show = value;
                    RaisePropertyChanged("Is_Missing_File_Exists_Show");
                }
            }
        }
        #endregion

        #region Command Binding
        /// <summary>
        /// Source 文件夹
        /// </summary>
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

        /// <summary>
        /// 对比 文件夹
        /// </summary>
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

        /// <summary>
        /// 刷新
        /// </summary>
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
                                                       if (Directory.Exists(SourcePath_Data))
                                                       {
                                                           setFileList(SourcePath_Data);
                                                       }
                                                       else
                                                       {
                                                           MessageBox.Show("Source folder is not exist!", "Warn");
                                                       }
                                                   }
                                                   else
                                                   {
                                                       FilesData = new ObservableCollection<Files>();
                                                       MergedFileNumber_Data = string.Format("Merged {0}/{1}", _countFileSelected, _countFile);
                                                   }
                                               }));
                }
                return _button_FileRefresh_Command;
            }
        }

        /// <summary>
        /// Merge文件夹
        /// </summary>
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
                                                       string CmdMergeStr = string.Empty;
                                                       if (MergeInstallVersion != null && !MergeInstallVersion.Equals(string.Empty))
                                                       {
                                                           foreach (Files fileData in FilesData)
                                                           {
                                                               IsMergeStart = Visibility.Visible.ToString();

                                                               if (isMergeStop) { break; }
                                                               if (!fileData.IsTargetExist)
                                                               {
                                                                   continue;
                                                               }
                                                               cmdMerge(fileData);
                                                           }
                                                           IsMergeStart = Visibility.Hidden.ToString();
                                                       }
                                                       else
                                                       {
                                                           MessageBox.Show("Merge not installed!", "Warn");
                                                       }
                                                   }
                                                   else
                                                   {
                                                       MessageBox.Show("Folders path cannot be empty!", "Warn", MessageBoxButton.OK, MessageBoxImage.Information);
                                                   }
                                               }));
                }
                return _button_MergeAllFile_Command;
            }
        }

        /// <summary>
        /// 添加所有目标文件夹不存在文件
        /// </summary>
        private MyCommand _button_AddAllMissingFile_Command;
        public MyCommand Button_AddAllMissingFile_Command
        {
            get
            {
                if (_button_AddAllMissingFile_Command == null)
                {
                    _button_AddAllMissingFile_Command = new MyCommand(
                                           new Action<object>(
                                               o =>
                                               {
                                                   foreach(Files fileData in FilesData)
                                                   {
                                                       if (fileData.IsTargetExist) { continue; }
                                                       else
                                                       {
                                                           SelectedFilesItem_Data = fileData;
                                                           Copy2Target();
                                                       }
                                                   }
                                               }));
                }
                return _button_AddAllMissingFile_Command;
            }
        }
        #endregion

        /// <summary>
        /// 获取WinMergeU注册表信息
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 运行CMD
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取文件夹中文件列表
        /// </summary>
        /// <param name="SelectedPath"></param>
        private void setFileList(string SelectedPath)
        {
            DirectoryInfo tempExcelFolder = new DirectoryInfo(SelectedPath);
            FileInfo[] allFile = tempExcelFolder.GetFiles();
            DirectoryInfo[] AllFolder = tempExcelFolder.GetDirectories();

            FilesData = new ObservableCollection<Files>();
            _countFileSelected = 0;
            countExistFile = 0;

            getPath(SelectedPath);

            _countFile = FilesData.Count;

            //set Label
            MergedFileNumber_Data = string.Format("Merged {0}/{1}", _countFileSelected, _countFile);
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
            foreach (FileInfo thisFile in allFile)
            {
                string[] Relative_FileData = thisFile.FullName.Split(new string[] { SourcePath_Data }, StringSplitOptions.None);
                Files thisFiles = new Files(Relative_FileData[1]);
                string TargetFilePath_ToMerge = "";
                try
                {
                    if (TargetPath_Data != null)
                    {
                        TargetFilePath_ToMerge = TargetPath_Data.Trim() + Relative_FileData[1];

                        if (File.Exists(TargetFilePath_ToMerge))
                        {
                            thisFiles.Model_Merge_pause();
                        }
                        else
                        {
                            thisFiles.Model_Merge_SeriousWarning();
                            countExistFile++;
                            _countFileSelected++;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                FilesData.Add(thisFiles);
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo Folder in AllFolder)
            {
                getPath(Folder.FullName);
                //list.Add(Folder.FullName);//添加文件夹的路径到列表
            }
        }
        #endregion 获取相对路径

        /// <summary>
        /// Merge此文件
        /// </summary>
        public void MergedThisFile_Command()
        {
            if (SourcePath_Data != null && SourcePath_Data != ""
                && TargetPath_Data != null && TargetPath_Data != "")
            {
                if (MergeInstallVersion != null && !MergeInstallVersion.Equals(string.Empty))
                {
                    cmdMerge(SelectedFilesItem_Data);
                }
                else
                {
                    MessageBox.Show("Merge not installed!", "Warn");
                }
            }
            else
            {
                MessageBox.Show("Folders path cannot be empty!", "Warn", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Merge 文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private Files cmdMerge(Files FilePath)
        {
            string SourceFilePath_ToMerge = SourcePath_Data.Trim() + FilePath.FileName;
            string TargetFilePath_ToMerge = TargetPath_Data.Trim() + FilePath.FileName;
            if (FilePath.IsTargetExist)
            {
                string CmdMergeStr = "\"" + MergeInstallVersion + " \" \"" + SourceFilePath_ToMerge + "\" \"" + TargetFilePath_ToMerge + "\"";
                string RunCmdResult = RunCmd(CmdMergeStr);
                FilePath.MergeSource = FilePath.Merge_ComplateOk16;
                FilePath.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
            }
            else
            {
                FilePath.MergeSource = FilePath.Merge_SeriousWarning16;
                FilePath.IsBtnAddShow = System.Windows.Visibility.Visible.ToString();
                MessageBox.Show("File does not exist in the target folder！", "Warn");
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (!FilePath.IsMerged)
                {
                    _countFileSelected++;
                }
                FilePath.IsMerged = true;
                MergedFileNumber_Data = string.Format("Merged {0}/{1}", _countFileSelected, _countFile);
            }));
            return FilePath;
        }

        /// <summary>
        /// copy2TargetFordel
        /// </summary>
        public void Copy2Target()
        {
            string Source_File_Path = SourcePath_Data + "\\" + SelectedFilesItem_Data.FileName;
            string Target_File_Path = TargetPath_Data + "\\" + SelectedFilesItem_Data.FileName;
            try
            {
                System.IO.File.Copy(Source_File_Path, Target_File_Path);
            }
            catch
            {
                if (MessageBox.Show("此文件已存在，是否覆盖", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.IO.File.Copy(Source_File_Path, Target_File_Path, true);
                }
                else
                {
                    return;
                }
            }
            SelectedFilesItem_Data.Model_Merge_pause();
            _countFileSelected--;
            countExistFile--;
            MergedFileNumber_Data = string.Format("Merged {0}/{1}", _countFileSelected, _countFile);
        }
    }
}
