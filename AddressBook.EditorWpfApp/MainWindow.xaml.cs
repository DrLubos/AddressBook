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

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeList? list = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New");
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
            }
            DataGrid.ItemsSource = list;
            UpdateValuesCount();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (list == null)
            {
                MessageBox.Show("No values to save");
                return;
            }
            var dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                list.SaveToJson(new System.IO.FileInfo(dialog.FileName));
            }
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
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
            MessageBox.Show("Delete");
        }

        private void EditOption(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete");
        }

        private void DeleteOption(object sender, RoutedEventArgs e)
        {
            // Pomoc od AI
            var selectedEmployee = DataGrid.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                var result = MessageBox.Show("Chcete odstrániť vybraného zamestnanca?", "Odstrániť zamestnanca", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    list?.Remove(selectedEmployee);
                    UpdateValuesCount();
                }
            }
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete");
        }

        private void UpdateValuesCount()
        {
            if (list != null)
            {
                valuesCount.Text = list.Count.ToString();
            }
            else
            {
                valuesCount.Text = "0";
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