using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountMyDart_;

public enum TypeOfGame
{ 
    Game101 = 101,
    Game201 = 201,
    Game301 = 301,
    Game501 = 501,
    Game701 = 701
}; // TypeOfGame in which we can play

public partial class MainWindow : Window
{
    int NumberOfPlayers;
    TypeOfGame GameType;
    public MainWindow()
    {
        InitializeComponent();

        int[] playerNumb = {2,3,4,5,6,7,8,9,10 }; // Array of available amount of player to play in game

        PlayersCombo.ItemsSource = playerNumb;
        GameCombo.ItemsSource = new ObservableCollection<TypeOfGame> {TypeOfGame.Game101, TypeOfGame.Game201, TypeOfGame.Game301, TypeOfGame.Game501, TypeOfGame.Game701 };

        PlayersCombo.SelectedIndex = 0; 
        GameCombo.SelectedIndex = 0; // making default choice in Combos
        
    }

    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AddPlayers addPlayersPage = new AddPlayers(NumberOfPlayers, GameType);
        Application.Current.MainWindow.Content = addPlayersPage;
    }


    #region ComboBoxes
    private void PlayersCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(PlayersCombo.SelectedItem is int selectedPlayer)
        {
            NumberOfPlayers = selectedPlayer;
        }
    }

    private void GameCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GameCombo.SelectedItem is TypeOfGame tog)
        {
            GameType = tog;
        }
    }
    #endregion

}