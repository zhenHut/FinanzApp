using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using FinanzApp.ViewModels;
using System.Windows;

namespace FinanzApp.View
{
    /// <summary>
    /// Interaktionslogik für TransaktionDetails.xaml
    /// </summary>
    public partial class TransactionDetail : Window
    {
        #region Constructor
        public TransactionDetail(Transaction transaction)
        {
            InitializeComponent();
            var viewModel = new TransactionDetailViewModel(transaction);
            RegisterDialogClose(viewModel);

            DataContext = viewModel;
        }

        #endregion

        #region fields
       
        #endregion

        #region Methods
        private void RegisterDialogClose(IDialogRequestClose viewModel)
        {
            viewModel.CloseRequested += OnCloseRequested;
         
        }

        private void OnCloseRequested(object ?sender, DialogCloseRequestedEventArgs e) 
        {
            DialogResult = e.DialogResult;
            Close();
        }


        #endregion

       
    }
}
