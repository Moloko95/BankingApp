using BankingApp.Models;
using SQLite;
using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace BankingApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profile.db3");

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Profile Picture",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    ProfileImage.Source = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to upload image: " + ex.Message, "OK");
            }
        }

        private void LoadProfile()
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Profile>();
            var profile = db.Table<Profile>().FirstOrDefault();

            if (profile != null)
            {
                NameEntry.Text = profile.Name;
                SurnameEntry.Text = profile.Surname;
                EmailEntry.Text = profile.EmailAddress;
                BioEditor.Text = profile.Bio;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameEntry.Text) || string.IsNullOrEmpty(SurnameEntry.Text) ||
                string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(BioEditor.Text))
            {
                await DisplayAlert("Error", "All fields are required. Please fill in all the fields.", "OK");
                return;
            }

            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Profile>();

            var profile = new Profile
            {
                Name = NameEntry.Text,
                Surname = SurnameEntry.Text,
                EmailAddress = EmailEntry.Text,
                Bio = BioEditor.Text
            };

            db.InsertOrReplace(profile);
            await DisplayAlert("Success", "Profile saved successfully!", "OK");
            await Navigation.PushAsync(new ProductsPage());
        }
    }
}
