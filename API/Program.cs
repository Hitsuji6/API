using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapGet("/", () => "Hello World!");

app.MapPost("/user", () => new{Name = "Eduardo Veiga", Age = 21});

app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Eduardo", "Veiga");
    return new {Name = "Eduardo Veiga", Age = 21};
});

// GetProduct?DateStart=x&DateEnd=y
app.MapGet("/GetProduct", ([FromQuery] string DateStart, [FromQuery] string DateEnd)=> 
{
    return DateStart + " - " + DateEnd;
});

app.MapGet("/GetProductHeader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});





app.MapPost("/Products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/proucts/{product.Code}", product.Code);
});

// GetProduct/exemplo
app.MapGet("/Products/{code}", ([FromRoute] string code)=> {
    var product = ProductRepository.GetBy(code);
    if (product != null)
    {
        Console.WriteLine("Product Found");
        return Results.Ok(product);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPut("/Products", (Product Product) => {
    var ProductSaved = ProductRepository.GetBy(Product.Code);
    ProductSaved.Name = Product.Name;
    return Results.Ok();
});

app.MapDelete("/Products/{code}", ([FromRoute] string Code) => {
    var ProductSaved = ProductRepository.GetBy(Code);
    ProductRepository.Remove(ProductSaved);
    return Results.Ok();
});

//mudança de ambiente
if (app.Environment.IsDevelopment())
{
    app.MapGet("/Configuration/Database", (IConfiguration configuration) => {
    return Results.Ok($"{configuration["Database:connection"]} / {configuration["Database:port"]}");
});
}


app.Run();


public static class ProductRepository{
    public static List<Product> Products { get; set; } = Products = new List<Product>();

    public static void Init(IConfiguration configuration){
        var products = configuration.GetSection("Products").Get<List<Product>>();
        Products = products;
    }

    public static void Add(Product product){
        Products = new List<Product>();
    }

    public static Product GetBy(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product){
    Products.Remove(product);
}

}


public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }

    public string Name { get; set; }
}

public class ApplicationDbContext : DbContext{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@sql2023;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}

