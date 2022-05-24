using Microsoft.OpenApi.Models;
using SwaggerVervions;
using Swashbuckle.AspNetCore.SwaggerUI;

CreateHostBuilder(args).Build().Run();


 IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });