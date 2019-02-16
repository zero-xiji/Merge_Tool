using Merge_Tool.Resource;
using System.ComponentModel;

namespace Merge_Tool.Models
{
    /// <summary>
    /// file name from TargetPath
    /// </summary>
    public class Files : NotifyObject
    {
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

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        public Files() { }
        public Files(string FileName)
        {
            this.FileName = FileName;
            this.IsChecked = false;
        }
    }

}
