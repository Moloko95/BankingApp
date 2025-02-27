using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BankingApp.ViewModels
{
    public class ProfileViewModel : BindableObject
    {
        private DatabaseService dbService;
        private Profile profile;

        public Profile Profile
        {
            get => profile;
            set
            {
                profile = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public ProfileViewModel()
        {
            dbService = new DatabaseService();
            LoadProfile();
            SaveCommand = new Command(SaveProfile);
        }

        private void LoadProfile()
        {
            var db = dbService.GetConnection();
            Profile = db.Table<Profile>().FirstOrDefault() ?? new Profile();
        }

        private async void SaveProfile()
        {
            if (string.IsNullOrEmpty(Profile.Name) || string.IsNullOrEmpty(Profile.Surname) ||
                string.IsNullOrEmpty(Profile.EmailAddress) || string.IsNullOrEmpty(Profile.Bio))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields are required. Please fill in all the fields.", "OK");
                return;
            }

            var db = dbService.GetConnection();
            db.InsertOrReplace(Profile);
            await Application.Current.MainPage.DisplayAlert("Success", "Profile saved successfully!", "OK");
        }
    }
}
