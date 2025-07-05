var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AsteriskService>();
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddSingleton<DiscadorService>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();
