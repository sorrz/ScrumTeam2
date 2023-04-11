using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopAdmin.Configuration;
using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Profiles;
using ShopGeneral.Services;
using ShopAdmin.Services;
using MailKit.Net.Smtp;

var builder = ConsoleApp.CreateBuilder(args);
builder.ConfigureServices((ctx, services) =>
{
    var connectionString = ctx.Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    services.AddDatabaseDeveloperPageExceptionFilter();


    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddTransient<IAgreementService, AgreementService>();
    services.AddTransient<IReportService, ReportService>();
    services.AddTransient<IPricingService, PricingService>();
    services.AddTransient<IProductService, ProductService>();
    services.AddTransient<ICategoryService, CategoryService>();
    services.AddTransient<IManufacturerService, ManufacturerService>();
    services.AddAutoMapper(typeof(Program));
    services.AddAutoMapper(typeof(ProductProfile));
    services.AddTransient<DataInitializer>();
    services.Configure<MailSettings>(ctx.Configuration.GetSection(nameof(MailSettings)));
    services.AddTransient<IMailService, MailService>();
    services.AddTransient<IFileOutputService, FileOutputService>();

    // Using Cysharp/ZLogger for logging to file
    //services.AddLogging(logging =>
    //{
    //    logging.AddZLoggerFile("log.txt");
    //});
});


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dataInitializer = scope.ServiceProvider.GetService<DataInitializer>();
    dataInitializer.SeedData();
}


app.AddAllCommandType();
app.Run();
//generate prices to PriceRunner (JSON file)
//verify all product images exists 
//report categories without products
//report  