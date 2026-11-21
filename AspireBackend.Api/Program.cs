using AspireBackend.Api.Shared.OpenApi;
using AspireBackend.Api.Shared.Authentication;
using AspireBackend.Api.Shared.Cors;
using AspireBackend.Api.Endpoints.Categories;
using AspireBackend.Api.Endpoints.Items;
using AspireBackend.Api.Endpoints.Files;
using AspireBackend.Api.Data;
using AspireBackend.Api.Services.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.AddSqlServerDbContext<ApplicationDbContext>("ShoppingListDb");

builder.AddAzureBlobServiceClient("picturestore");
builder.Services.AddSingleton<IBlobService, BlobService>();

builder.Services.AddOptions<AuthOptions>()
                .Bind(builder.Configuration.GetSection(AuthOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder();

builder.AddScalarApi();
builder.AddShoppingListCors();

builder.Services.AddValidation();

var app = builder.Build();
app.UseCors();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseScalar();
    app.ApplyMigrations();
}
else
{
    app.UseExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/scalar/v1"));
app.MapItems();
app.MapCategories();
app.MapFiles();

app.MapDefaultEndpoints();
app.Run();