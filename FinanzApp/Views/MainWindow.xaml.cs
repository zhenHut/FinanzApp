using FinanzApp.core.Models;
using FinanzApp.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace FinanzApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem lbi && lbi.DataContext is Transaction transaction)
            {
                {
                    if (DataContext is MainViewModel vm && vm.EditTransactionCommand.CanExecute(transaction))
                        vm.EditTransactionCommand.Execute(transaction);
                }
            }
        }
    }
}