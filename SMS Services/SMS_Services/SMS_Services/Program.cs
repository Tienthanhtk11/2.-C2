using Microsoft.EntityFrameworkCore;
using SMS_Services.Background;
using SMS_Services.Model;
using SMS_Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var contexOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddTransient<ISMSRepository, SMSRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHostedService<SendSMS_Cronjob_Services>();
//builder.Services.AddHostedService<ReadSMS_Cronjob_Services>();

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


builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
}
app.MapControllers();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.Run();

