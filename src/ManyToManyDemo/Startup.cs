using Microsoft.AspNet.Builder;

namespace ManyToManyDemo
{
    using AutoMapper;

    using ManyToManyDemo.Data;
    using ManyToManyDemo.Dtos;

    using Microsoft.Data.Entity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Mapper.Initialize(
                config =>
                    {
                        config.CreateMap<Student, StudentDto>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
                        config.CreateMap<Course, CourseDto>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

                        config.CreateMap<Student, int>().ConstructUsing(s => s.Id);
                        config.CreateMap<Course, int>().ConstructUsing(c => c.Id);
                    });

            services.AddTransient<Seeder>();

            services.AddLogging();
    
            services.AddEntityFramework()
                    .AddSqlServer()
                    .AddDbContext<DemoContext>(
                        o => o.UseSqlServer(@"Server=.\SQLEXPRESS;Database=ManyToManyDemo;integrated security=True;"));

            services.AddMvc();
        }

        public async void Configure(IApplicationBuilder app, Seeder seeder, ILoggerFactory loggerFactory)
        {
            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            loggerFactory.AddDebug(LogLevel.Information);

            app.SeedData(seeder);

            app.UseMvcWithDefaultRoute();
        }
    }
}
