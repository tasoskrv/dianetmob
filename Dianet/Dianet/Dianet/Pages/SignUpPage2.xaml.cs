﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Dianet.Pages
{
    public partial class SignUpPage2 : ContentPage
    {
        public SignUpPage2()
        {
            InitializeComponent();
        }
        
        public void OnClickProfilePage(object sender, EventArgs eventArgs)
        {
            App.Current.MainPage = new ProfilePage();
        }

        public void OnClickMainPage(object sender, EventArgs eventArgs)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}
