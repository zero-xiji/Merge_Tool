using Merge_Tool.Resource;
using System.ComponentModel;

namespace Merge_Tool.Models
{
    /// <summary>
    /// file name from TargetPath
    /// </summary>
    public class Files : NotifyObject
    {
        public string Merge_pause16_Path = "..\\Resource\\Merge_pause16.png";
        public string Merge_ComplateOk16 = "..\\Resource\\Merge_ComplateOk16.png";
        public string Merge_SeriousWarning16 = "..\\Resource\\Merge_SeriousWarning16.png";

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

        private string _isBtnAddShow;
        public string IsBtnAddShow
        {
            get { return _isBtnAddShow; }
            set
            {
                _isBtnAddShow = value;
                RaisePropertyChanged("IsBtnAddShow");
            }
        }

        public Files() { }
        public Files(string FileName)
        {
            this.FileName = FileName;
            this.IsMerged = false;
            this.MergeSource = this.Merge_pause16_Path;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
        }

        #region Model
        /// <summary>
        /// Wait
        /// </summary>
        public void Model_Merge_pause()
        {
            this.IsMerged = false;
            this.MergeSource = this.Merge_pause16_Path;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
        }

        /// <summary>
        /// Merged
        /// </summary>
        public void Model_Merge_ComplateOk()
        {
            this.IsMerged = true;
            this.MergeSource = this.Merge_ComplateOk16;
            this.IsTargetExist = true;
            this.IsBtnAddShow = System.Windows.Visibility.Hidden.ToString();
        }

        /// <summary>
        /// Not Exist
        /// </summary>
        public void Model_Merge_SeriousWarning()
        {
            this.IsMerged = true;
            this.MergeSource = this.Merge_SeriousWarning16;
            this.IsTargetExist = false;
            this.IsBtnAddShow = System.Windows.Visibility.Visible.ToString();
        }
        #endregion Model
    }

}
