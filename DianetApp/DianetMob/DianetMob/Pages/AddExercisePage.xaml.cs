using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;

using Xamarin.Forms;

namespace DianetMob.Pages
{

    public partial class AddExercisePage : ContentPage
    {
        private DateTime SelectedDate;
        private Exercise exercise;
        private SQLiteConnection conn = null;

        public AddExercisePage()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
        }

        protected override void OnAppearing()
        {
            exerciseMins.Focus();
        }


        public async void OnSaveExerciseBtnClicked(object sender, EventArgs eventArgs)
        {
            exercise.TrainDate = SelectedDate;
            exercise.UpdateDate = DateTime.UtcNow;

            if (exerciseMins.Text.Equals("") || exerciseMins.Text == null)
            {
                await DisplayAlert("Please", "Fill minutes", "OK");
            }
            else
            {
                if (exercise.IDExercise > 0)
                {
                    StorageManager.UpdateData<Exercise>(exercise);
                }
                else
                {
                    exercise.InsertDate = exercise.UpdateDate;
                    StorageManager.InsertData<Exercise>(exercise);
                }
                await Navigation.PopToRootAsync(true);
            }
        }

        public void LoadData(DateTime date,int IDExercise = 0)
        {
            SelectedDate = date;
            exercise = new Exercise();
            exercise.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            BindingContext = exercise;

            if (IDExercise > 0)
                exercise = conn.Get<Exercise>(IDExercise);
            else
            {
                exercise = new Exercise();
                exercise.IDUser = StorageManager.GetConnectionInfo().LoginUser.IDUser;
            }
            BindingContext = exercise;
        }


    }
}
