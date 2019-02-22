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

        /// <summary>
        /// 复制该文件到目标文件夹中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddThisFile_Click(object sender, RoutedEventArgs e)
        {
            thisViewModel.Copy2Target();
        }

        /// <summary>
        /// 选择该文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SelectThisFile_Click(object sender, RoutedEventArgs e)
        {
            thisViewModel.setIsChecked();
        }
    }
}
