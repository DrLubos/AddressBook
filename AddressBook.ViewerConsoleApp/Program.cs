//Inspiration: https://stackoverflow.com/questions/24661047/how-to-get-command-line-parameters-and-put-them-into-variables
//https://github.com/commandlineparser/commandline
using CommandLine;
namespace AddressBook.ViewerConsoleApp;
class Options
{
    [Option('i', "input", Required = true, HelpText = "Input JSON file.")]
    public string? Input { get; set; }

    [Option('n', "name", Required = false, HelpText = "Name to search.")]
    public string? Name { get; set; }

    [Option('p', "position", Required = false, HelpText = "Position to search.")]
    public string? Position { get; set; }

    [Option('m', "main-workplace", Required = false, HelpText = "Main workplace to search.")]
    public string? MainWorkplace { get; set; }

    [Option('o', "output", Required = false, HelpText = "Output CSV file.")]
    public string? Output { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (!File.Exists(o.Input))
                {
                    Console.Error.WriteLine("The file " + o.Input + " does not exist.");
                    Environment.Exit(1);
                }

                var employeeList = AddressBook.CommonLibrary.EmployeeList.LoadFromJson(new FileInfo(o.Input));
                if (employeeList == null)
                {
                    Console.Error.WriteLine("Failed to load employees from " + o.Input + ".");
                    Environment.Exit(1);
                }

                var searchResult = employeeList.Search(o.MainWorkplace, o.Position, o.Name);

                for (int i = 0; i < searchResult.Employees.Length; i++)
                {
                    var employee = searchResult.Employees[i];
                    Console.WriteLine($"[{i + 1}] {employee.Name}");
                    Console.WriteLine($"Pracovisko: {employee.Workplace}");
                    Console.WriteLine($"Miestnosť: {employee.Room}");
                    Console.WriteLine($"Telefón: {employee.Phone}");
                    Console.WriteLine($"E-mail: {employee.Email}");
                    Console.WriteLine($"Funkcia: {employee.Position}");
                    Console.WriteLine();
                }

                if (!string.IsNullOrEmpty(o.Output))
                {
                    searchResult.SaveToCsv(new FileInfo(o.Output));
                }
            });
    }
}