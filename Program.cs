using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using AppFinanzas.Security;
using AppFinanzas.Application.Interfaces;
using AppFinanzas.Application.Services;
using AppFinanzas.Application.Strategies;
using AppFinanzas.Infrastructure.Persistence;
using AppFinanzas.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// DbContext
builder.Services.AddDbContext<FinanzasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")
        ?? builder.Configuration["ConnectionStrings:Default"]));

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(path)) c.IncludeXmlComments(path, includeControllerXmlComments: true);

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT en header. Usá: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
                Reference = new Microsoft.OpenApi.Models.OpenApiReference{
                    Id = "Bearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

// ProblemDetails
builder.Services.AddProblemDetails();
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = ctx =>
        new ObjectResult(new ProblemDetails
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["errors"] = ctx.ModelState
                    .Where(kv => kv.Value?.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value!.Errors.Select(e => e.ErrorMessage))
            }
        })
        { StatusCode = StatusCodes.Status400BadRequest };
});

// Identity + Auth
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<FinanzasContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "FinanzasApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// DI
builder.Services.AddScoped<PricingStrategyFactory>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

var app = builder.Build();

if (app.Environment.IsEnvironment("Docker"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FinanzasContext>();

    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    await DbSeeder.RunAsync(db);

    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await IdentitySeeder.RunAsync(roleMgr, userMgr);
}
else if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FinanzasContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.RunAsync(db);

    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await IdentitySeeder.RunAsync(roleMgr, userMgr);
}

/* ---------- PIPELINE ---------- */
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

if (!app.Environment.IsEnvironment("Docker"))
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
