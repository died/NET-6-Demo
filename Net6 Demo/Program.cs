using Net6_Demo.Helpers;
using Net6_Demo.Extensions;
using Net6_Demo.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(logger =>
{
    logger.AddRabbitMQLogger();
});

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection(nameof(RabbitMQSetting)));

builder.Services.AddHostedService<BackgroundTask>();
builder.Services.AddSingleton<IWorker, ConsumeWorker>();

builder.Services.AddSingleton<RabbitMQHelper>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Net6_Demo", Version = "v1" });
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net6_Demo v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
