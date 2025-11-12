using MRO.SKM.Docu.Components;
using MRO.SKM.Docu.Infrastructure;
using MRO.SMK.Docu.ApplicationCore.Extensions;
using MudBlazor.Services;

namespace MRO.SKM.Docu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("debug.Db"));
            builder.Services.AddApplicationCore();

            builder.Services.AddMudServices();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
