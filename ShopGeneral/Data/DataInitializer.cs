using Bogus;
using Bogus.DataSets;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopGeneral.Data;

public class DataInitializer
{
    private static string fontawsomelistHtml;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly List<int> carImages = new();
    private readonly Random rand = new();

    public DataInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public void SeedData()
    {
        _context.Database.Migrate();
        SeedRoles();
        SeedUsers();
        SeedProducts();
        SeedAgreements();
        SeedUserAgreements();
    }

    private void SeedUserAgreements()
    {
        if (_context.UserAgreements.Any()) return;
        _context.UserAgreements.Add(new UserAgreements
        {
            Email = "stefan.holmberg@customer.systementor.se",
            Agreement = _context.Agreements.First(e => e.Name == "Nacka kommun")
        });
        _context.UserAgreements.Add(new UserAgreements
        {
            Email = "stefan.holmberg@vipcustomer.systementor.se",
            Agreement = _context.Agreements.First(e => e.Name == "Hederlige Harry Superdeal")
        });
        _context.SaveChanges();
    }

    private void SeedAgreements()
    {
        if (_context.Agreements.Any()) return;

        _context.Agreements.Add(new Agreement
        {
            CreatedDate = DateTime.Now,
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddYears(10),
            Name = "Nacka kommun",
            AgreementRows = new List<AgreementRow>
            {
                new()
                {
                    CategoryMatch = "van",
                    PercentageDiscount = 6
                },
                new()
                {
                    ProductMatch = "hybrid",
                    PercentageDiscount = 5
                }
            }
        });


        _context.Agreements.Add(new Agreement
        {
            CreatedDate = DateTime.Now,
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddYears(10),
            Name = "Hederlige Harry Superdeal",
            AgreementRows = new List<AgreementRow>
            {
                new()
                {
                    CategoryMatch = "volvo",
                    PercentageDiscount = 10
                },
                new()
                {
                    ProductMatch = "hybrid",
                    PercentageDiscount = 4
                }
            }
        });
        _context.SaveChanges();
    }


    private void SeedProducts()
    {
        if (_context.Products.Any()) return;


        for (var i = 0; i < 100; i++)
        {
            var vehicle = new Faker<Vehicle>().Generate();
            var product = new Product();

            product.Name = vehicle.Model() + " " + vehicle.Fuel();

            var categoryName = vehicle.Type();
            var category = _context.Categories.FirstOrDefault(e => e.Name == categoryName);
            if (category == null)
            {
                category = new Category
                {
                    Name = categoryName,
                    Icon = GetFaIcon()
                };
                _context.Categories.Add(category);
            }

            product.Category = category;

            var manufacturerName = vehicle.Manufacturer();
            var manufacturer = _context.Manufacturers.FirstOrDefault(e => e.Name == manufacturerName);
            if (manufacturer == null)
            {
                manufacturer = new Manufacturer
                {
                    Name = manufacturerName,
                    EmailReport = "info@" + manufacturerName.ToLower() + ".se",
                    Icon = GetFaIcon()
                };
                _context.Manufacturers.Add(manufacturer);
            }

            product.Manufacturer = manufacturer;

            product.BasePrice = rand.Next(10, 600) * 1000;

            product.AddedUtc = DateTime.Now.AddDays(-rand.Next(1, 100)).AddSeconds(-rand.Next(1, 100000));


            var url = GetCarImage();

            product.ImageUrl = url;
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }

    private string GetFaIcon()
    {
        if (string.IsNullOrEmpty(fontawsomelistHtml))
        {
            var httpClient = new HttpClient();
            fontawsomelistHtml = httpClient.GetStringAsync("https://fontawesome.bootstrapcheatsheets.com/").Result;
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(fontawsomelistHtml);
        var allIcons = doc.DocumentNode.SelectNodes("//span[@class='fa-class']");
        return allIcons[rand.Next(0, allIcons.Count)].InnerText.Substring(1);
    }


    public void Shuffle(List<int> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rand.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private string GetCarImage()
    {
        //Some trouble with Faker implementation made me write my own

        if (!carImages.Any())
        {
            for (var i = 1; i <= 209; i++) carImages.Add(i);
            Shuffle(carImages);
        }

        var num = carImages.First();
        carImages.RemoveAt(0);
        return $"https://objectstorage.eu-amsterdam-1.oraclecloud.com/n/axmjqhyyjpat/b/randomimages/o/cars%2F{num}.png";
    }

    private void SeedUsers()
    {
        AddUserIfNotExists(_userManager, "stefan.holmberg@systementor.se", "Hejsan123#", new[] { "Admin" });
        AddUserIfNotExists(_userManager, "stefan.holmberg@customer.systementor.se", "Hejsan123#", new[] { "Customer" });
        AddUserIfNotExists(_userManager, "stefan.holmberg@vipcustomer.systementor.se", "Hejsan123#",
            new[] { "Customer" });
    }


    private static void AddUserIfNotExists(UserManager<IdentityUser> userManager,
        string userName, string password, string[] roles)
    {
        if (userManager.FindByEmailAsync(userName).Result != null) return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };
        var result = userManager.CreateAsync(user, password).Result;
        var r = userManager.AddToRolesAsync(user, roles).Result;
    }


    private void SeedRoles()
    {
        var role = _context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (role == null) _context.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });
        role = _context.Roles.FirstOrDefault(r => r.Name == "Customer");
        if (role == null) _context.Roles.Add(new IdentityRole { Name = "Customer", NormalizedName = "Customer" });
        _context.SaveChanges();
    }

    private class JsonImg
    {
        public string File { get; set; }
    }
}