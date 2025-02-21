using System.Configuration;
using System.Data;
using System.Windows;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    
        public partial class App : Application
        {
            public static IServiceProvider ServiceProvider { get; private set; }

            protected override void OnStartup(StartupEventArgs e)
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                base.OnStartup(e);
            }

            private void ConfigureServices(ServiceCollection services)
            {
                services.AddDbContextFactory<DataContext>(options =>
                    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Project\\DataStorage_Assignment\\Data\\Database\\local_database.mdf;Integrated Security=True;Connect Timeout=30"),ServiceLifetime.Scoped);

                services.AddScoped<CustomerRepository>();
                services.AddScoped<ProductRepository>();
                services.AddScoped<UserRepository>();
                services.AddScoped<StatusTypeRepository>();
            }
        }
    

}
