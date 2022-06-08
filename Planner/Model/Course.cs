using System;
using System.ComponentModel;
using SQLite;

namespace Planner.Model
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int TermId { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int CourseStartNotif { get; set; }

        public int CourseEndNotif { get; set; }

        public int Status { get; set; }

        public string InstructorName { get; set; }

        public string InstructorPhone { get; set; }

        public string InstructorEmail { get; set; }

        public string Notes { get; set; }

        public string PerformanceAssessmentTitle { get; set; }

        public DateTime PerfStart { get; set; }

        public DateTime PerfEnd { get; set; }

        public int PerfStartNotif { get; set; }

        public int PerfEndNotif { get; set; }

        public string ObjAssessmentTitle { get; set; }

        public DateTime ObjStart { get; set; }

        public DateTime ObjEnd { get; set; }

        public int ObjStartNotif { get; set; }

        public int ObjEndNotif { get; set; }
    }
}
