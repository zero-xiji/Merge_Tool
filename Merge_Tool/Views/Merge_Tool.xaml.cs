using Merge_Tool.ViewModels;
using System.Windows;

namespace Merge_Tool.Views
{
    /// <summary>
    /// Copy_Tool.xaml の相互作用ロジック
    /// </summary>
    public partial class Merge_Tool : Window
    {
        public Merge_Tool()
        {
            InitializeComponent();
            this.DataContext = new Merge_ToolViewModel();
        }
    }
}
