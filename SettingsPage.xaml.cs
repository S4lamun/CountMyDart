using System.Collections.ObjectModel;

namespace CountMyDartMaui;

public partial class SettingsPage : ContentPage
{
    #region GlobalVaribles
    public ObservableCollection<string> languages { get; set; }

    private string _selectedLanguage;
    public string selectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage != value)
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(selectedLanguage));
            }
        }
    }

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

        var selected = Preferences.Get("AppLanguage", "");
        if(selected == "en")
        {
            selectedLanguage = languages[0];
        }
        else if(selected == "pl")
        {
            selectedLanguage = languages[1];
        }
        else if(selected == "de")
        {
            selectedLanguage = languages[2];
        }
            selectedInputMode = Preferences.Get("Input Mode", "");
        BindingContext = this;
    }

    private async void ButtonClicked(object sender, EventArgs e)
    {
        MainPage.LoadCulture();
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
        string lang = picker.SelectedItem.ToString();
        if(lang == "Deutsh")
            Preferences.Set("AppLanguage", "de");
        
        else if (lang == "Polski")
            Preferences.Set("AppLanguage", "pl");

        else Preferences.Set("AppLanguage", "en");
    }
}
