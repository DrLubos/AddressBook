using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using AddressBook.CommonLibrary;

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {

        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            if (employee != null)
            {
                DataContext = employee;
            }
        }
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
