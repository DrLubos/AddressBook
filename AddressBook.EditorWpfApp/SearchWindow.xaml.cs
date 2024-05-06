using AddressBook.CommonLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        EmployeeList? list = null;
        SearchResult? filteredList = null;

        public SearchWindow(EmployeeList list)
        {
            InitializeComponent();
            this.list = list;
            List<string> positions = [];
            List<string> workplaces = [];
            if (list != null)
            {
                foreach (var employee in list)
                {
                    if (!positions.Contains(employee.Position))
                    {
                        positions.Add(employee.Position);
                    }
                    if (employee.MainWorkplace != null)
                    {
                        if (!workplaces.Contains(employee.MainWorkplace))
                        {
                            workplaces.Add(employee.MainWorkplace);
                        }
                    }
                }
            }
            positions.Sort();
            workplaces.Sort();
            functionFilter.Items.Clear();
            workplaceFilter.Items.Clear();
            foreach (var position in positions)
            {
                functionFilter.Items.Add(position);
            }
            foreach (var workplace in workplaces)
            {
                workplaceFilter.Items.Add(workplace);
            }
        }

        // Metoda a aj parameter generovany AI
        public event EventHandler<SearchEventArgs>? SearchCompleted;
        protected virtual void OnSearchCompleted(SearchEventArgs e)
        {
            SearchCompleted?.Invoke(this, e);
        }

        // ...

        private void SearchEmployee(object sender, RoutedEventArgs e)
        {
            if (list == null)
            {
                MessageBox.Show("Open a file first");
                return;
            }
            var workplaceObj = workplaceFilter.SelectedItem;
            var positionObj = functionFilter.SelectedItem;
            string? workplace = workplaceObj?.ToString();
            string? position = positionObj?.ToString();
            filteredList = list.Search(mainWorkplace: workplace, position: position, name: nameFilter.Text);
            foundValues.Text = filteredList.Employees.Length.ToString();
            listBox.ItemsSource = filteredList.Employees;
            OnSearchCompleted(new SearchEventArgs(filteredList));
        }

        private void ResetSearch(object sender, RoutedEventArgs e)
        {
            nameFilter.Text = "";
            workplaceFilter.SelectedItem = null;
            functionFilter.SelectedItem = null;
            filteredList = null;
            foundValues.Text = "0";
            listBox.ItemsSource = null;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            if (filteredList == null)
            {
                MessageBox.Show("Search for employees first");
                return;
            }
            var dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                filteredList.SaveToCsv(new System.IO.FileInfo(dialog.FileName));
            }
        }
    }
}
