using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
    public partial class GamePage : Page
    {
        #region GlobalVariables
        TypeOfGame TypeOfGame;
        List<Player> Players;
        int NumberOfPlayers;
        List<Button> acceptButtons;
        int RoundId;
        #endregion

        public GamePage(TypeOfGame typeOfGame, List<Player> players)
        {
            InitializeComponent();
            TypeOfGame = typeOfGame;
            Players = players;
            NumberOfPlayers = players.Count();
            acceptButtons = new();
            RoundId = 1;

            #region View Making
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            StackPanel finalPanel = new StackPanel { Orientation = Orientation.Horizontal };

            StackPanel stackPanel = new  StackPanel();
            TextBlock roundBlock = new TextBlock {Width = 100, TextAlignment = TextAlignment.Center, FontSize = 20, Text = $"Round: {RoundId}"};
            stackPanel.Children.Add(roundBlock);
            scrollViewer.Content = finalPanel;

            foreach (var p in players)
            {
                StackPanel playerPanel = new StackPanel { Orientation = Orientation.Horizontal };

                StackPanel namePanel = new StackPanel { Orientation = Orientation.Vertical };

                TextBlock nameDescriptionBlock = new TextBlock { Width = 100, TextAlignment = TextAlignment.Center };
                nameDescriptionBlock.Text = "Player's name:";

                TextBlock nameBlock = new TextBlock { Width = 100, TextAlignment = TextAlignment.Center, FontSize = 16 };
                nameBlock.Text = p.Name;

                namePanel.Children.Add(nameDescriptionBlock);
                namePanel.Children.Add(nameBlock);


                StackPanel targetPanel = new StackPanel { Orientation = Orientation.Vertical };

                TextBlock targetDescriptionBlock = new TextBlock { Width = 100, TextAlignment = TextAlignment.Center };
                targetDescriptionBlock.Text = "Points to Gain:";

                TextBlock targetBlock = new TextBlock { Width = 100, TextAlignment = TextAlignment.Center, FontSize = 16 };
                targetBlock.Text = p.TargetPoints.ToString();

                targetPanel.Children.Add(targetDescriptionBlock);
                targetPanel.Children.Add(targetBlock);

                StackPanel throwPanel = new StackPanel { Orientation = Orientation.Vertical };
                
                TextBlock throwDescription = new TextBlock{Text = "Throws:", TextAlignment = TextAlignment.Center, FontSize=16 };
                throwPanel.Children.Add(throwDescription);

                TextBox throw1Block = new TextBox { Width = 100, Height = 50, FontSize = 24, TextAlignment = TextAlignment.Center };
                throw1Block.Text = p.Throw1.ToString();
                throw1Block.GotFocus += (sender, e) => Throw1Block_GotFocus(sender, e, throw1Block);

                TextBox throw2Block = new TextBox { Width = 100, Height = 50, FontSize = 24, TextAlignment = TextAlignment.Center };
                throw2Block.Text = p.Throw2.ToString();
                throw2Block.GotFocus += (sender, e) => Throw2Block_GotFocus(sender, e, throw2Block);

                TextBox throw3Block = new TextBox { Width = 100, Height = 50, FontSize = 24 , TextAlignment = TextAlignment.Center };
                throw3Block.Text = p.Throw3.ToString();
                throw3Block.GotFocus += (sender, e) => Throw3Block_GotFocus(sender, e, throw3Block);

                StackPanel throw2Panel = new StackPanel { Orientation = Orientation.Horizontal};
                throw2Panel.Children.Add(throw1Block);
                throw2Panel.Children.Add(throw2Block);
                throw2Panel.Children.Add(throw3Block);

                throwPanel.Children.Add(throw2Panel);
                Border separator = new Border { Height = 10 }; // to make distance between 2 players

                Button acceptButton = new Button { Visibility = Visibility.Hidden };
                acceptButton.Click += (sender, e) => AcceptButton_Click(sender, e, p, throw1Block.Text, throw2Block.Text, throw3Block.Text, targetBlock); //lamba expression to give Player as param
                acceptButtons.Add(acceptButton); // adding to list of button

                playerPanel.Children.Add(namePanel);
                playerPanel.Children.Add(targetPanel);
                playerPanel.Children.Add(throwPanel);
                playerPanel.Children.Add(acceptButton);
                stackPanel.Children.Add(separator);

                stackPanel.Children.Add(playerPanel); // Adding everything to StackPanels

            }

            Button scoreButton = new Button { Width = 130, Height = 50, Content = "Accept all scores" };
            scoreButton.Click += (sender, e) => ScoreButton_Click(sender, e, roundBlock);


            finalPanel.Children.Add(stackPanel);
            finalPanel.Children.Add(scoreButton);
            this.Content = scrollViewer;
            #endregion


        }


        #region Buttons
        private void AcceptButton_Click(object sender, RoutedEventArgs e, Player player, string throw1, string throw2, string throw3, TextBlock targetBlock)
        {
            if(!int.TryParse(throw1, out int a) || a < 0 || a > 60)
            {
                MessageBox.Show($"Invalid input! (Player {player.Name})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(throw2, out int b) || b<0||b>60)
            {
                MessageBox.Show($"Invalid input! (Player {player.Name})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(throw3, out int c) || c < 0 || c > 60)
            {
                MessageBox.Show($"Invalid input! (Player {player.Name})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(player.TargetPoints<(a+b+c))
            {
                //MessageBox.Show("You threw too much!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            player.TargetPoints = player.TargetPoints - a - b - c;
            targetBlock.Text = $"{player.TargetPoints}";

            if (player.TargetPoints == 0)
            {
                MessageBox.Show($"Player: {player.Name} won!", "WINNER", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
      
        }

        private void ScoreButton_Click(object sender, RoutedEventArgs e, TextBlock roundBlock)
        {
            
            foreach (var b in acceptButtons)
            {
                b.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); // making all AcceptButtons to be Clicked
                
            }
            
            roundBlock.Text = $"Round {++RoundId}";
        }
        #endregion


        #region GotFocus Functions
        private void Throw1Block_GotFocus(object sender, RoutedEventArgs e, TextBox throw1Block)
        {
            throw1Block.Text = string.Empty;
        }

        private void Throw2Block_GotFocus(object sender, RoutedEventArgs e, TextBox throw2Block)
        {
            throw2Block.Text = string.Empty;
        }

        private void Throw3Block_GotFocus(object sender, RoutedEventArgs e, TextBox throw3Block)
        {
            throw3Block.Text = string.Empty;
        }
        #endregion
    }
}
