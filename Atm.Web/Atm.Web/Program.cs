var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAtmDbContext(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddClientCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseAtmSwaggerEndpoint();

app.UseHttpsRedirection();

app.UseCientCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Apply Seed Data

await app.Services.SeedDataAsync();

// Run the app

app.Run();
