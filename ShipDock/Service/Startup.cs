using ShipDock.Service;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ITracktorRepository, TracktorRepository>();
        services.AddScoped<ITracktorService, TracktorService>();
        services.AddScoped<ICargoRepository, CargoRepository>();
        services.AddScoped<ICargoService, CargoService>();
        
        services.AddControllers();
        services.AddSingleton<LotJobService>();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}