using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.SDK.API;

namespace MindNote.Client.Host.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            services.AddScoped<INotesClient, NotesClient>();
            services.AddScoped<ICategoriesClient, CategoriesClient>();
            services.AddScoped<IUsersClient, UsersClient>();
            services.AddScoped<IRelationsClient, RelationsClient>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
