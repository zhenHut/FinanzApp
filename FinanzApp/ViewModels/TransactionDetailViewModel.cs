using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;



namespace FinanzApp.ViewModels
{
    public partial class TransactionDetailViewModel :ObservableObject, IDialogRequestClose
    {
        #region Constructor

        public TransactionDetailViewModel(Transaction transaction) 
        {
            Transaction = transaction;
        }

        #endregion

        #region Fields

        #endregion

        #region Properties
        public Transaction Transaction { get; init; }
        
        // Befüllung der Combo-Box
        public Array TransactionTypeList => Enum.GetValues(typeof(TransactionType));
        #endregion

        #region Events
        
        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
        #endregion

        #region Methods

        [RelayCommand]
       private void Save()
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false));
        }

        #endregion
    }
}
