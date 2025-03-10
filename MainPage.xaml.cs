namespace CountMyDartMaui
{
    public enum GameType { Game101 = 101, Game201 = 201, Game301 = 301, Game401 = 401, Game501 = 501 }
    public partial class MainPage : ContentPage
    {
        #region GlobalVariables
        public List<string> PlayersAmount { get; set; } = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        public List<GameType> TypeOfGame { get; set; } = new List<GameType> { GameType.Game101, GameType.Game201, GameType.Game301, GameType.Game401, GameType.Game501 };

        int PlayersAmountChoosen; //How many players will play
        int GameTypeChoosen; // what type of game they will play
        #endregion


        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            GamePicker.SelectedIndex = 0;
            PlayerPicker.SelectedIndex = 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void PlayerAmountChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            string choose = picker.SelectedItem.ToString();
            if (!int.TryParse(choose, out int a))
            {
                await DisplayAlert("Error", "Something went wrong", "Ok");
            }
            else PlayersAmountChoosen = a;

        }

        private async void GameTypeChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            string choose = picker.SelectedItem.ToString();
            if (!Enum.TryParse(choose, out GameType a))
            {
                await DisplayAlert("Error", "Something went wrong", "Ok");
            }
            else GameTypeChoosen = (int)a;
        }

        private async void AcceptButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PlayerPage(GameTypeChoosen, PlayersAmountChoosen)); // Push Modal - you cant back to previous page
        }

    }

}
