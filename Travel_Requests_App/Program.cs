using Travel_Requests_App.Data;
using Microsoft.EntityFrameworkCore;
using Travel_Requests_App.BLL.Locations;
using Travel_Requests_App.BLL.TravelRequests;
using Travel_Requests_App.DAL.Locations;
using Travel_Requests_App.DAL.TravelRequests;
using Travel_Requests_App.BLL.ClientService;
using Travel_Requests_App.DAL.TravelBudgets;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TravelPlannerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TravelPlanner_db")));

builder.Services.AddScoped<ILocationServices, LocationsServicesClass>();
builder.Services.AddScoped<ILocationRepo, LocationsRepoClass>();

builder.Services.AddScoped<ITravelRequestService, TravelRequestService>();
builder.Services.AddScoped<ITravelRequestsRepo, TravelRequestsRepoClass>();
builder.Services.AddScoped<ITravelBudgetRepo, TravelBudgetRepository>();

builder.Services.AddScoped<HrClientService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient("HRService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7260"); // Replace with actual URL
});


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
