using System;
using System.Windows;
using System.Windows.Controls;
using CafeManagementSystem.ViewModels;
using CafeManagementSystem.Views.UserControls;

namespace CafeManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for StaffMainWindow.xaml (Cửa sổ chính cho Nhân viên - Bán hàng & Chấm công)
    /// </summary>
    public partial class StaffMainWindow : Window
    {
        public StaffMainWindow()
        {
            InitializeComponent();
            DataContext = new StaffMainViewModel();
        }
    }
}
