using Kimmen.Patterns.Api.ApiExplorer;
using Kimmen.Patterns.Api.Controllers;
using Kimmen.Patterns.Api.Typing;
using Kimmen.Patterns.Core;
using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRequestTypeProvider, ReflectedRequestTypeProvider>();

            services.AddMediatR(typeof(IHandlerResponse).Assembly);

            //NOTE:  Custom ApiDescription group collection provider to be able generate proper swagger file.
            services.AddSingleton<IApiDescriptionGroupCollectionProvider, HandlerRequestApiGroupCollectionProvider<ApiController>>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",new Info {Description = "myApi", Title = "Api api"});
                //NOTE: All requests has the same name using this class-as-namespace convention, we need to provide a new schema-id generator
                options.CustomSchemaIds(type => type.DeclaringType != null 
                    ? $"{type.DeclaringType.Name}.{type.Name}" 
                    : type.Name);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
