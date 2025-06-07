using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization; // <-- Added
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace Windows_Phone_7._1_Expense_Tracker_App
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<Expense> expenses;

        public MainPage()
        {
            InitializeComponent();
            expenses = LoadExpenses();
            ExpenseListBox.ItemsSource = expenses;
        }

        private void ClearInputs()
        {
            AmountTextBox.Text = string.Empty;
            CategoryTextBox.Text = string.Empty;
            NotesTextBox.Text = string.Empty;
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AmountTextBox.Text) || string.IsNullOrEmpty(CategoryTextBox.Text))
            {
                MessageBox.Show("Please enter amount and category.");
                return;
            }

            double amount;
            if (!double.TryParse(AmountTextBox.Text, out amount))
            {
                MessageBox.Show("Invalid amount format. Please enter a valid number.");
                return;
            }

            var expense = new Expense
            {
                Amount = amount,
                Category = CategoryTextBox.Text,
                Date = DateTime.Now,
                Notes = NotesTextBox.Text
            };

            expenses.Add(expense);
            ExpenseListBox.ItemsSource = null;
            ExpenseListBox.ItemsSource = expenses;
            SaveExpenses();
            ClearInputs();
        }

        // === REPLACE THESE METHODS ===
        private void SaveExpenses()
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = storage.CreateFile("expenses.xml"))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(List<Expense>));
                        serializer.WriteObject(stream, expenses);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving expenses: " + ex.Message);
            }
        }

        private List<Expense> LoadExpenses()
        {
            try
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists("expenses.xml"))
                    {
                        using (IsolatedStorageFileStream stream = storage.OpenFile("expenses.xml", FileMode.Open))
                        {
                            DataContractSerializer serializer = new DataContractSerializer(typeof(List<Expense>));
                            return (List<Expense>)serializer.ReadObject(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading expenses: " + ex.Message);
            }
            return new List<Expense>();
        }
    }

    // === MODIFY THE EXPENSE CLASS ===
    [DataContract]
    public class Expense
    {
        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Notes { get; set; }
    }
}