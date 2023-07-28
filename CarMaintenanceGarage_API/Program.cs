using CarMaintenanceGarage_API;
using CarMaintenanceGarage_API.DALC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDALC, MSSQL>();
builder.Services.AddScoped<BLC>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Use(async (context, next) =>
{
    // Add a cancellation token to the HttpContext for each request
    var cancellationToken = context.RequestAborted;
    context.Items["CancellationToken"] = cancellationToken;

    await next();
});
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
