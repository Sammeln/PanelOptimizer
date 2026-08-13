using AGR_PanelOptimizer.Web.Components;
using AGR_PanelOptimizer.Core.Interfaces;
using AGR_PanelOptimizer.Core.Services;

namespace AGR_PanelOptimizer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<IPanelOptimizer, PanelOptimizer>();
            builder.Services.AddScoped<PanelCutter>();
            builder.Services.AddScoped<OrderMaterialPlanner>();
            builder.Services.AddScoped<ValvePlanner>();
            builder.Services.AddScoped<BlankPieceCutter>();
            builder.Services.AddScoped<ValveAssembler>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
