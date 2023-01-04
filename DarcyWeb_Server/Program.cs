using Darcy_DataAccess.Data;
using DarcyWeb_Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DarcyWeb_Server.Service.IService;
using DarcyWeb_Server.Service;
using Darcy_Business.Repository.IRepository;
using Darcy_Business.Repository;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.ResponseCompression;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("ODQ0Nzc5QDMyMzAyZTM0MmUzMFlzSVFsaFdOUFE3TXIrQjIxcEJBdnp5VGlCNDZvYmpBNjU2U3dRZC9rQ009");

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:8092");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddResponseCompression(opts =>
{
	opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		new[] { "application/octet-stream" });
});

//builder.Services.AddDbContext<DarcyAppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DarcyAppDbContext>(options =>
		options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<DarcyAppDbContext>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<ICameraRepository, CameraRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IMqttHandler, MqttHandler>();

builder.Services.AddHostedService<HostedMqttClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

SeedDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


async void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		await dbInitializer.Initialize();
	}
}