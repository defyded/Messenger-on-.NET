using Messenger.Hubs;
using Messenger.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MessengerDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IChatService, ChatService>();
//builder.Services.AddScoped<IMessageService, MessageService>();
//builder.Services.AddScoped<IAttachmentService, AttachmentService>();

//builder.Services.Configure<JwtOptions>(
//    builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

app.Run();
