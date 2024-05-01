using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace AddressBook.CommonLibrary;

public class EmployeeList : ObservableCollection<Employee>
{
    public static EmployeeList? LoadFromJson(FileInfo jsonFile)
    {
        try
        {
            string json = File.ReadAllText(jsonFile.FullName);
            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);
            return new EmployeeList(employees);
        }
        catch
        {
            return null;
        }
    }

    public void SaveToJson(FileInfo jsonFile)
    {
        string json = JsonConvert.SerializeObject(this);
        File.WriteAllText(jsonFile.FullName, json);
    }

    public IEnumerable<string> GetPositions()
    {
        return this.Select(employee => employee.Position).Distinct().OrderBy(position => position);
    }

    public IEnumerable<string> GetMainWorkplaces()
    {
        return this.Select(employee => employee.MainWorkplace).Distinct().OrderBy(workplace => workplace);
    }

    public SearchResult Search(string? mainWorkplace = null, string? position = null, string? name = null)
    {
        var result = new List<Employee>();
        result = this.Where(employee =>
                (mainWorkplace == null || employee.MainWorkplace == mainWorkplace) &&
                (position == null || employee.Position == position) &&
                (name == null || employee.Name.ToLower().Contains(name.ToLower())))
            .ToList();
        return new SearchResult(result.ToArray());
    }
    public EmployeeList(IEnumerable<Employee> employees) : base(employees) { }
}