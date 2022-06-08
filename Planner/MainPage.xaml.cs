using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planner.Model;
using SQLite;
using Xamarin.Forms;

namespace Planner
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Read from table.
            using (var conn = new SQLiteConnection(App.DbLocation))
            {
                conn.CreateTable<Term>();
                var terms = conn.Table<Term>().ToList();
                termListView.ItemsSource = terms;
            }
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NewTermPage());
        }

        void TermListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var selectedTerm = termListView.SelectedItem as Term;

            if (selectedTerm != null)
            {
                Navigation.PushAsync(new TermDetailPage(selectedTerm));
            }
        }
    }
}
