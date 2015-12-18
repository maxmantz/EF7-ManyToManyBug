using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToManyDemo.Data
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Logging;

    public class Seeder
    {
        private readonly DemoContext ctx;

        private ILogger<Seeder> logger;

        public Seeder(DemoContext ctx, ILogger<Seeder> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public async Task SeedData()
        {
            if (await this.ctx.Students.CountAsync() > 0) return;

            for (int i = 1; i <= 100; i++)
            {
                var student = new Student { Name = $"Student {i}" };
                var course = new Course { Name = $"Course {i}" };

                this.ctx.Students.Add(student);
                this.ctx.Courses.Add(course);
                this.logger.LogInformation($"Adding Student {student.Name}");
                this.logger.LogInformation($"Adding Course {course.Name}");
            }

            logger.LogInformation("Saving...");
            await this.ctx.SaveChangesAsync();

            var students = await this.ctx.Students.ToListAsync();

            foreach (var student in students)
            {
                for (int i = 1; i <= 50; i++)
                {
                    var course = await this.ctx.Courses.Where(c => c.Name.Equals($"Course {i}")).FirstOrDefaultAsync();

                    if (course != null)
                    {
                        var studentCourse = new StudentCourse(student, course);
                        this.ctx.StudentCourses.Add(studentCourse);
                        this.logger.LogInformation($"Linking {student.Name} with {course.Name}");
                    }
                }
            }

            logger.LogInformation("Saving...");
            await this.ctx.SaveChangesAsync();
            this.logger.LogInformation("Done!");
        }
    }
}
