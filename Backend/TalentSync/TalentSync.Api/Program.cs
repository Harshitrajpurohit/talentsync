using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using TalentSync.Api.Helper;
using TalentSync.Api.Middleware;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Mappings.UserMappings;
using TalentSync.Application.Services;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Infrastructure.Persistence;
using TalentSync.Infrastructure.Persistence.Seeders;
using TalentSync.Infrastructure.Repositories;
using TalentSync.Infrastructure.Repositories.Employees;
using TalentSync.Infrastructure.Repositories.Recruitment;
using TalentSync.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);


// Add AppDbContext 
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TalentSync.Api")
    )
);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });


// register auto mapper
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<CookieHelper>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IScreeningService, ScreeningService>();
builder.Services.AddScoped<IScreeningRepository, ScreeningRepository>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<ISelectionService, SelectionService>();
builder.Services.AddScoped<ISelectionRepository, SelectionRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var jwtKey = builder.Configuration["JwtConfig:Key"] ?? throw new InvalidOperationException("JWT key is not configured in appsettings.json.");
var issuer = builder.Configuration["JwtConfig:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured in appsettings.json.");
var audience = builder.Configuration["JwtConfig:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured in appsettings.json.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


app.UseGlobalExceptionMiddleware();


// Use the global exception handling middleware



// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    await SeederRunner.SeedAsync(
        context,
        builder.Configuration);
}


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
