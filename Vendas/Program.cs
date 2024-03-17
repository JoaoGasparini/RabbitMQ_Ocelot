using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vendas.ConsumerEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var configuration = builder.Configuration;

var PagamentosProcessadoFila = configuration.GetSection("MassTransit")["ProcessarPagamentosFila"] ?? String.Empty;
var Servidor = configuration.GetSection("MassTransit")["Servidor"];
var Usuario = configuration.GetSection("MassTransit")["Usuario"];
var Senha = configuration.GetSection("MassTransit")["Senha"];

builder.Services.AddMassTransit((x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Servidor, "/", h =>
        {
            h.Username(Usuario);
            h.Password(Senha);
        });

        cfg.ReceiveEndpoint(PagamentosProcessadoFila, e =>
        {
            e.Consumer<PagamentosEventsConsumer>();
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<PagamentosEventsConsumer>();
}));

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
