using Microsoft.EntityFrameworkCore;
using TechCorpApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();

var app = builder.Build();

// Executa o banco de dados e aplica migrations na inicialização
using (var scope = app.Services.CreateScope())
{
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(5);
    var retries = 0;

    while (true)
    {
        try
        {
            db.Database.Migrate(); // tenta migrar
            break;
        }
        catch (Exception ex)
        {
            retries++;
            if (retries >= maxRetries)
            {
                Console.WriteLine("Erro ao conectar ao banco após várias tentativas.");
                throw;
            }

            Console.WriteLine($"Banco ainda não disponível... tentando novamente ({retries})");
            Thread.Sleep(delay);
        }
    }
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
