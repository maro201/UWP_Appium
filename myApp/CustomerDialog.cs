using myApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace myApp
{
    public class CustomerDialog : ContentDialog
    {
        private readonly Customer _customer;
        private readonly TextBox _nameTextBox;
        private readonly TextBox _emailTextBox;

        public CustomerDialog(Customer customer)
        {
            _customer = customer;

            Title = "Add/Edit Customer";
            PrimaryButtonText = "Save";
            SecondaryButtonText = "Cancel";

            _nameTextBox = new TextBox
            {
                PlaceholderText = "Enter customer name",
                Text = _customer.Name
            };

            _emailTextBox = new TextBox
            {
                PlaceholderText = "Enter customer email",
                Text = _customer.Email
            };

            Content = new StackPanel
            {
                Children =
            {
                _nameTextBox,
                _emailTextBox
            }
            };

            PrimaryButtonClick += OnPrimaryButtonClick;
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _customer.Name = _nameTextBox.Text;
            _customer.Email = _emailTextBox.Text;
        }
    }

}
