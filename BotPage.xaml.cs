

namespace CountMyDartMaui;

public partial class BotPage : ContentPage
{
    #region ForBot
    public Player Bot { get; set; }
    int BotThrow1;
    int BotThrow2;
    int BotThrow3;
    #endregion 

    #region ForPlayer
    public Player Player { get; set; }
    int Throw1;
    int Throw2;
    int Throw3;
    #endregion

    #region roundText 
    string roundText;

    public string RoundText
    {
        get => roundText;
        set
        {
            if (roundText != value)
            {
                roundText = value;
                OnPropertyChanged(nameof(RoundText));
            }
        }
    }

    int roundCounter; // counts how many rounds already passed
    #endregion // round number

    bool HardOrEasy; // type of bot if hard - true if easy - false

    public BotPage(int GameType)
	{
        InitializeComponent();
        if (GameType == 112) 
            HardOrEasy = true;
        else HardOrEasy = false;

        Bot = new Player("Bot", 501);
		Player = new Player("You", 501);

        Player.Throw1 = "0";
        Player.Throw2 = "0";
        Player.Throw3 = "0";
        roundCounter = 1;
        RoundText = "Round " + roundCounter;
		BindingContext = this;
	}

	private async void AcceptThrowsButtonClicked(object sender, EventArgs e)
	{
        #region Logic for Player
        if (!int.TryParse(Player.Throw1, out Throw1) || !int.TryParse(Player.Throw2, out Throw2) || !int.TryParse(Player.Throw3, out Throw3) || Throw1 < 0 || Throw1 > 60 || Throw2 <0 || Throw2 > 60 || Throw3<0 || Throw3>60  )
		{
            await DisplayAlert("Error", "Please enter a valid number", "OK");
            return;
        }
        if (Player.TargetPoints < (Throw1 + Throw2 + Throw3))
        {
            await DisplayAlert("Error", "You threw too much!", "Ok");
        }
        if (Player.TargetPoints >= (Throw1 + Throw2 + Throw3))
        {
            Player.TargetPoints -= (Throw1 + Throw2 + Throw3);
        }
        Player.Throw1 = "0";
        Player.Throw2 = "0";
        Player.Throw3 = "0";
        #endregion

        #region Logic for Bot
        if (HardOrEasy)
        {
            Bot.DrawThrowsHard();
        }
        else Bot.DrawThrowsEasy();
        
        BotThrow1 = int.Parse(Bot.Throw1);
        BotThrow2 = int.Parse(Bot.Throw2);
        BotThrow3 = int.Parse(Bot.Throw3);

        if (Bot.TargetPoints < (BotThrow1 + BotThrow2 + BotThrow3))
        {
            await DisplayAlert("Error", $"Bot scored: {Bot.Throw1}, {Bot.Throw2}, {Bot.Throw3}." + " Bot threw too much!", "Ok");
        }

        if (Bot.TargetPoints >= (BotThrow1 + BotThrow2 + BotThrow3))
        {
            Bot.TargetPoints -= (BotThrow1 + BotThrow2 + BotThrow3);
            await DisplayAlert("Information", $"Bot scored: {Bot.Throw1}, {Bot.Throw2}, {Bot.Throw3}", "OK");
        }
        #endregion

        if (Player.TargetPoints == 0)
        {
            await DisplayAlert("Success!", $"You won game in {roundCounter} rounds! Left to score by Bot: {Bot.TargetPoints}", "OK");
            await Navigation.PushModalAsync(new MainPage());
        }
       
        if (Bot.TargetPoints == 0)
        {
            await DisplayAlert("Failure!", $"You lost game! Bot ended in {roundCounter} rounds. Left to score: {Player.TargetPoints}", "OK");
            await Navigation.PushModalAsync(new MainPage());
        }

        RoundText = "Round " + ++roundCounter;
        OnPropertyChanged(nameof(RoundText));
    }

    private async void ThrowFocused(object sender, EventArgs e) // if throwEntry clicked then entry.Text is empty
    {
        if (sender is Entry entry)
        {
            
            if(Preferences.Get("Input Mode", "") == "Inserting manually")
            {
                entry.Text = string.Empty;
            }
            else if (Preferences.Get("Input Mode", "") == "Clicking on Board")
            {
                entry.Text = string.Empty;
                GlobalSettings.CurrentThrow = -1;

                try
                {
                    await Shell.Current.GoToAsync(nameof(InteractiveDartboard));

                    while (GlobalSettings.CurrentThrow == -1)
                    {
                        await Task.Delay(100);
                    }
                    entry.Text = GlobalSettings.CurrentThrow.ToString();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Navigation Error", ex.Message, "OK");
                }
            }
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private void EndGameButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new MainPage());
    }
}