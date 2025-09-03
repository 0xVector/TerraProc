using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using TerraProc.Core.Provider;
using TerraProc.Server;
using TerraProc.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcReflection();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // For OpenAPI
builder.Services.AddSwaggerGen();

// Load config
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection("Server"));

// Register chunk provider
builder.Services.AddSingleton<IChunkProvider>(sp =>
{
    var o = sp.GetRequiredService<IOptions<ServerOptions>>().Value;
    return ProviderFactory.CreateProvider(new ProviderConfig
    {
        Seed = o.Seed,
        MaxThreads = o.Threads,
        UseCoalescing = true
    });
});

// Configure Swagger to use XML comments
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configure Kestrel
builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
    o.ListenAnyIP(builder.Configuration.GetValue("Server:Port", 5000),
        lo =>
        {
            lo.UseHttps();
            lo.Protocols = HttpProtocols.Http1AndHttp2;
        });
});

var app = builder.Build();

// Middleware
// app.UseHttpsRedirection();
app.UseDefaultFiles(); // Serve wwwroot/index.html by default
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseSwagger();
app.UseSwaggerUI();

// Configure gRPC
app.MapGrpcService<ChunksService>();
if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

// Configure Rest API
app.MapControllers();
app.MapGet("/status", () => Results.Ok(new { status = "Server is running" }));

app.Run();