using System.ComponentModel;

public class Player : INotifyPropertyChanged
{
    private string name; //Player Nickname
    private int targetPoints; //How many points player has to score to win
    private int throw1;
    private int throw2;
    private int throw3;
    private string place;
    private string endCommunicate; // if somebody didnt end game - it will tell you
    public event PropertyChangedEventHandler PropertyChanged;

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

    public int TargetPoints
    {
        get => targetPoints;
        set
        {
            if (targetPoints != value)
            {
                targetPoints = value;
                OnPropertyChanged(nameof(TargetPoints));
            }
        }
    }

    public int Throw1
    {
        get => throw1;
        set
        {
            if (throw1 != value)
            {
                throw1 = value;
                OnPropertyChanged(nameof(Throw1));
            }
        }
    }

    public int Throw2
    {
        get => throw2;
        set
        {
            if (throw2 != value)
            {
                throw2 = value;
                OnPropertyChanged(nameof(Throw2));
            }
        }
    }

    public int Throw3
    {
        get => throw3;
        set
        {
            if (throw3 != value)
            {
                throw3 = value;
                OnPropertyChanged(nameof(Throw3));
            }
        }
    }

    public string Place
    {
        get => place;
        set
        {
            if (place != value)
            {
                place = value;
                OnPropertyChanged(nameof(Throw3));
            }

        }
    }

    public string EndCommunicate
    {
        get => endCommunicate;
        set
        {
            if (endCommunicate != value)
            {
                endCommunicate = value;
                OnPropertyChanged(nameof(EndCommunicate));
            }

        }
    }

    public Player(string name, int targetPoints)
    {
        Name = name;
        TargetPoints = targetPoints;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
