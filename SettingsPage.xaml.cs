using System.Collections.ObjectModel;

namespace CountMyDartMaui;

public partial class SettingsPage : ContentPage
{
    #region GlobalVaribles
    public ObservableCollection<string> languages { get; set; }
    public string selectedLanguage { get; set; }

    public ObservableCollection<string> inputModes { get; set; }
    public string selectedInputMode { get; set; }
    #endregion

    public SettingsPage()
    {
        InitializeComponent();

        languages = new ObservableCollection<string>
            {
                "English",
                "Polski",
                "Deutsh"
            };

        inputModes = new ObservableCollection<string>
            {
                "Inserting manually",
                "Clicking on Board"
            };

        selectedLanguage = Preferences.Get("Language", "");
        selectedInputMode = Preferences.Get("Input Mode", "");
        BindingContext = this;
    }

    private async void ButtonClicked(object sender, EventArgs e)
    { 
        await Navigation.PushModalAsync(new MainPage());
    }

    private void InputSelectedChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        int index = picker.SelectedIndex;
        Preferences.Set("Input Mode", inputModes[index]);
    }

    private void LanguageSelectedChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        int index = picker.SelectedIndex;
        Preferences.Set("Language", languages[index]);
    }
}
