using System.ComponentModel;

namespace AddressBook.CommonLibrary;

public class Employee : INotifyPropertyChanged
{
    private string name;
    private string position;
    private string? phone;
    private string email;
    private string? room;
    private string? mainWorkplace;
    private string? workplace;
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Position
    {
        get => position;
        set
        {
            if (position != value)
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
    }

    public string? Phone
    {
        get => phone;
        set
        {
            if (phone != value)
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
    }

    public string Email
    {
        get => email;
        set
        {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public string? Room
    {
        get => room;
        set
        {
            if (room != value)
            {
                room = value;
                OnPropertyChanged(nameof(Room));
            }
        }
    }

    public string? MainWorkplace
    {
        get => mainWorkplace;
        set
        {
            if (mainWorkplace != value)
            {
                mainWorkplace = value;
                OnPropertyChanged(nameof(MainWorkplace));
            }
        }
    }

    public string? Workplace
    {
        get => workplace;
        set
        {
            if (workplace != value)
            {
                workplace = value;
                OnPropertyChanged(nameof(Workplace));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}