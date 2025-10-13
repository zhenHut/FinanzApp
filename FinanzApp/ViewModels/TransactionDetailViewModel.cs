using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Xml.Linq;



namespace FinanzApp.ViewModels
{
    public partial class TransactionDetailViewModel : ObservableObject, IDialogRequestClose
    {
        #region Constructor

        public TransactionDetailViewModel(Transaction transaction)
        {
            Transaction = transaction;
            _categoryService = App.Services.GetRequiredService<ICategoryService>();
            _ = LoadCategoriesAsync();
        }

        #endregion

        #region Fields
        private readonly ICategoryService _categoryService;
        #endregion

        #region Properties
        public Transaction Transaction { get; init; }
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

        [ObservableProperty]
        private string _newCategoryName = string.Empty;

        // Befüllung der Combo-Box
        public Array TransactionTypeList => Enum.GetValues(typeof(TransactionType));
        #endregion

        #region Events

        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
        #endregion

        #region Methods

        [RelayCommand]
        private async Task SaveTransactionAsync()
        {
            // Name aus der ComboBox
            var name = (NewCategoryName ?? string.Empty).Trim();

            // nur wenn nichts ausgewählt ist (keine FK) und ein Name eingegeben wurde
            if (Transaction.CategoryId is null && !string.IsNullOrWhiteSpace(name))
            {
                // existiert die Kategorie bereits?
                var existing = Categories.FirstOrDefault(c =>
                    string.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase));

                if (existing is null)
                {
                    // neu anlegen
                    var created = await _categoryService.AddSync(new Category { Name = name });
                    Categories.Add(created);
                    Transaction.CategoryId = created.Id;   // FK setzen
                }
                else
                {
                    // vorhandene zuordnen
                    Transaction.CategoryId = existing.Id;
                }

                // wichtig: Navigation leeren, damit kein Cross-DbContext-Problem entsteht
                Transaction.Category = null;
            }

            // Dialog schließen
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }
        

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false));
        }

        private async Task LoadCategoriesAsync()
        {
            var cats = await _categoryService.GetAllSync();
            Categories.Clear();
            foreach (var c in cats.OrderBy(c => c.Name))
            {
                Categories.Add(c);
            }
        }

        //[RelayCommand]
        //private async Task SaveAsync()
        //{
        //    var name = (NewCategoryName ?? string.Empty).Trim();

        //    if (string.IsNullOrWhiteSpace(name))
        //        return;

        //    if (Categories.Any(c => string.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase)))
        //        return;

        //    var created = await _categoryService.AddSync(new Category { Name = name });
        //    Categories.Add(created);
        //    Transaction.CategoryId = created.Id;
        //    Transaction.Category = created;
        //}

        #endregion
    }
}
