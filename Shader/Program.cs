using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Services;
using Shader.Services.Abstraction;
using Shader.Services.Implementation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShaderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  

builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IFruitService, FruitService>();
builder.Services.AddScoped<ICashTransactionService, CashTransactionService>();
builder.Services.AddScoped<IClientTransactionService, ClientTransactionService>();
builder.Services.AddScoped<ISupplierBillService, SupplierBillService>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IMerchantTransactionService, MerchantTransactionService>();
builder.Services.AddScoped<IClientPaymentService, ClientPaymentService>();
builder.Services.AddScoped<IMerchantPaymentService, MerchantPaymentService>();
builder.Services.AddScoped<IDailyEmpService, DailyEmpService>();
builder.Services.AddScoped<IDailyEmpAbsenceService, DailyEmpAbsenceService>();
builder.Services.AddScoped<IDailySRecordingService, DailySRecordingService>();
builder.Services.AddScoped<IMonthlyEmpService, MonthlyEmpService>();
builder.Services.AddScoped<IEmployeeLoanService, EmployeeLoanService>();
builder.Services.AddScoped<IMonthlyEmpAbsenceService, MonthlyEmpAbsenceService>();


builder.Services.AddControllers().AddJsonOptions(options =>
{ options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

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
