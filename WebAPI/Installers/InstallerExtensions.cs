namespace WebAPI.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            // Pobierz wszystkie klasy implementujące IInstaller w bieżącym zestawie
            var installers = typeof(InstallerExtensions).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
            // Wywołaj metodę installServices dla każdej z nich
            installers.ForEach(installer => 
            {
                installer.InstallServices(services, configuration);
            });
        }
    }
}
