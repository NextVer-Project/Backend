using NextVerBackend.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NextVer.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var app = builder.Build();

builder.Services.AddDbContext<NextVerDbContext>(options => options
    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.ConfigureDependencyInjection();

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


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextVer");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
