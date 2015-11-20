using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToManyDemo
{
    using ManyToManyDemo.Data;

    using Microsoft.AspNet.Builder;

    public static class ExtensionMethods
    {
        public static void SeedData(this IApplicationBuilder app, Seeder seeder)
        {
            var build = app.UseOwin();

            build.Invoke(next => async env =>
                {
                    await seeder.SeedData();
                    await next(env);
                });
        } 
    }
}
