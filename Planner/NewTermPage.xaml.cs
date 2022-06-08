using System;
using System.Collections.Generic;
using Planner.Model;
using SQLite;
using Xamarin.Forms;

namespace Planner
{
    public partial class NewTermPage : ContentPage
    {
        Term selectedTerm;
        public NewTermPage()
        {
            InitializeComponent();
        }

        public NewTermPage(Term selectedTerm)
        {
            InitializeComponent();
            this.selectedTerm = selectedTerm;
            pageTitleTxt.Text = "Update Term";
            termTitle.Text = selectedTerm.Title;
            startDate.Date = selectedTerm.Start.ToLocalTime();
            endDate.Date = selectedTerm.End.ToLocalTime();
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            if (endDate.Date < startDate.Date)
            {
                DisplayAlert("Error", "End Date must not be before Start Date.", "Ok");
            } else if (string.IsNullOrEmpty(termTitle.Text))
            {
                DisplayAlert("Title must not be empty", "Term Title must not be empty.", "Ok");
            } else
            {
                if (selectedTerm != null)
                {
                    selectedTerm.Title = termTitle.Text;
                    selectedTerm.Start = startDate.Date.ToUniversalTime();
                    selectedTerm.End = endDate.Date.ToUniversalTime();
                    using (var connection = new SQLiteConnection(App.DbLocation))
                    {
                        connection.CreateTable<Term>();

                        var rows = connection.Update(selectedTerm);
                        if (rows > 0)
                        {
                            DisplayAlert("Success", "Term successfully updated.", "Ok");
                            Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            DisplayAlert("Failure", "Term failed to updated.", "Ok");
                        }
                    }
                } else
                {
                    var term = new Term()
                    {
                        Title = termTitle.Text,
                        Start = startDate.Date.ToUniversalTime(),
                        End = endDate.Date.ToUniversalTime()
                    };
                    using (var connection = new SQLiteConnection(App.DbLocation))
                    {
                        connection.CreateTable<Term>();

                        var rows = connection.Insert(term);
                        if (rows > 0)
                        {
                            DisplayAlert("Success", "Term was created.", "Ok");
                            Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            DisplayAlert("Failure", "Failed to create new term.", "Ok");
                        }
                    }
                }
            }
        }
    }
}
