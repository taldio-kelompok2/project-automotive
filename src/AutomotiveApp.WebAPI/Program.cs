using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Application.Features.Users.Commands;
using AutomotiveApp.Application.Features.Users.Queries;
using AutomotiveApp.Application.Mapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Infrastructure.Data;
using AutomotiveApp.Infrastructure.Data.Seeder;
using AutomotiveApp.Infrastructure.Implementation.Repositories;
using AutomotiveApp.Infrastructure.Implementation.Utils;
using AutomotiveApp.WebAPI.Mapper;
using AutomotiveApp.WebAPI.Validators.Course;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GetCourseByIdValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

// DB Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR + AutoMapper
builder.Services.AddMediatR(typeof(GetCoursesPagedHandler).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repository & UnitOfWork
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(CourseProfile).Assembly);
builder.Services.AddMediatR(typeof(GetUserByIdHandler).Assembly);
builder.Services.AddMediatR(typeof(GetCoursesPagedHandler).Assembly);
builder.Services.AddMediatR(typeof(CreateUserHandler).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

//Utils (Storage)
builder.Services.AddSingleton<IFileStorage, LocalImageStorage>();
// Repository
builder.Services.AddScoped<IRepository<Course>, BaseRepository<Course>>();
builder.Services.AddScoped<IRepository<User>, BaseRepository<User>>();

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
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

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


// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await MasterSeeder.SeedAsync(db, userManager, roleManager);
}

app.Run();
