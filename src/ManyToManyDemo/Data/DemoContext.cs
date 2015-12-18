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
            modelBuilder.Entity<Student>().HasMany(e => e.StudentCourses);
            modelBuilder.Entity<Course>().HasMany(e => e.StudentCourses);
            modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
