using System;
using System.Collections.Generic;
using Planner.Model;
using SQLite;
using Xamarin.Forms;

namespace Planner
{
    public partial class TermDetailPage : ContentPage
    {
        Term currentTerm;
        public TermDetailPage(Term selectedTerm)
        {
            InitializeComponent();
            currentTerm = selectedTerm;
            termTitle.Text = selectedTerm.Title;
            termDates.Text = $"{selectedTerm.Start.ToLocalTime().ToShortDateString()} - {selectedTerm.End.ToLocalTime().ToShortDateString()}"; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Read from table.
            using (var conn = new SQLiteConnection(App.DbLocation))
            {
                conn.CreateTable<Course>();
                var allCourses= conn.Table<Course>().Where(course => course.TermId == currentTerm.Id).ToList();
                courseListView.ItemsSource = allCourses;
                if (allCourses.Count >= 6)
                {
                    addButton.IsEnabled = false;
                    addButton.IsVisible = false;
                } else
                {
                    addButton.IsEnabled = true;
                    addButton.IsVisible = true;
                }
            }
        }

        void UpdateButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NewTermPage(currentTerm));
        }

        void DeleteButton_Clicked(System.Object sender, System.EventArgs e)
        {
            using (var connection = new SQLiteConnection(App.DbLocation))
            {
                connection.CreateTable<Term>();

                var rows = connection.Delete(currentTerm);
                if (rows > 0)
                {
                    DisplayAlert("Success", "Term successfully deleted.", "Ok");
                    Navigation.PushAsync(new MainPage());
                }
                else
                {
                    DisplayAlert("Failure", "Term failed to deleted.", "Ok");
                }
            }
        }

        void AddButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NewCoursePage(currentTerm));
        }

        void CourseListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var selectedCourse = courseListView.SelectedItem as Course;

            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseDetailPage(selectedCourse));
            }
        }
    }
}
