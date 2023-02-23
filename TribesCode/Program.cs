using System.Runtime.InteropServices;
using dusicyon_midnight_tribes_backend.Contexts;
using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation;
using dusicyon_midnight_tribes_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using System.Text;
using dusicyon_midnight_tribes_backend.Services.Repositories;
using dusicyon_midnight_tribes_backend.Domain.GameConfig;
using dusicyon_midnight_tribes_backend.Domain.SeedData;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger authorization button setup will go here.
builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "jwt",
        Description = "Enter a valid JWT bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] {} }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

// Making the DB set-up compatible for both Windows and Mac.
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    var connectionStringDev = builder.Configuration.GetConnectionString("Development");
    var connectionStringTest = builder.Configuration.GetConnectionString("Test");
    var connectionStringProduction = builder.Configuration.GetConnectionString("Production");

    string envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    // Based on the active Environment Variable, we build IContext with only the appropriate implementation (either App or Test Db Context) - this saves us a bit of code in the actual Repositories (namely, we do not need to always inject both contexts and implement a conditional DI in their constructors).
    if (envVar == "Development")
    {
        builder.Services.AddDbContext<IContext, AppDbContext>(options => options.UseMySql(connectionStringDev, ServerVersion.AutoDetect(connectionStringDev)));
    }
    else if (envVar == "Test" || envVar == null)
    {
        builder.Services.AddDbContext<IContext, TestDbContext>(options => options.UseMySql(connectionStringTest, ServerVersion.AutoDetect(connectionStringTest)));

    }
}
// Here we just do the exact same thing, but for Windows and using MS SQL server.
else
{
    var connectionStringDev = builder.Configuration["ConnectionStrings:Development"];
    var connectionStringTest = builder.Configuration["ConnectionStrings:Test"];
    var connectionStirngProduction = builder.Configuration["ConnectionStrings:Production"];

    string envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    if (envVar == "Development")
    {
        builder.Services.AddDbContext<IContext, AppDbContext>(options =>
            options.UseSqlServer(connectionStringDev));
    }
    else if (envVar == "Test" || envVar == null)
    {
        builder.Services.AddDbContext<IContext, TestDbContext>(options =>
            options.UseSqlServer(connectionStringTest));
    }
    else if (envVar == "Production")
    {
        builder.Services.AddDbContext<IContext, AppDbContext>(options =>
            options.UseSqlServer(connectionStirngProduction));
    }
}

// Custom model validation set-up.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var result = new ValidationFailedResult(context.ModelState);
        result.ContentTypes.Add(MediaTypeNames.Application.Json);
        return result;
    };
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IGenericRepository, GenericRepository>();

builder.Services.AddScoped<IPlayerManagementService, PlayerManagementService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IEmailVerificationsRepository, EmailVerificationsRepository>();
builder.Services.AddScoped<IForgottenPasswordsRepository, ForgottenPasswordsRepository>();

builder.Services.AddScoped<IKingdomService, KingdomService>();
builder.Services.AddScoped<IKingdomRepository, KingdomRepository>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddScoped<IWorldService, WorldService>();
builder.Services.AddScoped<IWorldRepository, WorldRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();

builder.Services.AddScoped<IBuildingTypeRepository, BuildingTypeRepository>();

builder.Services.AddScoped<IProductionOptionRepository, ProductionOptionRepository>();
builder.Services.AddScoped<IProductionOptionService, ProductionOptionService>();
builder.Services.AddScoped<IProductionRepository, ProductionRepository>();
builder.Services.AddScoped<IProductionService, ProductionService>();

builder.Services.AddScoped<IGameConfig, GameConfig>();
builder.Services.AddScoped<ISeedData, SeedData>();




var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Test")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "tribesK3");
        c.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



// This is for the testing project to work (see Testing MR from Week 9 on GFA wiki)
public partial class Program { }