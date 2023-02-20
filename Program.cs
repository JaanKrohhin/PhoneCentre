using PhoneCentre.Data.Databases;
using PhoneCentre.Data.Interfaces;
using PhoneCentre.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
var serviceDbIsSql = builder.Configuration.GetValue<bool>("UseSqlOverMongo");

builder.Services.Configure<DbOptions>(
    builder.Configuration.GetSection(DbOptions.SectionName));


if (serviceDbIsSql)
{
    builder.Services.AddSingleton<IService, SqlService>();
}
else
{
    builder.Services.AddSingleton<IService, MongoService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
