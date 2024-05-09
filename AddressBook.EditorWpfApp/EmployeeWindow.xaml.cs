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
        private readonly Employee? originalEmployee;
        private readonly Employee? currentEmployee;

        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            if (employee != null)
            {
                originalEmployee = employee;
                currentEmployee = new Employee
                {
                    Name = originalEmployee.Name,
                    Position = originalEmployee.Position,
                    Phone = originalEmployee.Phone,
                    Email = originalEmployee.Email,
                    Room = originalEmployee.Room,
                    MainWorkplace = originalEmployee.MainWorkplace,
                    Workplace = originalEmployee.Workplace
                };
                DataContext = currentEmployee;
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            if (currentEmployee == null || originalEmployee == null)
            {
                Close();
                return;
            }
            currentEmployee.Name = originalEmployee.Name;
            currentEmployee.Position = originalEmployee.Position;
            currentEmployee.Phone = originalEmployee.Phone;
            currentEmployee.Email = originalEmployee.Email;
            currentEmployee.Room = originalEmployee.Room;
            currentEmployee.MainWorkplace = originalEmployee.MainWorkplace;
            currentEmployee.Workplace = originalEmployee.Workplace;
            Close();
        }

        public void OkButton(object sender, RoutedEventArgs e)
        {
            if (currentEmployee == null || originalEmployee == null)
            {
                Close();
                return;
            }
            originalEmployee.Name = currentEmployee.Name;
            originalEmployee.Position = currentEmployee.Position;
            originalEmployee.Phone = currentEmployee.Phone;
            originalEmployee.Email = currentEmployee.Email;
            originalEmployee.Room = currentEmployee.Room;
            originalEmployee.MainWorkplace = currentEmployee.MainWorkplace;
            originalEmployee.Workplace = currentEmployee.Workplace;
            if (originalEmployee.Name == string.Empty)
            {
                MessageBox.Show("Meno nesmie byť prázdne.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (originalEmployee.Position == string.Empty)
            {
                MessageBox.Show("Funkcia nesmie byť prázdna.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (originalEmployee.Email == string.Empty)
            {
                MessageBox.Show("Email nesmie byť prázdny.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
