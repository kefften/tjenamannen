using tjenamannen.Data;
using Microsoft.EntityFrameworkCore;
using tjenamannen.Services.Rimmaskin;
using Microsoft.AspNetCore.Identity;
using tjenamannen.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
	{
		//options.Password.RequiredLength = 7;
		//options.Password.RequireDigit = false;
		//options.Password.RequireUppercase = false;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRimmaskinService, RimmaskinService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddAutoMapper(typeof(Program));





builder.Services.ConfigureApplicationCookie(options =>
{
	//options.Cookie.Expiration = TimeSpan.FromDays(150);
	options.Cookie.HttpOnly = true;
	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
	options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Use authentication
app.UseAuthorization();   // Use authorization

app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Rimmaskin}/{action=Index}/{id?}");

app.Run();