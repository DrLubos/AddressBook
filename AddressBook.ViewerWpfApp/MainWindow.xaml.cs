using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using AddressBook.CommonLibrary;

namespace AddressBook.ViewerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeList? list = null;
        SearchResult? filteredList = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ...

        private void Open(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Address Book Files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                list = AddressBook.CommonLibrary.EmployeeList.LoadFromJson(new System.IO.FileInfo(dialog.FileName));
            }
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