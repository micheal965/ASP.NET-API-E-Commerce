using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using System.Text.Json;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Email;
using Talabat.Core.IServices;
using Talabat.Service;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Adding swagger services
builder.Services.AddSwaggerExtensions();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Identity
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(s =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});


//Add ApplicationServices
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAppServices();

//Add IdentityServices
builder.Services.AddIdentityServices(builder.Configuration);


#endregion

var app = builder.Build();

//Migrate and Seeding data for the first time
await Migrate_SeedingData.Migrate_Seed(app);

#region Configure Kestrel Middlewares
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TokenBlacklistMiddleware>();
if (app.Environment.IsDevelopment())
{
    //Using swagger services
    app.UseSwaggerExtensions();

}
app.UseStatusCodePagesWithReExecute("/Errors/{0}");
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion
app.Run();
