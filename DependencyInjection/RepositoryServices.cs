using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ps.EfCoreRepository.SqlServer.DependencyInjection
{
    public static class RepositoryServices
    {
        public static void AddEfCoreRepository<T>(this IServiceCollection services, IConfiguration config, string configKeyForConnStr = "AppDbConnection") where T : DbContext
        {
            services.AddScoped<DbContext, T>();
            services.AddScoped<IRepository, Repository>();
            services.AddSqlServerDatabase<T>(config, configKeyForConnStr);
        }
        private static IServiceCollection AddSqlServerDatabase<T>(this IServiceCollection services, IConfiguration config, string configKeyForConnStr = "AppDbConnection") where T : DbContext
        {
            string connStr = config.GetConnectionString(configKeyForConnStr);
            services.AddDbContextPool<T>(options => options.UseSqlServer(connStr));
            return services;
        }
    }
}
