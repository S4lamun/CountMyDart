using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CountMyDartMaui.Resources.Styles
{
   public static class LocalizationService
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("TwojaAplikacja.Resources.Localization.AppResources", typeof(LocalizationService).Assembly);

        public static string GetString(string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }

        public static void SetLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
