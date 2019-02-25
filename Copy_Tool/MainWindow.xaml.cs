using System.Windows;

namespace Copy_Tool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            string Source_Root_Path = this.Source_Root_Path.Text.Trim();
            string Source_File = this.Source_Path.Text.Trim();
            string Target_Root_Path = this.Target_Root_Path.Text.Trim();

            string Source_File_Path = System.IO.Path.Combine(Source_Root_Path, Source_File);
            string Target_File_Path = Target_Root_Path;
            string[] NewFolderList = Source_File.Split('/');
            foreach (string NowFolder in NewFolderList)
            {
                Target_File_Path = System.IO.Path.Combine(Target_File_Path, NowFolder);
                if (NowFolder.Equals(NewFolderList[(NewFolderList.Length - 1)]))
                    break;
                System.IO.Directory.CreateDirectory(Target_File_Path);
            }
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
            }
        }
    }
}
