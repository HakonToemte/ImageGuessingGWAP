using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImageGuessingGame.GameContext.GameHandling;
using ImageGuessingGame.Hubs;
using ImageGuessingGame.GameContext.SuggestionHandling;
namespace ImageGuessingGame
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
            });
            services.AddDbContext<GuessingGameContext>(
                options => options.UseSqlite("Data Source=sqlitedb1;Cache=Shared"));
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<GuessingGameContext>();  
            services.AddScoped<ILoginUserValidator, LoginUserValidator>();
            services.AddScoped<ILoginUserProvider, LoginUserProvider>();
            services.AddScoped<IGameProvider, GameProvider>();
            services.AddScoped<IGameValidator, GameValidator>();
            services.AddScoped<IGuessValidator, GuessValidator>();
            services.AddScoped<ISuggestionValidator, SuggestionValidator>();
            services.AddScoped<ISuggestionProvider, SuggestionProvider>();
            services.AddSignalR();
            services.AddRazorPages();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<StartGameHub>("/startGameHub");
                endpoints.MapHub<MultiplayerHub>("/multiplayerHub");
            });
        }
    }
}
