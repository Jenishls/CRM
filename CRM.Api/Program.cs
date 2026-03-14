
using CRM.Application;
using CRM.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

