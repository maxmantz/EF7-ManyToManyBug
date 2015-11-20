namespace ManyToManyDemo.Data
{
    using Microsoft.Data.Entity;

    /// <summary>
    /// The demo context.
    /// </summary>
    public class DemoContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
