namespace AddressBook.CommonLibrary;

public class SearchResult
{
    public Employee[] Employees { get; }

    public SearchResult(Employee[] employees)
    {
        Employees = employees;
    }

    public void SaveToCsv(FileInfo csvFile, string delimiter = "\t")
    {
        using (StreamWriter writer = new StreamWriter(csvFile.FullName))
        {
            writer.WriteLine("Name" + delimiter + "MainWorkplace" + delimiter + "Workplace" + delimiter +
                             "Room" + delimiter + "Phone" + delimiter + "Email" + delimiter + "Position");
            foreach (var employee in Employees)
            {
                writer.WriteLine($"{employee.Name}{delimiter}{employee.MainWorkplace}{delimiter}" +
                                 $"{employee.Workplace}{delimiter}{employee.Room}{delimiter}" +
                                 $"{employee.Phone}{delimiter}{employee.Email}{delimiter}{employee.Position}");
            }
        }
    }
}