using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NextVer.Infrastructure.Persistance;
using NextVer.Infrastructure.Persistence.DbSeed;
using NextVerBackend.Configuration;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));

builder.Services.AddDbContext<NextVerDbContext>(options => options
    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors();
builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureHangfire(configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<NextVerDbContext>();
    var databaseSeed = new Seed(context);
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    databaseSeed.SeedData();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextVer");
    c.RoutePrefix = "swagger";
});

app.UseRouting();

app.UseCors(x => x
    .WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHangfireDashboard();

app.Run();
