using BloodDonation;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.UseBloodDonation()
    .AddControllersWithViews();

// Configure pipeline
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();