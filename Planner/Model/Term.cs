using System;
using SQLite;

namespace Planner.Model
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public DateTime Start { get; set; }

        [MaxLength(50)]
        public DateTime End { get; set; }
    }
}
