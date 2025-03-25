using System.Collections.ObjectModel;

namespace CountMyDartMaui;

public partial class SettingsPage : ContentPage
{

    public ObservableCollection<string> languages { get; set; }
    public string selectedLanguage { get; set; }

    public ObservableCollection<string> inputModes { get; set; }
    public string selectedInputMode { get; set; }

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

        selectedLanguage = languages[0];
        selectedInputMode = inputModes[0];
        BindingContext = this;
    }

    private async void ButtonClicked(object sender, EventArgs e)
    {
        GlobalSettings.SelectedLanguage = selectedLanguage;
        GlobalSettings.SelectedInputMode = selectedInputMode;
        await Navigation.PushModalAsync(new MainPage());
    }

    private void InputSelectedChange(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        int index = picker.SelectedIndex;
        selectedInputMode = inputModes[index];
    }

    private void LanguageSelectedChange(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        int index = picker.SelectedIndex;
        selectedLanguage = languages[index];
    }
}
