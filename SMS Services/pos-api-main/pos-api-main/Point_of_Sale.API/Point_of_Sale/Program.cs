using Point_of_Sale.Entities;
using Point_of_Sale.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Point_of_Sale.IRepositories;
using Point_of_Sale.Extensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var contexOptions = new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(connectionString).Options;
builder.Services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();
builder.Services.AddTransient<IPartnerRepository, PartnerRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IVoucherRepository, VoucherRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IRequestRepository, RequestRepository>();
builder.Services.AddTransient<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddTransient<IWarehouseExportRepository, WarehouseExportRepository>();
builder.Services.AddTransient<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddTransient<ISales_SessionRepository, Sales_SessionRepository>();
builder.Services.AddTransient<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddTransient<IRefundRepository, RefundRepository>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient<IDasboardRepository, DasboardRepository>();
builder.Services.AddTransient<IWarehouseInventoryRepository, WarehouseInventoryRepository>();
builder.Services.AddTransient<IPosFileRepository, PosFileRepository>();
builder.Services.AddTransient<IPromotionRepository, PromotionRepository>();



builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
string issuer = builder.Configuration["TokenSettings:Issuer"].ToString();
string audience = builder.Configuration["TokenSettings:Audience"].ToString();
string key = builder.Configuration["TokenSettings:Key"].ToString();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidAudience = audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());
});

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers().AddJsonOptions(o =>
{
   o.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dataContext.Database.Migrate();
}
app.MapControllers();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.Run();
