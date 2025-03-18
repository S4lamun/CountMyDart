using System.ComponentModel;
using System.Xml.Serialization;

public class Player : INotifyPropertyChanged
{
    private string name; //Player Nickname
    private int targetPoints; //How many points player has to score to win
    private string throw1;
    private string throw2;
    private string throw3;
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

    public string Throw1
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

    public string Throw2
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

    public string Throw3
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

    public void DrawThrowsEasy() //for Bot
    {
        int throw1; int throw2; int throw3;
        int choice; // is made to do different levels
        int shouldEnd;

        Random random = new Random();

        throw1 = random.Next(2, 6);
        throw2 = random.Next(2, 6);
        throw3 = random.Next(2, 6); // if any condition isnt met

        choice = random.Next(1, 5);
        shouldEnd = random.Next(1, 4);

        if (TargetPoints < 40 && shouldEnd == 1) // to end game quicker
        {
            throw1 = targetPoints / 3;
            throw2 = targetPoints / 3;
            throw3 = targetPoints - throw1 - throw2;
        }

        if ((choice == 1 || choice == 2) && (shouldEnd == 2 || shouldEnd == 3))
        {
            throw1 = random.Next(1, 31);
            throw2 = random.Next(1, 31);
            throw3 = random.Next(1, 31);
        }
        if((choice ==3 || choice ==4) && (shouldEnd == 2 || shouldEnd == 3))
        {
            throw1 = random.Next(15, 46);
            throw2 = random.Next(15, 46);
            throw3 = random.Next(15, 46);
        }
       
        Throw1 = throw1.ToString();
        Throw2 = throw2.ToString();
        Throw3 = throw3.ToString();
        


    }
    public void DrawThrowsHard()
    {
        int throw1; int throw2; int throw3;
        int choice; // is made to do different levels
        int shouldEnd;
        Random random = new Random();

        throw1 = random.Next(5, 16);
        throw2 = random.Next(5, 16);
        throw3 = random.Next(5, 16); // if any condition isnt met

        choice = random.Next(1, 5);
        shouldEnd = random.Next(1, 4);

        if (TargetPoints < 60 && shouldEnd == 1) // to end game quicker
        {
            throw1 = targetPoints / 3;
            throw2 = targetPoints / 3;
            throw3 = targetPoints - throw1 - throw2;
        }

        if ((choice == 1 || choice == 2) && (shouldEnd==2 || shouldEnd == 3))
        {
            throw1 = random.Next(10, 61);
            throw2 = random.Next(10, 61);
            throw3 = random.Next(10, 61);
        }

        if ((choice == 3 || choice == 4) && (shouldEnd == 2 || shouldEnd == 3))
        {
            throw1 = random.Next(5, 41);
            throw2 = random.Next(5, 41);
            throw3 = random.Next(5, 41);
        }
       
        Throw1 = throw1.ToString();
        Throw2 = throw2.ToString();
        Throw3 = throw3.ToString();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
