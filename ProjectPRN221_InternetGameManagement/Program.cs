using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_InternetGameManagement.Hubs;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đăng ký DbContext với DI container
            builder.Services.AddDbContext<InternetGameManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR(); // Thêm SignalR service

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session là 30 phút
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); // Thêm session với cấu hình thời gian timeout

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSignalR(); // Thêm SignalR


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession(); // Kích hoạt session

            app.MapGet("/",  context =>
            {
                context.Response.Redirect("/login");
                return Task.CompletedTask;
            });
            app.MapHub<TimeHub>("/TimeHub"); // Map SignalR hub
            app.MapHub<OrderHub>("/orderHub");
            app.MapHub<SignalRServer>("/SignalRServer");
            app.MapRazorPages();

            app.Run();
        }
    }
}
