using System.Globalization;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Splat;
using Splat.Autofac;
using WeatherTeller.Persistence;
using WeatherTeller.Persistence.EntityFramework;

namespace WeatherTeller.Infrastructure;

public class AppHost
{
    public static string AppName = "WeatherTeller";
    public static string AvaloniaResourceUriBase = $"avares://{AppName}/";
    public static string AvaloniaResourceUriAssetsBase = $"avares://{AppName}/Assets/";
    public static Uri GetResourceUri(string path) => new Uri($"{AvaloniaResourceUriBase}{path}");
    public static Uri GetAssetUri(string path) => new Uri($"{AvaloniaResourceUriAssetsBase}{path}");
    private static List<Module> _modules = new();
    public static void AddModule(Module module) => _modules.Add(module);
    public static void AddModule<TModule>() where TModule : Module, new() => _modules.Add(new TModule());
    private readonly Lazy<IHostBuilder> _builder;
    private readonly Lazy<IHost> _host;

    private readonly Lazy<string> _path = new(() =>
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appDirectory = System.IO.Path.Combine(appData, "WeatherTeller");
        if (!Directory.Exists(appDirectory)) Directory.CreateDirectory(appDirectory);

        Directory.SetCurrentDirectory(appDirectory);
        return appDirectory;
    });

    private readonly ISubject<Exception> _unhandledExceptions = new Subject<Exception>();

    public AppHost()
    {
        // set culture to invariant
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        
        
        _host = new Lazy<IHost>(() => Builder.Build(), LazyThreadSafetyMode.ExecutionAndPublication);
        _builder = new Lazy<IHostBuilder>(CreateHostBuilder, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private string Path => _path.Value;

    private IHost Host => _host.Value;
    private IHostBuilder Builder => _builder.Value;
    public IServiceProvider Services { get; private set; } = null!;
    public IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();

    private IHostBuilder CreateHostBuilder()
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
        builder.UseContentRoot(Path);
        builder.ConfigureHostConfiguration(ConfigureHostConfiguration);
        builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.ConfigureContainer<ContainerBuilder>(ConfigureContainer);
        builder.ConfigureLogging(ConfigureLogging);
        builder.ConfigureServices((Action<HostBuilderContext, IServiceCollection>)ConfigureServices);

        return builder;
    }

    protected virtual void ConfigureHostConfiguration(IConfigurationBuilder builder)
    {
        if (!Design.IsDesignMode)
        {
            // add appsettings.Development.json if running in development mode
            // get environment variable
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            // or
            env ??= Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env)) env = "";

            var appSettingWritePath = System.IO.Path.Combine(Path, "appsettings.json");
            var appSettingsDevWritePath = System.IO.Path.Combine(Path, "appsettings.Development.json");
            var isDevelopment = env.Equals("Development", StringComparison.OrdinalIgnoreCase);
            if (isDevelopment)
            {
                var appsetingsDevUri = GetAssetUri("appsettings.Development.json");
                // write default appsettings.json
                var exists = File.Exists(appSettingsDevWritePath);
                var resourceExists = AssetLoader.Exists(appsetingsDevUri);
                if (!exists && resourceExists)
                {
                    using var stream = AssetLoader.Open(appsetingsDevUri);
                    using var fileStream = File.Create(appSettingsDevWritePath);
                    stream.CopyTo(fileStream);
                }
            }
            else
            {
                var appsetingsUri = GetAssetUri("appsettings.json");
                // write default appsettings.json
                var exists = File.Exists(appSettingWritePath);
                var resourceExists = AssetLoader.Exists(appsetingsUri);
                if (!exists && resourceExists)
                {
                    using var stream = AssetLoader.Open(appsetingsUri);
                    using var fileStream = File.Create(appSettingWritePath);
                    stream.CopyTo(fileStream);
                }
            }

            builder.AddJsonFile(isDevelopment ?
                    appSettingsDevWritePath : 
                    appSettingWritePath, 
                optional: true, reloadOnChange: true);
        }
        else
        {
            builder.AddInMemoryCollection([
                new KeyValuePair<string, string>("Logging:LogLevel:Default", "Debug")!,
                new KeyValuePair<string, string>("Logging:LogLevel:System", "Information")!,
                new KeyValuePair<string, string>("Logging:LogLevel:Microsoft", "Information")!,
                new KeyValuePair<string, string>("App:IsOffline", "true")!
            ]);
        }
    }

    protected virtual void ConfigureLogging(HostBuilderContext ctx, ILoggingBuilder builder)
    {
        var seqSection = ctx.Configuration.GetSection("Seq");
        var maybeSeqUrl = seqSection?.GetValue<string>("ServerUrl");
        builder.ClearProviders();
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", AppName)
            .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .WriteTo.Console(LogEventLevel.Debug,
                "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new RenderedCompactJsonFormatter(), System.IO.Path.Combine(Path, "logs", "log-.log"), rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Verbose);
        
        if (maybeSeqUrl != null)
        {
            loggerConfiguration
                .MinimumLevel.Verbose()
                .WriteTo.Seq(maybeSeqUrl, LogEventLevel.Verbose);
        }
        
        builder.AddSerilog(loggerConfiguration
            .CreateLogger());
    }

    protected virtual void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
    {
        services.AddSingleton(ctx.HostingEnvironment.ContentRootFileProvider);
        services.AddSingleton(MessageBus.Current);
        services.AddPersistence();
        var isDesign = Design.IsDesignMode;
        if (isDesign)
        {
            services.AddWeatherTellerInMem();
        }
        else
        {
            services.AddWeatherTellerSqlite(Design.IsDesignMode
                ? System.IO.Path.Combine(Path, "teller-design.sqlite")
                : System.IO.Path.Combine(Path, "teller.sqlite"));
        }
    }

    private void ConfigureContainer(ContainerBuilder builder)
    {
        builder.UseAutofacDependencyResolver();
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

        var appViewLocator = new AppViewLocator();
        Locator.CurrentMutable.RegisterConstant<IViewLocator>(appViewLocator);
        
        // register all modules
        foreach (var module in _modules)
        {
            builder.RegisterModule(module);
        }
    }

    public void Build()
    {
        _ = _builder.Value;
        var container = Host.Services.GetAutofacRoot();
        var serviceProvider = container.Resolve<IServiceProvider>();
        Services = serviceProvider;
    }

    public void Start()
    {
        var logger = Services.GetRequiredService<ILogger<AppHost>>();
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            logger.LogError((Exception)args.ExceptionObject, "Unhandled exception");
        };

        RxApp.DefaultExceptionHandler = _unhandledExceptions;

        _unhandledExceptions.Select(ex =>
        {
            logger.LogError(ex, "Unhandled exception");
            var message = ex.Message;
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error", message);

            return messageBox.ShowAsync();
        }).Concat().Subscribe();


        ServiceLocator.Instance = Services;

        Host.Start();
    }

    public void Stop()
    {
        Task.Run(async () => await StopAsync());
    }

    public async Task StopAsync()
    {
        await Host.StopAsync(TimeSpan.FromSeconds(2));
        await Host.WaitForShutdownAsync();
        Host.Dispose();
    }
}