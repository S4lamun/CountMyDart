using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;

namespace CountMyDartMaui
{
    public partial class InteractiveDartboard : ContentPage
    {
        private readonly int[] SectorScores = { 6, 13, 4, 18, 1, 20, 5, 12, 9, 14, 11, 8, 16, 7, 19, 3, 17, 2, 15, 10 };

        private Label ScoreLabel;
        private Image DartboardImage;

        public InteractiveDartboard()
        {
            InitializeComponent();

            ScoreLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24,
                Margin = new Thickness(0, 10)
            };

            DartboardImage = new Image
            {
                Source = "dartboard2.svg",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFit
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnDartboardTapped;
            DartboardImage.GestureRecognizers.Add(tapGesture);

            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                },
                Children =
                {
                    new Grid
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = { DartboardImage }
                    },
                    ScoreLabel
                }
            };
        }

        private async void OnDartboardTapped(object sender, TappedEventArgs e)
        {
            var tapPoint = e.GetPosition((VisualElement)sender);

            if (tapPoint.HasValue)
            {
                int score = await CalculateScoreFromTapPosition(tapPoint.Value, ((VisualElement)sender).Width, ((VisualElement)sender).Height);
                GlobalSettings.CurrentThrow = score;
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task<int> CalculateScoreFromTapPosition(Point tapPoint, double width, double height)
        {
            double centerX = width / 2;
            double centerY = height / 2;
            double radius = Math.Min(centerX, centerY);

            double dx = tapPoint.X - centerX;
            double dy = centerY - tapPoint.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            double angle = Math.Atan2(dy, dx) * 180 / Math.PI;

            if (angle < 0)
                angle += 360;

            int sector = (int)((angle + 9) / 18) % 20;
            int score = SectorScores[sector];

            double bullseyeRadius = radius * 0.05;
            double innerBullRadius = radius * 0.125;
            double tripleRingInner = radius * 0.43;
            double tripleRingOuter = radius * 0.5;
            double doubleRingInner = radius * 0.70;
            double doubleRingOuter = radius * 0.80;

            string action=string.Empty; 

            if(distance<=innerBullRadius)
            {
                if(Preferences.Get("AppLanguage", "")=="en")
                {
                    action = await DisplayActionSheet("Choose how much you scored:","", "", "25", "50");
                    
                }
                else if(Preferences.Get("AppLanguage", "") == "pl")
                {
                    action = await DisplayActionSheet("Wybierz ile rzuci³eœ:", "", "", "25", "50");
                }
                else if(Preferences.Get("AppLanguage", "")=="de")
                {
                    action = await DisplayActionSheet("Wählen Sie aus, wie viele Punkte Sie erzielt haben:", "", "", "25", "50");
                }
                if (action == "25")
                {
                    return 25;
                }
                else if (action == "50")
                {
                    return 50;
                }

            }
            else if (distance <= doubleRingOuter)
            {
                if (Preferences.Get("AppLanguage", "") == "en")
                {
                    action = await DisplayActionSheet("Choose how much you scored:", "", "", $"{score}", $"{score*2}", $"{score*3}");

                }
                else if (Preferences.Get("AppLanguage", "") == "pl")
                {
                    action = await DisplayActionSheet("Wybierz ile rzuci³eœ:", "", "", $"{score}", $"{score * 2}", $"{score * 3}");
                }
                else if (Preferences.Get("AppLanguagee", "") == "de")
                {
                    action = await DisplayActionSheet("Wählen Sie aus, wie viele Punkte Sie erzielt haben:", "", "", $"{score}", $"{score * 2}", $"{score * 3}");
                }

                if(int.TryParse(action, out int d))
                    {
                    return d;
                }
            }

            return 0;
            /*
            if (distance <= bullseyeRadius)
                return 50; // Bullseye

            if (distance <= innerBullRadius)
                return 25; // Inner Bull

            if (distance >= tripleRingInner && distance <= tripleRingOuter)
                return score * 3; // Triple Ring

            if (distance >= doubleRingInner && distance <= doubleRingOuter)
                return score * 2; // Double Ring

            if (distance < doubleRingInner)
                return score; // Normal Sector

            return 0; // Beyond board\
            */
        }

        private void DisplayScore(int score)
        {
            ScoreLabel.Text = $"Score: {score}";
            GlobalSettings.CurrentThrow = score;
        }
    }
}