using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interface;
using FinanzApp.core.Model;
using FinanzApp.core.ViewModel;
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
