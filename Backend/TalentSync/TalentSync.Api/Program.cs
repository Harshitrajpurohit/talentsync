using Microsoft.EntityFrameworkCore;
using TalentSync.Api.Middleware;
using TalentSync.Infrastructure.Persistence;
using TalentSync.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);


// Add AppDbContext 
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TalentSync.Api")
    )
);

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
