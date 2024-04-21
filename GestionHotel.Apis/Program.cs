using GestionHotel.Apis;
using GestionHotel.Apis.Endpoints.Booking;
using GestionHotel.Apis.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SampleInjectionInterface, SampleInjectionImplementation>();
builder.Services.AddScoped<ApiContext,  ApiContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapBookingsEndpoints();
app.Run();
