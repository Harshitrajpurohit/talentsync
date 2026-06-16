using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TalentSync.Api.Middleware;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Mappings.UserMappings;
using TalentSync.Application.Services;
using TalentSync.Infrastructure.Persistence;
using TalentSync.Infrastructure.Persistence.Seeders;
using TalentSync.Infrastructure.Repositories;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Use the global exception handling middleware
app.UseGlobalExceptionMiddleware();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
