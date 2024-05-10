using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.API.Middleware;
using Ecom.Core.Services;
using Ecom.Infrastructure;
using Ecom.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiRegestration();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
	var securitySchema = new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "JWT Auth Bearer",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		Reference = new OpenApiReference
		{
			Id = "Bearer",
			Type = ReferenceType.SecurityScheme
		}
	};
	x.AddSecurityDefinition("Bearer", securitySchema);
	var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
	x.AddSecurityRequirement(securityRequirement);
});
builder.Services.InfrastructureConfiguration(builder.Configuration);


// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
	var configure = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
	return ConnectionMultiplexer.Connect(configure);
});

// Configure Order Services
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Configure scope
InfrastructureRegister.InfrastructureConfigMiddleware(app);

app.Run();
