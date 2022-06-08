using System;
using System.Collections.Generic;
using Planner.Model;
using SQLite;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Planner
{
    public partial class CourseDetailPage : ContentPage
    {
        Course currentCourse;
        readonly List<String> statuses = new List<string>()
            {
                "In progress",
                "Completed",
                "Dropped",
                "Plan to take"
            };
        // Implement notes sharing through SMS.
        public CourseDetailPage(Course course)
        {
            InitializeComponent();
            currentCourse = course;
            courseTitle.Text = currentCourse.Name;
            courseDates.Text = $"{currentCourse.Start.ToLocalTime().ToShortDateString()} - {currentCourse.End.ToLocalTime().ToShortDateString()}";
            courseStatus.Text = $"Course Status: {statuses[currentCourse.Status]}";
            instructorName.Text = $"Instructor Name: {currentCourse.InstructorName}";
            instructorPhone.Text = $"Instructor Phone Number: {currentCourse.InstructorPhone}";
            instructorEmail.Text = $"Instructor Email: {currentCourse.InstructorEmail}";
            courseNotes.Text = $"Notes (Optional): {currentCourse.Notes}";
            perfCourseTitle.Text = currentCourse.PerformanceAssessmentTitle;
            perfCourseDates.Text = $"{currentCourse.PerfStart.ToLocalTime().ToShortDateString()} - {currentCourse.PerfEnd.ToLocalTime().ToShortDateString()}";
            objCourseTitle.Text = currentCourse.ObjAssessmentTitle;
            objCourseDates.Text = $"{currentCourse.ObjStart.ToLocalTime().ToShortDateString()} - {currentCourse.ObjEnd.ToLocalTime().ToShortDateString()}";
            if (string.IsNullOrEmpty(currentCourse.Notes))
            {
                shareButton.IsEnabled = false;
                shareButton.IsVisible = false;
            } else
            {
                shareButton.IsEnabled = true;
                shareButton.IsVisible = true;
            }
        }

        void DeleteButton_Clicked(System.Object sender, System.EventArgs e)
        {
            using (var connection = new SQLiteConnection(App.DbLocation))
            {
                connection.CreateTable<Course>();
                var courseRows = connection.Delete(currentCourse);

                if (courseRows > 0)
                {
                    DisplayAlert("Success", "Course successfully deleted.", "Ok");
                    Navigation.PushAsync(new MainPage());
                }
                else
                {
                    DisplayAlert("Failure", "Course failed to deleted.", "Ok");
                }
            }
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NewCoursePage(currentCourse));
        }

        void ShareButton_Clicked(System.Object sender, System.EventArgs e)
        {
            _ = SendEmail("Course Notes", currentCourse.Notes, new List<string>() { "" });
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                DisplayAlert("Can not send SMS", "This feature is not supported on your device.", "Ok");
            }
            catch (Exception ex)
            {
                DisplayAlert("SMS could not be sent.", $"Error: {ex.Message}", "Ok");
            }
        }
    }
}
