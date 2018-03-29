using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using UsersMicroservice.Data;

namespace UsersMicroservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connStr = Configuration.GetConnectionString("DefaultConnection");
            //System.Console.WriteLine("The connection string is: {0}\n",connStr);
            services.AddDbContext<AppDbContext>(options => 
                                                options.UseSqlServer(connStr));

            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MSUser", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.  
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSUser");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                          name: "default",
                          template: "{controller=Todo}/{action=Index}/{id?}");
            });
        }
    }
}
