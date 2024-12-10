﻿using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session là 30 phút
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); // Thêm session với cấu hình thời gian timeout
            builder.Services.AddSignalR(); // Thêm SignalR

            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Đặt tên header cho CSRF token
            });
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "RequestVerificationToken";
            });

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

            app.MapGet("/", async context =>
            {
                context.Response.Redirect("/login");
            });
            app.MapHub<SignalRServer>("/SignalRServer"); // Map SignalR hub

            app.MapRazorPages();

            app.Run();
        }
    }
}
