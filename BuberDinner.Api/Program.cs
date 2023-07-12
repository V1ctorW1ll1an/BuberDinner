using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
{
    // replace this
    // builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    // with this
    builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
    builder.Services.AddControllers();
}

var app = builder.Build();


// Configure the HTTP request pipeline.
{
    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
