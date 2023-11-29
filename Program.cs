using Microsoft.EntityFrameworkCore;
using Bloggie.Data;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Bloggie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add BloggieDbContext to the container
            builder.Services.AddDbContext<BloggieDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString"));
            });

            // Add AuthDbContext to the container
            builder.Services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString"));
            });

            // Add Identity to the container
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
            });


            // Add TagRepository to the container
            // e.g when i ask for ITagRepository, give me the implementation TagRepository
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
            builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();
            builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
