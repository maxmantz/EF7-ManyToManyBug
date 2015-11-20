using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ManyToManyDemo.Data;

namespace ManyToManyDemo.Migrations
{
    [DbContext(typeof(DemoContext))]
    [Migration("20151120111957_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ManyToManyDemo.Data.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ManyToManyDemo.Data.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ManyToManyDemo.Data.StudentCourse", b =>
                {
                    b.Property<int>("StudentId");

                    b.Property<int>("CourseId");

                    b.HasKey("StudentId", "CourseId");
                });

            modelBuilder.Entity("ManyToManyDemo.Data.StudentCourse", b =>
                {
                    b.HasOne("ManyToManyDemo.Data.Course")
                        .WithMany()
                        .ForeignKey("CourseId");

                    b.HasOne("ManyToManyDemo.Data.Student")
                        .WithMany()
                        .ForeignKey("StudentId");
                });
        }
    }
}
