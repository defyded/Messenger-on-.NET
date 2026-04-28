using Projects;


var builder = DistributedApplication.CreateBuilder(args);

// 1. Регистрируем ваш сервер мессенджера
// Это запустит ваш Web API / SignalR сервер
//var messengerServer = builder.AddProject<Projects.Messenger>("messenger-server");

// 2. Регистрируем MAUI (если добавили его в решение как Existing Project)
// Если не добавили, можно просто запустить сервер через Aspire, 
// а MAUI запускать отдельно, как вы делали раньше.


builder.Build().Run();
