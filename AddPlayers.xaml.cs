using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountMyDart_
{
    public partial class AddPlayers : Page
    {
        #region GlobalVariables
        int Id; // Number of players already added
        List <Player> players;
        int NumberOfPlayers; // needed amount of players to start game
        TypeOfGame TypeOfGame;
        MainWindow mainWindow;
        #endregion


        public AddPlayers(int numberOfPlayers, TypeOfGame gameType, MainWindow mainWindow)
        {
            InitializeComponent();
            Id = 0;
            players = new();
            PlayerList.ItemsSource = null; 
            NumberOfPlayers = numberOfPlayers;
            TypeOfGame = gameType;
            this.mainWindow = mainWindow;
        }


        #region Buttons
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            string nick = PlayerNickname.Text.Trim();

            if(nick.Length<3 || string.IsNullOrEmpty(nick))
            {
                MessageBox.Show("Nickname is too short (minimum 3 letters)!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            nick = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nick); // making every first letter in word to be Capital letter
            
            if (players.Any(t=>t.Name==nick))
            {
                MessageBox.Show("Player with this nickname is already in game!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Id++;
            players.Add(new Player(nick, (int)TypeOfGame));

            PlayerList.ItemsSource = null;
            PlayerList.ItemsSource = new ObservableCollection <Player> (players); // Updating View of Players
            PlayerNickname.Text = string.Empty;

            if (Id<NumberOfPlayers)
            {
                MessageBox.Show($"Player {PlayerNickname.Text} was added! You must add {NumberOfPlayers - Id} more players to start!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show($"Player {PlayerNickname.Text} was added! You can start the game!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            

        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if(Id!=NumberOfPlayers)
            {
                MessageBox.Show("You gave not enough players!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //GamePage gamePage = new(TypeOfGame, players);
            //Application.Current.MainWindow.Content = gamePage; // Opening gamePage 
            mainWindow.MainFrame.Navigate(new GamePage(TypeOfGame, players, mainWindow));

            
        }

        #endregion
    }
}
