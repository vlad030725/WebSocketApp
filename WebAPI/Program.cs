using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebSocketServer.BLL.Interfaces;
using WebSocketServer.BLL.Services;
using WebSocketServer.DAL;
using WebSocketServer.DAL.Interfaces;
using WebSocketServer.DAL.RepositorySQLite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebSocketContext>(options =>
{
    options.UseSqlite("Data Source=WebSocket.db");
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddTransient<IDbRepos, DbReposSQLite>();
builder.Services.AddTransient<IMessageService, MessageService>();

// Add services to the container.
builder.Services.AddSingleton<WebSocketHandler>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();
var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();

app.Map("/ws", webSocketHandler.HandleWebSocketAsync);


app.MapControllers();

app.Run();
