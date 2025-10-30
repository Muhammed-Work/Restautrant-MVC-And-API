using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantGorRahsa.Models;
using RestaurantGorRahsa.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = TokenService.GetTokenValidationParameter(builder.Configuration);
});

builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();


// Add services to the container.

builder.Services.AddControllers();  // for api
builder.Services.AddControllersWithViews(); // for mvc
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RMScontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services for MVC authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Adjust based on your needs
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RMScontext>()
.AddDefaultTokenProviders();



builder.Services.AddTransient<TokenService>();

builder.Services.AddScoped<ITypeServices,TypeServices>();
builder.Services.AddScoped<IMealServices,MealServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();  // Ensure static files are enabled

app.UseHttpsRedirection();


app.MapControllers();

// Map default route for MVC controllers with views
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AccountMvc}/{action=Register}");


app.Run();
