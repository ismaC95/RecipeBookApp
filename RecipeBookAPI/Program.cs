using RecipeBookAPI.Repositories;
using RecipeBookAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency injection -to study-, another option to 
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CategoryServices>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<RecipeServices>();
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<IngredientRepository>();
builder.Services.AddScoped<RecipeIngredientRepository>();
builder.Services.AddScoped<SearchServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
