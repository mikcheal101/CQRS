using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infra;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain;
using Post.Cmd.Infra;
using Post.Cmd.Infra.Config;
using Post.Cmd.Infra.Handlers;
using Post.Cmd.Infra.Repositories;
using Post.Cmd.Infra.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();


// Register Command Handler Methods
var CommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var Dispatcher = new CommandDispatcher();
Dispatcher.Registerhandler<NewPostCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<EditMessageCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<LikeMessageCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<AddCommentCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<EditCommentCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<RemoveCommentCommand>(CommandHandler.HandleAsync);
Dispatcher.Registerhandler<DeletePostCommand>(CommandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => Dispatcher);


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
