using System.Collections.ObjectModel;

namespace CountMyDartMaui;

public partial class ScoreboardPage : ContentPage
{
    Dictionary<Player, int> PlayerScoreboard;
    public new ObservableCollection<Player> Players { get; set; }
    int Counter;
    public ScoreboardPage(Dictionary<Player, int> playerScoreboard)
    {
        InitializeComponent();


        PlayerScoreboard = playerScoreboard;
        Counter = 2;
        Players = new();

        if (!playerScoreboard.Keys.Any(x => x.EndCommunicate == "Ended game"))
        {
            WinnerText.Text = "No winner!";
            WinnerRounds.Text = string.Empty;
            return;
        }

        var winner = playerScoreboard.OrderBy(x => x.Value).First();
        WinnerText.Text = winner.Key.Name;
        WinnerRounds.Text = $"Won in {winner.Value} rounds!";

        PlayerScoreboard.Remove(winner.Key);


        foreach (var a in PlayerScoreboard)
        {
            a.Key.Name = a.Key.Name + $" ({a.Value})";
            Players.Add(a.Key);
            a.Key.Place = $"{Counter}.";
            Counter++;
        }

        BindingContext = this;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private async void StartNewGameButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }
}