using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Разрешение CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
    options.LogTo(Console.WriteLine); // Логирование запросов в консоль
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;  // Для удобства чтения JSON
    });


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379";  // Укажи свой адрес Redis
});

builder.Services.AddSwaggerGen();



var app = builder.Build();

// Включаем CORS
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArticleApp API V1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5000");
