using System;
using System.Collections.Generic;
using Planner.Controller;
using Planner.Model;
using SQLite;
using Xamarin.Forms;

namespace Planner
{
    public partial class NewCoursePage : ContentPage
    {
        Term currentTerm;
        Course selectedCourse;
        readonly List<String> statuses = new List<string>()
            {
                "In progress",
                "Completed",
                "Dropped",
                "Plan to take"
            };

        // New course constructor
        public NewCoursePage(Term currentTerm)
        {
            InitializeComponent();
            this.currentTerm = currentTerm;
            courseStatus.ItemsSource = statuses;
            courseStatus.SelectedItem = statuses[0];
        }

        // Editing a course constructor
        public NewCoursePage(Course selectedCourse)
        {
            InitializeComponent();
            courseStatus.ItemsSource = statuses;
            courseStatus.SelectedItem = statuses[selectedCourse.Status];
            pageTitleTxt.Text = "Update Course";
            courseName.Text = selectedCourse.Name;
            startDate.Date = selectedCourse.Start.ToLocalTime();
            endDate.Date = selectedCourse.End.ToLocalTime();
            courseNotes.Text = selectedCourse.Notes;
            instructorEmail.Text = selectedCourse.InstructorEmail;
            instructorName.Text = selectedCourse.InstructorName;
            instructorPhone.Text = selectedCourse.InstructorPhone;
            perfAssessmentTitle.Text = selectedCourse.PerformanceAssessmentTitle;
            perfEndDate.Date = selectedCourse.PerfStart.ToLocalTime();
            perfEndDate.Date = selectedCourse.PerfEnd.ToLocalTime();
            objAssessmentTitle.Text = selectedCourse.ObjAssessmentTitle;
            objStartDate.Date = selectedCourse.ObjStart.ToLocalTime();
            objEndDate.Date = selectedCourse.ObjEnd.ToLocalTime();
            this.selectedCourse = selectedCourse;
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            // Implement notifications for start and end date of course and assessments. 
            if (string.IsNullOrEmpty(courseName.Text))
            {
                DisplayAlert("Course Name Required", "Course Name can not be empty.", "Ok");
            } else if (endDate.Date < startDate.Date)
            {
                DisplayAlert("Error", "End Date must not be before Start Date.", "Ok");
            } else if (string.IsNullOrEmpty(instructorName.Text))
            {
                DisplayAlert("Instructor Name Required", "Instructor Name can not be empty.", "Ok");
            } else if (string.IsNullOrEmpty(instructorPhone.Text))
            {
                DisplayAlert("Instructor Phone Number Required", "Instructor Phone Number can not be empty.", "Ok");
            } else if (string.IsNullOrEmpty(instructorEmail.Text))
            {
                DisplayAlert("Instructor Email Required", "Instructor Email can not be empty.", "Ok");
            } else if (string.IsNullOrEmpty(perfAssessmentTitle.Text))
            {
                DisplayAlert("Performance Assessment Title Required", "Performance Assessment Title can not be empty.", "Ok");
            }
            else if (perfEndDate.Date < perfStartDate.Date)
            {
                DisplayAlert("Error", "Performance End Date must not be before Performance Start Date.", "Ok");
            } else if (string.IsNullOrEmpty(objAssessmentTitle.Text))
            {
                DisplayAlert("Objective Assessment Title Required", "Objective Assessment Title can not be empty.", "Ok");
            } else if (objEndDate.Date < objStartDate.Date)
            {
                DisplayAlert("Error", "Objective End Date must not be before Objective Start Date.", "Ok");
            } else {
                if (selectedCourse != null)
                {
                    // Updating course
                    selectedCourse.Name = courseName.Text;
                    selectedCourse.Start = startDate.Date.ToUniversalTime();
                    selectedCourse.End = endDate.Date.ToUniversalTime();
                    selectedCourse.Status = courseStatus.SelectedIndex;
                    selectedCourse.Notes = courseNotes.Text;
                    selectedCourse.InstructorEmail = instructorEmail.Text;
                    selectedCourse.InstructorName = instructorName.Text;
                    selectedCourse.InstructorPhone = instructorPhone.Text;
                    selectedCourse.PerformanceAssessmentTitle = perfAssessmentTitle.Text;
                    selectedCourse.PerfStart = perfStartDate.Date.ToUniversalTime();
                    selectedCourse.PerfEnd = perfEndDate.Date.ToUniversalTime();
                    selectedCourse.ObjAssessmentTitle = objAssessmentTitle.Text;
                    selectedCourse.ObjStart = objStartDate.Date.ToUniversalTime();
                    selectedCourse.ObjEnd = objEndDate.Date.ToUniversalTime();

                    using (var connection = new SQLiteConnection(App.DbLocation))
                    {
                        connection.CreateTable<Course>();

                        var courseRows = connection.Update(selectedCourse);

                        if (courseRows > 0)
                        {
                            DisplayAlert("Success", $"Course successfully updated.", "Ok");
                            Notifications.DeleteNotifications(new List<int> { selectedCourse.CourseStartNotif, selectedCourse.CourseEndNotif, selectedCourse.PerfStartNotif, selectedCourse.PerfEndNotif, selectedCourse.ObjStartNotif, selectedCourse.ObjEndNotif });
                            Notifications.SetNotifications(selectedCourse);
                            Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            DisplayAlert("Failure", "Course failed to updated.", "Ok");
                        }
                    }
                } else
                {
                    var newCourse = new Course()
                    {
                        TermId = currentTerm.Id,
                        Name = courseName.Text,
                        Start = startDate.Date.ToUniversalTime(),
                        End = endDate.Date.ToUniversalTime(),
                        Status = courseStatus.SelectedIndex,
                        InstructorEmail = instructorEmail.Text,
                        InstructorName = instructorName.Text,
                        InstructorPhone = instructorPhone.Text,
                        Notes = courseNotes.Text,
                        PerformanceAssessmentTitle = perfAssessmentTitle.Text,
                        PerfStart = perfStartDate.Date.ToUniversalTime(),
                        PerfEnd = perfEndDate.Date.ToUniversalTime(),
                        ObjAssessmentTitle = objAssessmentTitle.Text,
                        ObjStart = objStartDate.Date.ToUniversalTime(),
                        ObjEnd = objEndDate.Date.ToUniversalTime()
                    };
                    // Creating a new course
                    using (var connection = new SQLiteConnection(App.DbLocation))
                    {
                        connection.CreateTable<Course>();

                        var courseRows = connection.Insert(newCourse);

                        if (courseRows > 0)
                        {
                            DisplayAlert("Success", $"Course was created.", "Ok");
                            Notifications.SetNotifications(newCourse);
                            Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            DisplayAlert("Failure", "Failed to create new course.", "Ok");
                        }
                    }
                }
            }
        }
    }
}