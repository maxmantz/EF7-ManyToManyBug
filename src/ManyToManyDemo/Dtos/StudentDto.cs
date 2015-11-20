using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToManyDemo.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<int> Courses { get; set; }
    }
}
