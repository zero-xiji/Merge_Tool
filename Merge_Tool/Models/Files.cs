using Merge_Tool.Resource;
using System.ComponentModel;

namespace Merge_Tool.Models
{
    /// <summary>
    /// file name from TargetPath
    /// </summary>
    public class Files : NotifyObject
    {
        #region const
        public string Image_Merge_pause16_Path = "..\\Resource\\Merge_pause16.png";
        public string Image_Merge_ComplateOk16 = "..\\Resource\\Merge_ComplateOk16.png";
        public string Image_Merge_SeriousWarning16 = "..\\Resource\\Merge_SeriousWarning16.png";
        public string Image_Selected = "..\\Resource\\Selected.png";
        public string Image_UnSelected = "..\\Resource\\UnSelected.png";
        #endregion const

        #region Properties
        /// <summary>
        /// 文件名
        /// </summary>
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 是否Merge过了
        /// </summary>
        private bool _isMerged;
        public bool IsMerged
        {
            get { return _isMerged; }
            set
            {
                _isMerged = value;
                RaisePropertyChanged("IsMerged");
            }
        }

        /// <summary>
        /// 是否选择
        /// </summary>
        private bool _isSelect;
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                if (_isSelect)
                {
                    IsSelectedImageSource = this.Image_Selected;
                }
                else
                {
                    IsSelectedImageSource = this.Image_UnSelected;
                }
                RaisePropertyChanged("IsSelect");
            }
        }

        /// <summary>
        /// 选择按钮图片
        /// </summary>
        private string _isSelectedImageSource;
        public string IsSelectedImageSource
        {
            get { return _isSelectedImageSource; }
            set
            {
                _isSelectedImageSource = value;
                RaisePropertyChanged("IsSelectedImageSource");
            }
        }

        /// <summary>
        /// merge的状态图片
        /// </summary>
        private string _mergeSource;
        public string MergeSource
        {
            get { return _mergeSource; }
            set
            {
                _mergeSource = value;
                RaisePropertyChanged("MergeSource");
            }
        }

        /// <summary>
        /// 在目标文件夹中是否存在该文件
        /// </summary>
        private bool _isTargetExist;
        public bool IsTargetExist
        {
            get { return _isTargetExist; }
            set
            {
                _isTargetExist = value;
                RaisePropertyChanged("IsTargetExist");
            }
        }

        /// <summary>
        /// 是否显示ADD按钮
        /// </summary>
        private string _isBtnAddShow;
        public string IsBtnAddShow
        {
            get { return _isBtnAddShow; }
            set
            {
                _isBtnAddShow = value;
                if (_isBtnAddShow.Equals(System.Windows.Visibility.Hidden.ToString()))
                    IsBtnSelectShow = System.Windows.Visibility.Visible.ToString();
                else
                {
                    IsBtnSelectShow = System.Windows.Visibility.Hidden.ToString();
                }
                RaisePropertyChanged("IsBtnAddShow");
            }
        }

        /// <summary>
        /// 是否显示Select按钮
        /// </summary>
        private string _isBtnSelectShow;
        public string IsBtnSelectShow
        {
            get { return _isBtnSelectShow; }
            set
            {
                _isBtnSelectShow = value;
                RaisePropertyChanged("IsBtnSelectShow");
            }
        }
        #endregion Properties

        public Files() { }
        public Files(string FileName)
        {
            this.FileName = FileName;
            this.IsMerged = false;
            this.MergeSource = this.Image_Merge_pause16_Path;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
            this.IsSelect = false;
        }

        #region Model
        /// <summary>
        /// Wait
        /// </summary>
        public void Model_Merge_pause()
        {
            this.IsMerged = false;
            this.MergeSource = this.Image_Merge_pause16_Path;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
        }

        /// <summary>
        /// Merged
        /// </summary>
        public void Model_Merge_ComplateOk()
        {
            this.IsMerged = true;
            this.MergeSource = this.Image_Merge_ComplateOk16;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
        }

        /// <summary>
        /// Not Exist
        /// </summary>
        public void Model_Merge_SeriousWarning()
        {
            this.IsMerged = true;
            this.MergeSource = this.Image_Merge_SeriousWarning16;
            this.IsTargetExist = false;
            this.IsBtnAddShow = System.Windows.Visibility.Visible.ToString();
        }
        #endregion Model
    }

}
