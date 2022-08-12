using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // register the controller created
builder.Services.AddMvc().AddXmlSerializerFormatters(); // To support XML format responses

//builder.Services.AddDbContext<EmployeeContext>(opt =>
//    opt.UseInMemoryDatabase("EmployeeList")); // To use in-memory databases, for quick testing

// Adding the Postgres database
builder.Services.AddDbContext<EmployeeContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage(); // To custom define error and response pages

    // Pre-formatted UI to issue requests and check responses
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

