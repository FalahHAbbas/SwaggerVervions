using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerVervions;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public static readonly ILoggerFactory Factory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    public void ConfigureServices(IServiceCollection services) {

        services.AddSwaggerGen(genOptions =>
        {
            genOptions.SwaggerDoc("Path1",
                new OpenApiInfo{ Version = "Path1", Title = "Path1 API", });

            genOptions.SwaggerDoc("Path2",
                new OpenApiInfo{ Version = "Path2", Title = "Path2 API", });

            genOptions.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName.Contains("Path1")){
                    return apiDesc.RelativePath!.Contains("Path1/");
                }

                if (docName.Contains("Path2")){
                    return apiDesc.RelativePath!.Contains("Path2/");
                }

                return true;

            });

            genOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                In = ParameterLocation.Header,
                Description =
                    "Please enter into field the word 'Bearer' following by space and JWT",
                Name = "authorization",
                Type = SecuritySchemeType.ApiKey
            });
            genOptions.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                {
                    new OpenApiSecurityScheme{
                        Reference =
                            new OpenApiReference{
                                Type = ReferenceType.SecurityScheme, Id = "Bearer"
                            },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
    }

    public void Configure(IApplicationBuilder app,
        IWebHostEnvironment env /*,
        SseService sseService*/
    ) {
        //if (env.IsDevelopment())
        //{
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocExpansion(DocExpansion.None);
            c.SwaggerEndpoint($"/swagger/Path1/swagger.json", "Path1 API V1");
            c.SwaggerEndpoint($"/swagger/Path2/swagger.json", "Path2 API V1");
        });
        //    }


        app.UseAuthentication();
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthorization();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoint => endpoint.MapControllers());
    }
}