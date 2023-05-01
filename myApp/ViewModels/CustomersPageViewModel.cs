using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using myApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace myApp.ViewModels
{
    public class CustomersPageViewModel : ViewModelBase
    {
        private readonly MyAppDbContext _dbContext;

        public ObservableCollection<Customer> Customers { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public Customer SelectedCustomer { get; set; }

        public CustomersPageViewModel()
        {
            _dbContext = new MyAppDbContext();

            Customers = new ObservableCollection<Customer>(_dbContext.Customers);

            AddCustomerCommand = new RelayCommand(AddCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, CanEditOrDeleteCustomer);
            DeleteCustomerCommand = new RelayCommand(DeleteCustomer, CanEditOrDeleteCustomer);
        }

        private async void AddCustomer()
        {
            var customer = new Customer();
            var customerDialog = new CustomerDialog(customer);
            if (await customerDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();
                Customers.Add(customer);
            }
        }

        private async void EditCustomer()
        {
            var customerDialog = new CustomerDialog(SelectedCustomer);
            if (await customerDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                _dbContext.SaveChanges();
            }
        }

        private bool CanEditOrDeleteCustomer()
        {
            return SelectedCustomer != null;
        }

        private void DeleteCustomer()
        {
            _dbContext.Customers.Remove(SelectedCustomer);
            _dbContext.SaveChanges();
            Customers.Remove(SelectedCustomer);
        }
    }


}
