using Dapper.FluentMap;
using Library.Repositories;
using Library.Services;
using Library.Utils;

var builder = WebApplication.CreateBuilder(args);

FluentMapper.Initialize(config =>
{
    config.AddMap(new AuthorMap());
    config.AddMap(new CategoryMap());
    config.AddMap(new BookMap());
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<AuthorService>();
builder.Services.AddTransient<CategoryService>();

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<AuthorRepository>();
builder.Services.AddTransient<CategoryRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
