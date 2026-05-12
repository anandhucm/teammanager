using MYTEAMMANAGER.Data;
using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Services;
using MYTEAMMANAGER.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MYTEAMMANAGER.MiddleWare;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection.Metadata;
using MYTEAMMANAGER.OptionPatterns;
using MyGrpcService;

// This switch allows gRPC to work over HTTP (without SSL/TLS)
// AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddHttpClient();


//injecting the ApplicationDbContext inside the Program.cs file.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//registering the configuration options - this registration works for all - IOptions, IOptionsSnapshot, IOptionsMonitor
//IOptions          →  reads config ONCE at startup,  never changes
//IOptionsSnapshot  →  reads config ONCE per request, changes between requests
//IOptionsMonitor   →  reads config LIVE,             reacts instantly to changes
builder.Services.Configure<AzureBlobSettings>(
    builder.Configuration.GetSection("AzureBlobStorage")
);
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<AzureFunctionsService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenKey = builder.Configuration["tokenKey"] ?? throw new Exception("Token key not found - program.cs");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LastNamePolicy", policy =>
    {
        policy.RequireClaim("LastName", "Menon");
        
    });
});


builder.Services.AddControllers();

builder.Services.AddGrpcClient<Greeter.GreeterClient>(o =>
{
    o.Address = new Uri(
        builder.Configuration["GrpcSettings:Url"]!
    );
});

var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(allowedOrigins!);
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleWare>();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseCors("CorsPolicy");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error occured during migration");

}
app.Run();

