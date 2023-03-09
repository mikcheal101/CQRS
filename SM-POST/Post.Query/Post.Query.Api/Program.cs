using Microsoft.EntityFrameworkCore;
using Post.Query.Infra.DBAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Action<DbContextOptionsBuilder> _configureDbContext = (options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(_configureDbContext);
builder.Services.AddSingleton<DbContextFactory>(new DbContextFactory(_configureDbContext));

// Create Db and tables from code
var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dbContext.Database.EnsureCreated();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
