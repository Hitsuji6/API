using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/user", () => new{Name = "Eduardo Veiga", Age = 21});

app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Eduardo", "Veiga");
    return new {Name = "Eduardo Veiga", Age = 21};
});

app.MapPost("/SaveProduct", (Product product) => 
{
    return product.Code + " - " + product.Name;
});

// GetProduct?DateStart=x&DateEnd=y
app.MapGet("/GetProduct", ([FromQuery] string DateStart, [FromQuery] string DateEnd)=> 
{
    return DateStart + " - " + DateEnd;
});

// GetProduct/exemplo
app.MapGet("/GetProduct/{code}", ([FromRoute] string code)=> 
{
    return code;
});

app.MapGet("/GetProductHeader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});




app.Run();



public class Product
{
    public string Code { get; set; }

    public string Name { get; set; }
}

