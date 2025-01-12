using SimpliSeoSearch.API.Handlers;
using SimpliSeoSearch.Core.Cache;
using SimpliSeoSearch.Core.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);
var app = builder.Build();
ConfigureMiddleware(app);
ConfigureEndpoints(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient();
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMemoryCache();

    builder.Services.AddCors(o => o.AddPolicy("MyCORS_Policy", option =>
    {
        option.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials();
    }));

    // Services
    builder.Services.AddScoped<ICacheService, MemoryCacheService>();
    builder.Services.AddScoped<ISearchService>(x => 
        new SearchService(x.GetRequiredService<ICacheService>(), [
            new GoogleSearchEngine(x.GetRequiredService<IHttpClientFactory>()),
            new BingSearchEngine(x.GetRequiredService<IHttpClientFactory>())
        ]));
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseCors("MyCORS_Policy");

    app.UseHttpsRedirection();

    app.UseAuthorization();
}

void ConfigureEndpoints(WebApplication app)
{
    app.MapControllers();
}
