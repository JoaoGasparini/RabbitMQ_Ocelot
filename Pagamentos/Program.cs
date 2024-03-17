using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pagamentos.ConsumerEvents;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

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

var VendaEfetuadaFila = configuration.GetSection("MassTransit")["VendaEfetuadaFila"] ?? String.Empty;
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

        cfg.ReceiveEndpoint(VendaEfetuadaFila, e =>
        {
            e.Consumer<PedidoEfetuadoConsumer>();
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<PedidoEfetuadoConsumer>();

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
