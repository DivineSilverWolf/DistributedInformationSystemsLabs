using Lab_1;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using Md5_Selection.HashWorkers;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables()
    .Build();
builder.Services.AddSingleton<IHashWorker, Md5HashWorker>();
builder.Services.Configure<AlphabetSetting>(config.GetSection("AlphabetSetting"));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
