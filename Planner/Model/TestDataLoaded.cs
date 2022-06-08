using System;
using SQLite;
namespace Planner.Model
{
    public class TestDataLoaded
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public bool Loaded { get; set; }
    }
}
