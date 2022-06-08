using System;
using Planner.Controller;
using Planner.Model;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Planner
{
    public partial class App : Application
    {
        
        public static string DbLocation = string.Empty;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public App(string dbLocation)
        {
            InitializeComponent();
            DbLocation = dbLocation;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Read from table.
            using (var conn = new SQLiteConnection(DbLocation))
            {
                conn.CreateTable<TestDataLoaded>();
                var result = conn.Table<TestDataLoaded>().ToList();
                var loaded = new TestDataLoaded() { Loaded = false };
                if (result.Count == 0)
                {
                    LoadTestData.LoadData(conn);
                    loaded.Loaded = true;
                    conn.CreateTable<TestDataLoaded>();
                    conn.Insert(loaded);
                }
                else if (result.Count > 0)
                {
                    if (!result[0].Loaded)
                    {
                        LoadTestData.LoadData(conn);
                        loaded.Loaded = true;
                        conn.CreateTable<TestDataLoaded>();
                        conn.Insert(loaded);
                    }
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
