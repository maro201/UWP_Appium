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

namespace myApp.ViewModels
{
    public class CustomersPageViewModel : INotifyPropertyChanged
    {
        private readonly MyAppDbContext _dbContext;

        public CustomersPageViewModel()
        {
            _dbContext = new MyAppDbContext();
            LoadCustomers();
        }

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        public ICommand AddCustomerCommand => new RelayCommand(AddCustomer);
        public ICommand EditCustomerCommand => new RelayCommand(EditCustomer, CanEditDeleteCustomer);
        public ICommand DeleteCustomerCommand => new RelayCommand(DeleteCustomer, CanEditDeleteCustomer);

        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in _dbContext.Customers)
            {
                Customers.Add(customer);
            }
        }

        private void AddCustomer()
        {
            var customer = new Customer();
            var customerDialog = new CustomerDialog(customer);
            if (customerDialog.ShowDialog() == true)
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();
                Customers.Add(customer);
            }
        }

        private void EditCustomer()
        {
            var customerDialog = new CustomerDialog(SelectedCustomer);
            if (customerDialog.ShowDialog() == true)
            {
                _dbContext.SaveChanges();
            }
        }

        private void DeleteCustomer()
        {
            _dbContext.Customers.Remove(SelectedCustomer);
            _dbContext.SaveChanges();
            Customers.Remove(SelectedCustomer);
        }

        private bool CanEditDeleteCustomer()
        {
            return SelectedCustomer != null;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }


}
