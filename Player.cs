using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

public class Player // class which represents single player
{

    string name;
    int targetPoints; // needed amount of points
    private int throw1;
    private int throw2;
    private int throw3;

    public string Name { get => name; set => name = value; }
    public int TargetPoints { get => targetPoints; set => targetPoints = value; }
    public int Throw1 { get => throw1; set => throw1 = value; }
    public int Throw2 { get => throw2; set => throw2 = value; }
    public int Throw3 { get => throw3; set => throw3 = value; }

    public Player(string name, int targetPoints)
    {
        Name = name;
        TargetPoints = targetPoints;
    }

    public override string ToString()
    {
        return Name;
    }
}
