using Microsoft.EntityFrameworkCore;
using Laboratorio08.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 1. Agregar servicios al contenedor
// ----------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Swagger (documentación)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de conexión PostgreSQL
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
);


// ----------------------------------------------------
// 2. Configurar el pipeline HTTP
// ----------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();