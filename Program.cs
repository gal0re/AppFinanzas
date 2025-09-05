using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON settings
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// DbContext
builder.Services.AddDbContext<FinanzasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default") ?? builder.Configuration["ConnectionStrings:Default"]));

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
});

// Swagger con XML comments
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(path)) c.IncludeXmlComments(path, includeControllerXmlComments: true);
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
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
});

var app = builder.Build();

// Migración automática
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FinanzasContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.RunAsync(db);
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
