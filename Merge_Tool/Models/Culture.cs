using Merge_Tool.Resource;
using System.ComponentModel;            //INotifyPropertyChanged

namespace Merge_Tool.Models
{
    public class Culture : NotifyObject
    {
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

        private string _cultureName;
        public string CultureName
        {
            get { return _cultureName; }
            set
            {
                _cultureName = value;
                RaisePropertyChanged("CultureName");
            }
        }

        private string _extension;
        public string Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                RaisePropertyChanged("Extension");
            }
        }

        public Culture() { }
        public Culture(string Extension)
        {
            this.Extension = Extension;
            this.IsChecked = false;
            setCultureName(Extension);
        }

        private void setCultureName(string Extension)
        {
            switch (Extension)
            {
                case "en":
                    this.CultureName = "English";
                    break;
                case "ja":
                    this.CultureName = "Japanese";
                    break;
                case "fr":
                    this.CultureName = "French";
                    break;
                case "de":
                    this.CultureName = "German";
                    break;
                case "it":
                    this.CultureName = "Italian";
                    break;
                case "es":
                    this.CultureName = "Spanish";
                    break;
                case "pt":
                    this.CultureName = "Portuguese";
                    break;
                case "id":
                    this.CultureName = "Indonesian";
                    break;
                case "th":
                    this.CultureName = "Thai";
                    break;
                case "vi":
                    this.CultureName = "Vietnamese";
                    break;
                case "zh-Hans":
                    this.CultureName = "Chinese(Simplified)";
                    break;
                case "zh-Hant":
                    this.CultureName = "Chinese(Traditional)";
                    break;
                //错别字
                case "zh-Hanst":
                    this.CultureName = "Chinese(Simplified)";
                    break;
                case "zn-Hans":
                    this.CultureName = "Chinese(Simplified)";
                    break;
                case "zn-Hant":
                    this.CultureName = "Chinese(Traditional)";
                    break;
                case "zh-vi":
                    this.CultureName = "Vietnamese";
                    break;
                case "zh-Th":
                    this.CultureName = "Thai";
                    break;
                default:
                    this.CultureName = "ありません";
                    break;
            }
        }
    }

}
