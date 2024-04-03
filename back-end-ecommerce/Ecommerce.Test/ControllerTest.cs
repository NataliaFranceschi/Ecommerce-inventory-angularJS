using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

public class LoginJson
{
    public string? token { get; set; }
}

public class ControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;

    public ControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    public async Task AuthenticateUserAsync()
    {
        var inputLogin = new
        {
            Email = "teste@teste.com",
            Password = "teste"
        };

        var responseLogin = await _httpClient.PostAsync("/login", new StringContent(JsonConvert.SerializeObject(inputLogin), System.Text.Encoding.UTF8, "application/json"));
        responseLogin.EnsureSuccessStatusCode();

        var responseLoginString = await responseLogin.Content.ReadAsStringAsync();
        LoginJson jsonLogin = JsonConvert.DeserializeObject<LoginJson>(responseLoginString);
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jsonLogin.token);
    }


    [Fact]
    public async Task GetProducts_ProductExist_ReturnSuccessWithProducts()
    {
        using(var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }
       
        await AuthenticateUserAsync();
        var response = await _httpClient.GetAsync("/product");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_SendingValidProduct_ReturnSuccess()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        var newProduct = new Product { Name = "Teste" };
        var body = JsonConvert.SerializeObject(newProduct);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        
        var response = await _httpClient.PostAsync("/product", content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_SendingInvalidProduct_ReturnBadRequest()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        var newProduct = new Product();
        var body = JsonConvert.SerializeObject(newProduct);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.PostAsync("/product", content);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetByName_MatchProduct_ReturnSuccessWithProduct()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);

            db.Products.Add(new Product { Name = "Test Name" });
            await db.SaveChangesAsync();
        }

        await AuthenticateUserAsync();

        string param = "name";
        var response = await _httpClient.GetAsync($"/product/search/{param}");
        var result = await response.Content.ReadFromJsonAsync<List<Product>>();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().HaveCount(1);

    }

    [Fact]
    public async Task GetByName_NoProductsMatch_ReturnSuccessWithEmptyList()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        string param = "testando";
        var response = await _httpClient.GetAsync($"/product/search/{param}");
        var result = await response.Content.ReadFromJsonAsync<List<Product>>();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeEmpty();

    }

    [Fact]
    public async Task GetById_ProductExists_ReturnSuccessWithProduct()
    {
        var productId = Guid.NewGuid();
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);

            db.Products.Add(new Product { Id = productId, Name = "Test Name" });
            await db.SaveChangesAsync();
        }

        await AuthenticateUserAsync();

        var response = await _httpClient.GetAsync($"/product/{productId}");
        var result = await response.Content.ReadFromJsonAsync<Product>();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().NotBeNull();

    }

    [Fact]
    public async Task GetById_ProductDoesNotExists_ReturnNotFound()
    {
        var nonExistId = Guid.NewGuid();
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        var response = await _httpClient.GetAsync($"/product/{nonExistId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

    }


    [Fact]
    public async Task Delete_ProductExists_ReturnSuccess()
    {
        var productId = Guid.NewGuid();
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);

            db.Products.Add(new Product { Id = productId, Name = "Test Product" });
            await db.SaveChangesAsync();
        }

        await AuthenticateUserAsync();

        var response = await _httpClient.DeleteAsync($"/product/{productId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ProductDoesNotExist_ReturnNotFound()
    {
        var nonExistentProductId = Guid.NewGuid(); 
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        var response = await _httpClient.DeleteAsync($"/product/{nonExistentProductId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ProductExists_ReturnSuccess()
    {
        var productId = Guid.NewGuid();
        var updatedProductName = "Updated Test Product";
        var updateRequest = new
        {
            Id = productId,
            Name = updatedProductName
        };
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);

            db.Products.Add(new Product { Id = productId, Name = "Test Product" });
            await db.SaveChangesAsync();
        }

        await AuthenticateUserAsync();
        var response = await _httpClient.PutAsJsonAsync($"/product", updateRequest);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ProductDoesNotExist_ReturnNotFound()
    {
        var nonExistentProductId = Guid.NewGuid();
        var updateRequest = new
        {
            Id = nonExistentProductId,
            Name = "Updated Test Product"
        };
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            db.Database.Migrate();
            Seeding.InitializeTestDB(db);
        }

        await AuthenticateUserAsync();

        var response = await _httpClient.PutAsJsonAsync($"/product", updateRequest);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}