using ChelovchikBotApi.Authentication;
using ChelovchikBotApi.Models;
using ChelovchikBotApi.Repositories;
using ChelovchikBotApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Basic Authorization header using the Basic scheme. Example: \"Authorization: Basic {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Basic"
        }
    };

    c.AddSecurityDefinition("Basic", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Basic" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDBConnection"));
builder.Services.Configure<TwitchAPIConfig>(builder.Configuration.GetSection("TwitchAPI"));
builder.Services.AddSingleton<FeedDbContext>();
builder.Services.AddSingleton<IFeedRepository, FeedRepository>();
builder.Services.AddSingleton<IUserRepository, FeedRepository>();
builder.Services.AddSingleton<ICommandService, CommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();