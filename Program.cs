using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSQLAPI.Controllers;
using NoSQLAPI.Services;

// Youtube video:
// https://www.youtube.com/watch?v=iWTdJ1IYGtg&t=264s

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Map the appsettings.json settings to the class DatabaseSettings
builder.Services.Configure<StudentStoreDatabaseSettings>(
                builder.Configuration.GetSection(nameof(StudentStoreDatabaseSettings)));

// Tying the DatabaseSettings class to the Interface so that when the interface is
// used, it just refers back to the class, which refers to the json file above
builder.Services.AddSingleton<IStudentStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<StudentStoreDatabaseSettings>>().Value);

// Giving this service the database name in the json
// To see where IMongoClient and IDatabaseSettings are used go to StudentService
builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("StudentStoreDatabaseSettings:ConnectionString")));

// Tying the Interface with the class
// https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
builder.Services.AddScoped<IStudentService, StudentService>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();