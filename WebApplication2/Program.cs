//using Microsoft.EntityFrameworkCore;
//using WebApplication2.Data;
//using WebApplication2.Service;

//var builder = WebApplication.CreateBuilder(args);

//// Kết nối tới database
//var connectionString = builder.Configuration.GetConnectionString("WebApplication2Context");
//builder.Services.AddDbContext<WebApplication2Context>(options =>
//    options.UseLazyLoadingProxies().UseSqlServer(connectionString));

//// Đăng ký service
//builder.Services.AddScoped<UserService, UserServiceImpl>();

//// Thêm MVC và session
//builder.Services.AddControllersWithViews();
//builder.Services.AddSession();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Cấu hình middleware
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseSession();
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Service;

var builder = WebApplication.CreateBuilder(args);

// Kết nối tới database
var connectionString = builder.Configuration.GetConnectionString("WebApplication2Context");
builder.Services.AddDbContext<WebApplication2Context>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));

// Đăng ký service
builder.Services.AddScoped<UserService, UserServiceImpl>();

// Thêm MVC, Session và Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRazorPages();

// ✅ Thêm cấu hình Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Cấu hình các đường dẫn liên quan đến xác thực
        options.LoginPath = "/User/Login";           // Nếu chưa login → chuyển đến đây
        options.LogoutPath = "/User/Logout";         // Trang đăng xuất
        options.AccessDeniedPath = "/User/Denied";   // Khi bị chặn truy cập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie hết hạn sau 30 phút
    });

var app = builder.Build();

// Cấu hình middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Bắt buộc: thêm 2 dòng này để bật xác thực & phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
