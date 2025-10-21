using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Helpers;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Data.Seeder;
using AutomotiveApp.Infrastructure.Implementation.Repositories;
using AutomotiveApp.Infrastructure.Implementation.Utils;
using AutomotiveApp.Infrastructure.Repositories;
using AutomotiveApp.WebAPI.Validators.Course;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutomotiveApp.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//Fluent Validation
builder.Services.AddValidatorsFromAssemblyContaining<GetCourseByIdValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.EnableAnnotations();
});

// DB Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sql => sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
));

// MediatR + AutoMapper
builder.Services.AddMediatR(typeof(GetCoursesPagedHandler).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repository & UnitOfWork
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
builder.Services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();
builder.Services.AddScoped<ICourseBookingRepository, CoursebookingRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderitemRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Email Service
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Configuration (appsettings.json)
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
?? throw new InvalidOperationException("JWT settings are not configured in appsettings.json.");
builder.Services.AddSingleton<IJwtSettings>(jwtSettings);


//Utils (Storage)
builder.Services.AddSingleton<IFileStorage, LocalImageStorage>();

//Helpers 
builder.Services.AddScoped<UrlGeneratorHelper>();
// Add generic repository untuk semua entity
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false; // TODO: ganti jadi true nanti

    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager<SignInManager<User>>();

// Authentication
// builder.Services.AddScoped<IJwtSettings, JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidateLifetime = jwtSettings.ValidateLifetime,
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew)
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"JWT validated for user {userId}");
                return Task.CompletedTask;
            },
            OnChallenge = async context =>
                    {
                        // Skip the default 401 response
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var apiResponse = new
                        {
                            Success = false,
                            StatusCode = 401,
                            Errors = new[] { "Unauthorized access. Please login." }
                        };

                        await context.Response.WriteAsJsonAsync(apiResponse);
                    }
        };
    });

// Authorization
builder.Services.AddAuthorization(AuthorizationPolicies.ConfigurePolicies);

// Add JWT Authentication to Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Transaction Error Handling API with Authentication",
        Version = "v1",
        Description = "API for demonstrating transaction management, error handling, and JWT authentication in ASP.NET Core"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
});

// Cors
var allowedOrigins = new[] { "https://localhost:5160" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

Console.WriteLine("Current environment: " + app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutomotiveApp API V1");
        c.RoutePrefix = string.Empty; // serve Swagger UI at root "/"
    });
}

//global error handling 
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Images")),
    RequestPath = "/images"
});


app.UseHttpsRedirection();
app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await MasterSeeder.SeedAsync(db, userManager, roleManager, true);
}

app.Run();