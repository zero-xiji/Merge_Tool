﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Practices.Prism;
using Merge_Tool.ViewModels;

namespace TimeChange.Views
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
