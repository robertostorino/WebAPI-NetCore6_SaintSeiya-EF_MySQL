using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using WebApp_SaintSeiya.CasosDeUso;
using WebApp_SaintSeiya.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(routing => routing.LowercaseUrls = true); //Convierte la URL a lowercase. Es el standard en todos los demás lenguajes

// hace inyección de dependencia, para que la app tome este DbContext
builder.Services.AddDbContext<CaballerosDatabaseContext>(mysqlBuilder =>
{
    // toma la connection string desde appsettings.json
    mysqlBuilder.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection"));
});

// hace la inyección de depenencia de la interface de UpdateCaballeroUseCase
// identicion en <> primero la interface y luego la clase a la que hace refencia
builder.Services.AddScoped<IUpdateCaballeroUseCase, UpdateCaballeroUseCase>();


// Las dependencias pueden ser: Scoped, Transient ó Singleton.
// -> Singleton: Significa que se va a instanciar una única vez cuando se ejecuta la app
// -> Scoped: Significa que se va a instanciar cada vez que llega una request
// -> Transient: Implica que se va a instanciar cada vez que se inyecta en un servicio

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
