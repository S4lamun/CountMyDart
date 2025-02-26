using System;
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
        #endregion


        public AddPlayers(int numberOfPlayers, TypeOfGame gameType)
        {
            InitializeComponent();
            Id = 0;
            players = new();
            PlayerList.ItemsSource = null; 
            NumberOfPlayers = numberOfPlayers;
            TypeOfGame = gameType;
        }


        #region Buttons
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            string nick = PlayerNickname.Text.Trim().ToLower();
            nick = char.ToUpper(nick[0]) + nick.Substring(1);

            if (players.Any(t=>t.Name==nick))
            {
                MessageBox.Show("Player with this nickname is already in game!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Id++;
            players.Add(new Player(nick, (int)TypeOfGame));

            PlayerList.ItemsSource = null;
            PlayerList.ItemsSource = new ObservableCollection <Player> (players); // Updating View of Players

            if(Id<NumberOfPlayers)
            {
                MessageBox.Show($"Player {PlayerNickname.Text} was added! You must add {NumberOfPlayers - Id} more players to start!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show($"Player {PlayerNickname.Text} was added! You can start the game!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            PlayerNickname.Text = string.Empty;

        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if(Id!=NumberOfPlayers)
            {
                MessageBox.Show("You gave not enough players!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GamePage gamePage = new(TypeOfGame, players);
            Application.Current.MainWindow.Content = gamePage; // Opening gamePage 
        }

        #endregion
    }
}
