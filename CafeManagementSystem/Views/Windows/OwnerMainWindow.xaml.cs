using System;
using System.Windows;
using System.Windows.Controls;
using CafeManagementSystem.ViewModels;
using CafeManagementSystem.Views.UserControls;

namespace CafeManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml (Cửa sổ chính cho Chủ quán - Tổng quan, Nhân sự, Ca làm)
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public OwnerMainWindow()
        {
            InitializeComponent();
            DataContext = new OwnerMainViewModel();
        }
    }
}
