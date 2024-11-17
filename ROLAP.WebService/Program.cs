using ROLAP.Process;
using ROLAP.Process.Interfaces;
using ROLAP.QueryProcessor;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<QueryProcessor>(sp => new QueryProcessor(ROLAP.Configuration.ConfigurationExtensions.GetConfigurationStore()));
builder.Services.AddSingleton<IProcessor>(sp => new Processor(sp.GetService<QueryProcessor>()!));

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();