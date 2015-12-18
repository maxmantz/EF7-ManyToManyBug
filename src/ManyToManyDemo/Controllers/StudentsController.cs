using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ManyToManyDemo.Controllers
{
    using AutoMapper;

    using ManyToManyDemo.Data;
    using ManyToManyDemo.Dtos;

    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Query.ExpressionTranslators;

    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        /// <summary>
        /// The demo context.
        /// </summary>
        private readonly DemoContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentsController"/> class.
        /// </summary>
        /// <param name="ctx">
        /// The ctx.
        /// </param>
        public StudentsController(DemoContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// The get method. Works with a limit of anything below 60, but trying a limit of 60 or higher gives out the buggy data.
        /// </summary>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="limit">
        /// The limit. Works fine with 50 or less. Doesn't work with 100.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 100)
        {
            var students = await this.ctx.Students
                .Where(s => true)
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            var dtos = new List<StudentDto>();

            foreach (var student in students)
            {
                var dto = Mapper.Map<StudentDto>(student);
                var courseIds = student.StudentCourses.Select(sc => sc.CourseId);
                dto.Courses = courseIds.ToList();
                dtos.Add(dto);
            }

            return this.Ok(dtos);
        }

        /// <summary>
        /// The alternative where the Skip() and Take() are used directly after the predicate. Produces the same bug.
        /// </summary>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("alternative")]
        public async Task<IActionResult> Alternative(int offset = 0, int limit = 100)
        {
            var students =
                await
                this.ctx.Students.Where(s => true)
                    .Skip(offset)
                    .Take(limit)
                    .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                    .ToListAsync();

            var dtos = new List<StudentDto>();

            foreach (var student in students)
            {
                var dto = Mapper.Map<StudentDto>(student);
                var courseIds = student.StudentCourses.Select(sc => sc.CourseId);
                dto.Courses = courseIds.ToList();
                dtos.Add(dto);
            }

            return this.Ok(dtos);
        }

        /// <summary>
        /// This method works because the limit and offset are applied AFTER the database query has been made.
        /// But this defeats the usefulness of the offset and limit, because every record is loaded from the database regardless.
        /// NOTE: the order of the IDs in the courses list is inconsistent, but I don't know whether this is also a bug.
        /// </summary>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("works")]
        public async Task<IActionResult> Works(int offset = 0, int limit = 100)
        {
            var students =
                await
                this.ctx.Students.Where(s => true)
                    .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                    .ToListAsync();

            students = students.Skip(offset).Take(limit).ToList();

            var dtos = new List<StudentDto>();

            foreach (var student in students)
            {
                var dto = Mapper.Map<StudentDto>(student);
                var courseIds = student.StudentCourses.Select(sc => sc.CourseId);
                dto.Courses = courseIds.ToList();
                dtos.Add(dto);
            }

            return this.Ok(dtos);
        }

        /// <summary>
        /// This method uses no prediate (Where()) but has the same bug.
        /// </summary>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("nopredicate")]
        public async Task<IActionResult> NoPredicate(int offset = 0, int limit = 100)
        {
            var students =
                await
                this.ctx.Students
                    .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

            students = students.Skip(offset).Take(limit).ToList();

            var dtos = new List<StudentDto>();

            foreach (var student in students)
            {
                var dto = Mapper.Map<StudentDto>(student);
                var courseIds = student.StudentCourses.Select(sc => sc.CourseId);
                dto.Courses = courseIds.ToList();
                dtos.Add(dto);
            }

            return this.Ok(dtos);
        }
    }
}
