using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToManyDemo.Data
{
    public class StudentCourse
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public StudentCourse()
        {
        }

        public StudentCourse(Student student, Course course)
        {
            StudentId = student.Id;
            Student = student;
            CourseId = course.Id;
            Course = course;
        }
    }
}
