using Microsoft.EntityFrameworkCore;
using ProcessControl.Server.Data;
using ProcessControl.Server.Models;
using ProcessControl.Server.Services;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//הוספת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>   /////("AllowAngularDevApp",
    {
        //  policy.WithOrigins("http://localhost:4200")  // כתובת הדומיין של אפליקציית Angular
        policy.AllowAnyOrigin()
               .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//builder.WebHost.UseUrls("https://localhost:5001");


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;

    });
});

// טעינת שרשור החיבור מ-appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        //לאפשר נסיונות חוזרים של חיבור לדטה בייס
        //,sqlOptions =>
        //{
        //    sqlOptions.EnableRetryOnFailure(
        //        maxRetryCount: 5,
        //        maxRetryDelay: TimeSpan.FromSeconds(10),
        //        errorNumbersToAdd: null);
        //}
        ));

builder.Services.AddHostedService<RamIvStatusBackgroundService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("https://localhost:5001");
}


//if (app.Environment.IsDevelopment())
//{
//    app.UseSpa(spa =>
//    {
//        spa.Options.SourcePath = "ClientApp"; // הנתיב שבו נמצא קוד Angular

//        // הפעלת שרת הפיתוח של Angular ב-Development
//        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
//    });
//}
//else
//{
//    app.UseSpaStaticFiles();
//}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

// הפעלת CORS
app.UseCors("AllowAll");

app.MapControllers();

//app.MapFallbackToFile("/index.html");

app.Run();
