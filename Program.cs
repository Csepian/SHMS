using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SHMS.Authorize;
using SHMS.Data;
using SHMS.Repositories;
using SHMS.Services;  

// SHMS - initialize app and prepare for DI ,DB setup,other config
var builder = WebApplication.CreateBuilder(args);

// DB setup - SQL server connection using EFCore
builder.Services.AddDbContext<SHMSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnect")));

builder.Services.AddControllers();  // support API controller 
builder.Services.AddEndpointsApiExplorer();   // swagger support
builder.Services.AddSwaggerGen();

// register services for DI
builder.Services.AddScoped<IHotel, HotelServices>();   // repo-manage db operation ,services - handle business logic
builder.Services.AddScoped<IRoom, RoomServices>();
builder.Services.AddScoped<IReview, ReviewServices>();
builder.Services.AddScoped<IBooking, BookingService>();
builder.Services.AddScoped<IPayment, PaymentService>();
builder.Services.AddScoped<ITokenGenerate, TokenService>();
builder.Services.AddScoped<IUser, UserService>();

// check JSON response valid and not occur any issue it 
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
//add CORS support - allow react app to access API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
// JWT auth setup - only authorized user can acess API 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });

// Swagger Config - allow develpoer to test API in swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// middleware pipeline - order is important 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();