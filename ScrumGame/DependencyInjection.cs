using ScrumGame.Services.Interfaces;
using ScrumGame.Services;
using ScrumGame.DataStore;

namespace ScrumGame
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddSingleton<IRoomInfoContext, RoomInfoContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IUserInfoContext, UserInfoContext>();

            return services;
        }
    }
}
