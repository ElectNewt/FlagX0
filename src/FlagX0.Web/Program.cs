using FlagX0.Web.Business.UseCases;
using FlagX0.Web.Business.UseCases.Flags;
using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie("Identity.Bearer");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseMySQL(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<FlagsUseCases>();
builder.Services.AddScoped<AddFlagUseCase>();
builder.Services.AddScoped<GetPaginatedFlagsUseCase>();
builder.Services.AddScoped<GetSingleFlagUseCase>();
builder.Services.AddScoped<UpdateFlagUseCase>();
builder.Services.AddScoped<DeleteFlagUseCase>();

builder.Services.AddScoped<IFlagUserDetails, FlagUserDetails>();


builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("token", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.Http,
		In = ParameterLocation.Header,
		Name = HeaderNames.Authorization,
		Scheme = "Bearer"
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();

	app.UseSwagger();
	app.UseSwaggerUI();

}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{
	ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.Run();
