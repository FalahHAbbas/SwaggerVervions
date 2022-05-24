using System.Reflection;
using Microsoft.AspNetCore.Mvc;
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

    private List<string?> GetControllerNameSpaces() {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var controllerNameSpaces = (from type in types where type.IsClass && type.CustomAttributes.Any(x => x.AttributeType == typeof(ApiControllerAttribute)) select type.Namespace!).ToList();
        return controllerNameSpaces.Distinct().ToList();
    }

    public void ConfigureServices(IServiceCollection services) {
        var controllersNameSpaces = GetControllerNameSpaces();
        services.AddSwaggerGen(genOptions =>
        {
            controllersNameSpaces.ForEach(x => genOptions.SwaggerDoc(x,
                new OpenApiInfo{
                    Title = x?[x.LastIndexOf(".", StringComparison.Ordinal)..],
                    Version = x
                }));

            genOptions.DocInclusionPredicate((docName, apiDesc) =>
            {
                foreach (var x in controllersNameSpaces.Where(docName.Contains!)){
                    return apiDesc.ActionDescriptor.DisplayName!.Contains(x!);
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
        IWebHostEnvironment env
    ) {
        var controllersNameSpaces = GetControllerNameSpaces();
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocExpansion(DocExpansion.None);

            controllersNameSpaces.ForEach(x => c.SwaggerEndpoint($"/swagger/{x}/swagger.json",
                x?[(x.LastIndexOf(".", StringComparison.Ordinal) + 1)..]));
        });

        app.UseAuthentication();
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthorization();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoint => endpoint.MapControllers());
    }
}