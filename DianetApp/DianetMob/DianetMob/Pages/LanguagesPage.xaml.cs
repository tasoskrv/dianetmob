using DianetMob.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DianetMob.DB.Entities;
using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class LanguagesPage : ContentPage
    {
        private List<Lang> langs = new List<Lang>();
        private Settings settings;
        public LanguagesPage()
        {
            InitializeComponent();
            langs.Add(new Lang() { ID = 1, Name = "Ελληνική", LangCode = "el-gr" });
            langs.Add(new Lang() { ID = 2, Name = "English", LangCode = "en-GB" });
            LangPicker.ItemsSource = langs;
            ConnectionInfo info = StorageManager.GetConnectionInfo();
            settings = info.Settings;
        }

        private void OnSelectLangButtonClicked(object sender, EventArgs e)
        {
            settings.Lang = ((Lang)LangPicker.SelectedItem).ID;
            StorageManager.UpdateData<Settings>(settings);
            App.Current.MainPage = new LoginPage();
        }
    }
    public class Lang
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LangCode { get; set; }
    }
}
