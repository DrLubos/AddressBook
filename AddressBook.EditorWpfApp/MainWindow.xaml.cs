using Microsoft.Win32;
using System.Collections.Generic;
using System.Net.Mime;
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
using AddressBook.CommonLibrary;
using System.ComponentModel;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeList? list = new EmployeeList(new Employee[0]);
        private bool edited = false;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            if (edited)
            {
                var result = MessageBox.Show(this, "Adresár bol zmenený. Chcete ho uložiť?", "Uložiť adresár", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (result == MessageBoxResult.Yes)
                {
                    SaveClick(sender, e);
                }
            }
            list?.Clear();
            UpdateValuesCount();
            edited = false;
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Address Book Files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                list = AddressBook.CommonLibrary.EmployeeList.LoadFromJson(new System.IO.FileInfo(dialog.FileName));
                DataGrid.ItemsSource = list;
                UpdateValuesCount();
                edited = false;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (list == null || list.Count < 1)
            {
                MessageBox.Show("No values to save", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var dialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                list.SaveToJson(new System.IO.FileInfo(dialog.FileName));
                edited = false;
            }
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            if (edited)
            {
                var result = MessageBox.Show(this, "Neuložené zmeny. Chcete odísť?", "Neuložené zmeny", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    return;
                }
            }
            Application.Current.Shutdown();
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Window
            {
                Title = "O Aplikácii",
                Width = 330,
                Height = 170,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };

            var stackPanel = new StackPanel();
            var title = new TextBlock { Text = "UNIZA adresár", Margin = new Thickness(10, 10, 0, 10), FontWeight = FontWeights.Bold, FontSize = 16 };
            var version = new TextBlock { Text = "Verzia 1.0", Margin = new Thickness(10, 0, 0, 0) };
            var copyright = new TextBlock { Text = "Copyright (c) 2024 Ľuboš Dragan", Margin = new Thickness(10, 0, 0, 0) };
            var aboutText = new TextBlock { Text = "Aplikácia na editovanie a vyhľadávanie zamestnancov.", Margin = new Thickness(10, 0, 0, 0) };
            var button = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right, Width = 70, Margin = new Thickness(0, 10, 10, 0) };

            stackPanel.Children.Add(title);
            stackPanel.Children.Add(version);
            stackPanel.Children.Add(copyright);
            stackPanel.Children.Add(aboutText);
            stackPanel.Children.Add(button);
            aboutWindow.Content = stackPanel;

            button.Click += (s, args) =>
            {
                aboutWindow.Close();
            };
            aboutWindow.ShowDialog();
        }

        private void AddOption(object sender, RoutedEventArgs e)
        {
            // If je generovany AI
            var newEmployee = new Employee();
            var employeeWindow = new EmployeeWindow(newEmployee);
            employeeWindow.ShowDialog();
            if (employeeWindow.DialogResult.HasValue && employeeWindow.DialogResult.Value)
            {
                list?.Add(newEmployee);
                DataGrid.ItemsSource = list;
                edited = true;
                UpdateValuesCount();
            }
        }

        private void EditOption(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = DataGrid.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                selectedEmployee.PropertyChanged += DetectEdit;

                var employeeWindow = new EmployeeWindow(selectedEmployee);
                employeeWindow.Closed += (s, args) =>
                {
                    selectedEmployee.PropertyChanged -= DetectEdit;
                };
                employeeWindow.ShowDialog();
            }
        }

        private void DetectEdit(object? sender, EventArgs e)
        {
            edited = true;
        }

        private void DeleteOption(object sender, RoutedEventArgs e)
        {
            // Vygenerovane od AI, aby som videl ako to asi funguje, bez toho aby som to musel ja hladat
            var selectedEmployee = DataGrid.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                var result = MessageBox.Show("Chcete odstrániť vybraného zamestnanca?", "Odstrániť zamestnanca", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    list?.Remove(selectedEmployee);
                    UpdateValuesCount();
                    edited = true;
                }
            }
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow(list);
            searchWindow.SearchCompleted += SearchWindow_SearchCompleted;
            searchWindow.ShowDialog();
        }

        private void SearchWindow_SearchCompleted(object sender, SearchEventArgs e)
        {
            // Metoda generovana AI
            if (e.FilteredResult != null)
            {
                list = new EmployeeList(e.FilteredResult.Employees);
                DataGrid.ItemsSource = list;
                edited = true;
                UpdateValuesCount();
            }
        }

        private void UpdateValuesCount()
        {
            valuesCount.Text = list?.Count.ToString();
            if (list?.Count > 0)
            {
                SearchButtonDef.IsEnabled = true;
            }
            else
            {
                SearchButtonDef.IsEnabled = false;
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            EditButtonDef.IsEnabled = false;
            DeleteButtonDef.IsEnabled = false;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                EditButtonDef.IsEnabled = true;
                DeleteButtonDef.IsEnabled = true;
                EditMenuOption.IsEnabled = true;
                DeleteMenuOption.IsEnabled = true;
            }
            else
            {
                EditButtonDef.IsEnabled = false;
                DeleteButtonDef.IsEnabled = false;
                EditMenuOption.IsEnabled = false;
                DeleteMenuOption.IsEnabled = false;
            }
        }
    }
}