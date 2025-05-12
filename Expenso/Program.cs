using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MonthlyExpenseTracker.Constants;
using MonthlyExpenseTracker.Data;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.EntityModels;
using MonthlyExpenseTracker.Helper.Jwt;
using MonthlyExpenseTracker.Helper.Middleware;
using MonthlyExpenseTracker.Helper.Validator;
using MonthlyExpenseTracker.Services.Implementation;
using MonthlyExpenseTracker.Services.Interface;

var builder = WebApplication.CreateBuilder(args);


//Add Db Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<tblUsers>, PasswordHasher<tblUsers>>();


// ✅ Register Validators from Assembly (New Way)
builder.Services.AddValidatorsFromAssemblyContaining<ExpenseValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

#region Jwt Configuration
// Bind JwtSettings once using Options pattern
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<JwtTokenHelper>();

// For JwtBearer setup, manually extract config just for use here
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

#endregion


#region Swagger Registration with Jwt Login
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Xpenso API", Version = "v1" });

    // Adding security definition for JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT Bearer token"
    });

    // Adding security requirement for Swagger to include JWT token
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

#endregion

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Enable CORS for mobile client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI only in development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Xpenso API V1");
        options.RoutePrefix = "swagger"; // Optional: to serve Swagger UI at the root
    });
}
app.UseCors("AllowAll");
// Add custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var config = services.GetRequiredService<IConfiguration>();
    var hasher = services.GetRequiredService<IPasswordHasher<tblUsers>>();

    var adminSettings = config.GetSection("AdminUser").Get<AdminUserSettings>();

    // 🔹 Check if role exists
    var role = await context.Role.FirstOrDefaultAsync(r => r.Name == adminSettings.Role);
    if (role == null)
    {
        role = new tblRole { Name = adminSettings.Role };
        context.Role.Add(role);
        await context.SaveChangesAsync(); // Save to get generated RoleId
    }

    // 🔹 Check if admin user already exists
    var existingAdmin = await context.Users
        .FirstOrDefaultAsync(u => u.Email == adminSettings.Email);

    if (existingAdmin == null)
    {
        var adminUser = new tblUsers
        {
            Name = adminSettings.FullName,
            Email = adminSettings.Email,
            RoleId = role.Id,
            City = adminSettings.City,
            Country = adminSettings.Country,
            Gender = adminSettings.Gender,
            CreatedAt = DateTime.UtcNow,
            DateOfBirth = adminSettings.DateOfBirth,
            Provider = GlobalConstants.Application
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, adminSettings.Password);

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
    }
}



app.Run();
