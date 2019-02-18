using Merge_Tool.ViewModels;
using System.Windows;

namespace Merge_Tool.Views
{
    /// <summary>
    /// Copy_Tool.xaml の相互作用ロジック
    /// </summary>
    public partial class Merge_Tool : Window
    {
        Merge_ToolViewModel thisViewModel = new Merge_ToolViewModel();
        public Merge_Tool()
        {
            InitializeComponent();
            this.DataContext = thisViewModel;
        }

        private void DataGrid_FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            thisViewModel.MergedThisFile_Command();
        }
    }
}
