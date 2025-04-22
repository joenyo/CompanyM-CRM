using Microsoft.EntityFrameworkCore;
using CompanyM_CRM.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container BEFORE building the app
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64;
});

// Add database context
builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy - ADD THIS HERE BEFORE building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Use CORS - ADD THIS HERE after building but before other middleware
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();