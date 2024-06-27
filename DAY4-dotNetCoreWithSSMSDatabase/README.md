# Day 4 Lesson 
## .NET Core with SQL

 In this guide, we are going to create a product list screen for you to learn the basics on how to implement entity framework on .NET Core app.

 1. Before proceeding, change directory to the `ProductsSolution` first and try running it using `dotnet run`. Proceed to step 2 if successful.

    This solutions should be running smoothly before proceeding to the next step.

 2. Install entity framework core

    ```bash
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    ```

 3. Create `Models` folder and create `Products.cs` inside.

    ```cs
    // Models/Product.cs
    namespace ProductsSolution.Models
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
    ```
 4. Create database context
    
    The DbContext class manages the entity objects during runtime, which includes populating objects with data from a database, change tracking, and persisting data to the database. Create a `Data` folder and add an `AppDbContext.cs` class:

    ```cs
    // Data/AppDbContext.cs
    using Microsoft.EntityFrameworkCore;
    using ProductsSolution.Models;

    namespace ProductsSolution.Data
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Product> Products { get; set; }
        }
    }
    ```

 5. Create Repository Pattern

    Create an `Interfaces` folder and add `IProductRepository.cs` file:

    ```cs
    // Interfaces/IProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProductsSolution.Models;

    namespace ProductsSolution.Interfaces
    {
        public interface IProductRepository
        {
            // this is an interface to get all products
            Task<IEnumerable<Product>> GetAllProducts();
        }
    }
    ```

    Implement the interface in the Repository. Create a `Repositories` folder and add `ProductRepository.cs`:

    ```cs
    // Repositories/ProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProductsSolution.Data;
    using ProductsSolution.Interfaces;
    using ProductsSolution.Models;

    namespace ProductsSolution.Repositories
    {
        public class ProductRepository : IProductRepository
        {
            private readonly AppDbContext _context;

            public ProductRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllProducts()
            {
                return await _context.Products.ToListAsync();
            }
        }
    }

    ```

 6. Configure the Database Context

    In your `Program.cs` file, configure the database context in the `ConfigureServices` method:

    ```cs
    // Include the necessary package and code
    using Microsoft.EntityFrameworkCore;
    using ProductsSolution.Data;
    using ProductsSolution.Interfaces;
    using ProductsSolution.Repositories;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // Add DbContext with SQL Server provider
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add repository dependency
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
    ```

    Also, add your connection string in `appsettings.json`:

    NOTE: Change the `server_name` with your server name and the `database_name` with the database that you created

    ```json
    {
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
            "DefaultConnection": "Server=server_name;Database=database_name;Trusted_Connection=True;TrustServerCertificate=True"
        }
    }
    ```
 7. Create a list screen

    Step 1: Create `AppModel` folder and add ``App.cs`:

    ```cs
    // AppModel/App.cs
    using ProductsSolution.Models;

    namespace ProductsSolution.AppModels
    {
        public class App
        {
            public IEnumerable<Product> Products { get; set; }
        }
    }
    ```

    Step 2: Update your `HomeController.cs`

    ```cs
    using Microsoft.AspNetCore.Mvc;
    using ProductsSolution.Interfaces;
    using ProductsSolution.AppModels;

    namespace ProductsSolution.Controllers;

    public class HomeController : Controller
    {
        // Declare IProductrepository
        private readonly IProductRepository productRepository;

        // Injection
        public HomeController(IProductRepository productRepository){
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            App app = new App();
            app.Products = await this.productRepository.GetAllProducts();
            return View(app);
        }
    }
    ```

    Step3: Update Index.cshtml

    ```cshtml
    @model ProductsSolution.AppModels.App;
    @{
        ViewData["Title"] = "Home Page";
    }
    <table class="table table-hover">
        <thead>
            <tr>
                <th>Product Name</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            @foreach(ProductsSolution.Models.Product product in Model.Products){
                <tr>
                    <td>@product.Name</td>
                    <td>@product.Price</td>
                </tr>
            }
        </tbody>
    </table>
    ```

 8. Apply Migration and Update Database

    install EF Core tools

    ```bash
    dotnet tool install --global dotnet-ef
    ```

    Run the following commands to create and apply the initial migration, and then update the database:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

## Create Edit Screen

 - *Step 1 :* Add button to list screen

    ```cshtml
    @model ProductsSolution.AppModels.App;
    @{
        ViewData["Title"] = "Home Page";
    }

    <a class="btn btn-primary" asp-area="" asp-controller="Home" asp-action="EditScreen">Add Product</a>

    <table class="table table-hover">
        <thead>
            <tr>
                <th>Product Name</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            @foreach(ProductsSolution.Models.Product product in Model.Products){
                <tr>
                    <td>@product.Name</td>
                    <td>@product.Price</td>
                </tr>
            }
        </tbody>
    </table>
    ```

 - *Step 2 :* Create a new View

    Update `App.cs`

    ```cs
    using ProductsSolution.Models;

    namespace ProductsSolution.AppModels
    {
        public class App
        {
            public IEnumerable<Product> Products { get; set; }
            public Product NewProduct { get; set; }
        }
    }
    ```

    Update `HomeController.cs`

    ```cs
    using Microsoft.AspNetCore.Mvc;
    using ProductsSolution.Interfaces;
    using ProductsSolution.AppModels;

    namespace ProductsSolution.Controllers;

    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository){
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            App app = new App();
            app.Products = await this.productRepository.GetAllProducts();
            return View(app);
        }

        public IActionResult EditScreen()
        {
            App app = new App();
            return View(app);
        }
    }
    ```

    Create a new screen in `Views/Home`:

    ```cshtml
    // EditScreen.cshtml
    @model ProductsSolution.AppModels.App;
    @{
        ViewData["Title"] = "Edit Product";
    }

    <div class="card border-primary mb-3" style="width: 100%">
    <div class="card-header">Add Product</div>
    <div class="card-body">
        @using (Html.BeginForm("Save", "Home", FormMethod.Post, new { @name = "testForm" })){
            
            <fieldset>
                <div class="row">
                    <label for="Name" class="col-sm-2 col-form-label">Name</label>
                    <div class="col-sm-10">
                        @Html.TextBoxFor(model => model.NewProduct.Name, new { @class = "form-control", @id = "Name" })
                    </div>
                </div>

                <div class="row mt-2">
                    <label for="Price" class="col-sm-2 col-form-label">Price</label>
                    <div class="col-sm-10">
                        @Html.TextBoxFor(model => model.NewProduct.Price, new { @class = "form-control", @id = "Price", @type = "number" })
                    </div>
                </div>

                <button type="submit" class="btn btn-success" href=""><i class="icon-save"></i>  Save</button>

            </fieldset>
        }
    </div>
    </div>
    ```

 - *Step 3 :* Update `IProductRepository.cs`

    ```cs
    // Interfaces/IProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProductsSolution.Models;

    namespace ProductsSolution.Interfaces
    {
        public interface IProductRepository
        {
            // this is an interface to get all products
            Task<IEnumerable<Product>> GetAllProducts();

            // Add Interface to Add new product
            Task AddProduct(Product product);
        }
    }
    ```

 - *Step 4 :* Update `ProductRepository.cs`

    ```cs
    // Repositories/ProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProductsSolution.Data;
    using ProductsSolution.Interfaces;
    using ProductsSolution.Models;

    namespace ProductsSolution.Repositories
    {
        public class ProductRepository : IProductRepository
        {
            private readonly AppDbContext _context;

            public ProductRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllProducts()
            {
                return await _context.Products.ToListAsync();
            }

            public async Task AddProduct(Product product)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
        }
    }
    ```

 - *Step 5 :* Add `Save` endpoint in `HomeController.cs`

    ```cs
    using Microsoft.AspNetCore.Mvc;
    using ProductsSolution.Interfaces;
    using ProductsSolution.AppModels;
    using ProductsSolution.Models;

    namespace ProductsSolution.Controllers;

    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository){
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            App app = new App();
            app.Products = await this.productRepository.GetAllProducts();
            return View(app);
        }

        public IActionResult EditScreen()
        {
            App app = new App();
            return View(app);
        }

        [HttpPost, ActionName("Save")]
        public async Task<ActionResult> Save(Product NewProduct)
        {
            await this.productRepository.AddProduct(NewProduct);

            return RedirectToAction("Index");
        }
    }
    ```

## Adding delete functionality

 - *Step 1 :* Update `index.cshtml`

    ```cshtml
    @model ProductsSolution.AppModels.App;
    @{
        ViewData["Title"] = "Home Page";
    }

    <a class="btn btn-primary" asp-area="" asp-controller="Home" asp-action="EditScreen">Add Product</a>

    <table class="table table-hover">
        <thead>
            <tr>
                <th></th>
                <th>Product Name</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            @foreach(ProductsSolution.Models.Product product in Model.Products){
                <tr>
                    <td>
                        <form asp-action="Delete" method="post" style="display:inline;">
                            <input type="hidden" name="id" value="@product.Id" />
                            <button type="submit" class="btn btn-danger">Delete</button>
                        </form>
                    </td>
                    <td>@product.Name</td>
                    <td>@product.Price</td>
                </tr>
            }
        </tbody>
    </table>
    ```
 - *Step 2 :* Update `IProductRepository.cs`

    ```cs
    // Interfaces/IProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProductsSolution.Models;

    namespace ProductsSolution.Interfaces
    {
        public interface IProductRepository
        {
            // this is an interface to get all products
            Task<IEnumerable<Product>> GetAllProducts();

            // Add Interface to Add new product
            Task AddProduct(Product product);
            
            // Add Interface to delete product
            Task DeleteProduct(int id);
        }
    }
    ```

 - *Step 3 :* Update `ProductRepository.cs`

    ```cs
    // Repositories/ProductRepository.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProductsSolution.Data;
    using ProductsSolution.Interfaces;
    using ProductsSolution.Models;

    namespace ProductsSolution.Repositories
    {
        public class ProductRepository : IProductRepository
        {
            private readonly AppDbContext _context;

            public ProductRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllProducts()
            {
                return await _context.Products.ToListAsync();
            }

            public async Task AddProduct(Product product)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            
            public async Task DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
    ```

 - *Step 4 :* Update `HomeController.cs`

    ```cs
    using Microsoft.AspNetCore.Mvc;
    using ProductsSolution.Interfaces;
    using ProductsSolution.AppModels;
    using ProductsSolution.Models;

    namespace ProductsSolution.Controllers;

    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository){
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            App app = new App();
            app.Products = await this.productRepository.GetAllProducts();
            return View(app);
        }

        public IActionResult EditScreen()
        {
            App app = new App();
            return View(app);
        }

        [HttpPost, ActionName("Save")]
        public async Task<ActionResult> Save(Product NewProduct)
        {
            await this.productRepository.AddProduct(NewProduct);

            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            await this.productRepository.DeleteProduct(id);

            return RedirectToAction("Index");
        }
    }
    ```

## Publish the site

    ```bash
    dotnet publish -c Release -o ./publish
    ```