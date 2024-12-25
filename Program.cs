using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using backendMuseum.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using backendMuseum.Models;
using backendMuseum.Helpers;
using backendMuseum.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Backend Museum API",
        Version = "v1",
        Description = "API cho hệ thống quản lý bảo tàng",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Backend Team",
            Email = "contact@backendmuseum.com"
        }
    });
});

// Configure DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configure Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Helper
var secretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Jwt:SecretKey is not configured in appsettings.json");
}

builder.Services.AddScoped<JwtHelper>(provider =>
    new JwtHelper(secretKey)
);

// Configure IEmailSender (Ensure EmailSender class is implemented correctly)
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Configure CORS (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Create a scope to check and create the "User" role if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    // Check and create the "User" role
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new Role { Name = "User" });
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Museum API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Enable CORS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
