using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NordwindRestApi.Models;
using NordwindRestApi.Services.Interfaces;
using NordwindRestApi.Services;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("x",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Dependency Injektiolla v‰litetty tietokantatieto kontrollereille
builder.Services.AddDbContext<NorthwindOriginalContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("pilvi")
    //builder.Configuration.GetConnectionString("pilvi")
    ));

//Tuodaan appSettings.json tekem‰mme m‰‰ritys

var appSettingssection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingssection);

//JWT autentikaatio
var appSettings = appSettingssection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Key);

builder.Services.AddAuthentication(au =>
{
    au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddScoped<IAuthenticate,AuthenticateService>();

var app = builder.Build();

//T‰m‰ kohta lis‰tty
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("x");

app.MapControllers();

app.Run();


