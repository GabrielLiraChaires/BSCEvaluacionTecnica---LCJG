using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.Server.Custom;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

//PDF.
builder.Services.AddSingleton<IConverter>(sp => new SynchronizedConverter(new PdfTools()));

//Add services to the container..
builder.Services.AddControllers();

//Configuraci�n de Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BSC Evaluaci�n T�cnica API",
        Version = "v1",
        Description = "API para el sistema de evaluaci�n t�cnica."
    });

});

builder.Services.AddScoped<Utilidades>();

//Autenticaci�n de JWT.
builder.Services.AddAuthentication(configuracion =>
{
    configuracion.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configuracion.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configuracion =>
{
    configuracion.RequireHttpsMetadata = false;
    configuracion.SaveToken = true;
    configuracion.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false, //Validar verificaci�n para aplicaci�n externa
        ValidateAudience = false, //Servidor o Dominio que acceder� a API
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Clave"]!))
    };
});

//Cadena de conexi�n.
builder.Services.AddDbContext<BSCEvaluacionTecnicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Habilitar Cors.
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("corsNuevos", app =>
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

builder.Services.AddLogging(logging => {
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

//Middleware de Swagger (siempre disponible, incluso en producci�n).
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BSC API v1");
    c.RoutePrefix = "swagger"; 
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("corsNuevos");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();