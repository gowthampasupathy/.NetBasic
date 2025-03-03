using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));


builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>
{
    options.RequireHttpsMetadata=false;
    options.SaveToken=true;
    options.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateIssuer=false,
        ValidateAudience=false,
        ValidateLifetime=true,
        ValidIssuer="",
        ValidAudience="",
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ihgigiugughugujhuigkujgbkugiugiujgbiugiugbiugiug"))

    }; 

});


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.MapGet("/check-db", async ([FromServices] AppDbContext dbContext) =>
{
    try
    {
        await dbContext.Database.CanConnectAsync();
        return Results.Ok("Database connection is successful! ");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}");
    }
});


app.Run();
