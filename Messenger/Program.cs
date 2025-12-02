using Messenger.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors(o =>
    o.AddDefaultPolicy(p =>
    p.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true))
);

var app = builder.Build();

app.UseCors();
app.MapGet("/", () => "Messenger is running...");
//app.MapHub<ChatHub>("/chat");
app.MapHub<AuthHub>("/auth");

app.Run();
