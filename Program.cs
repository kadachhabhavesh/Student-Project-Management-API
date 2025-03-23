using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;
using StudentProjectManagementAPI.Validation;
using StudentProjectManagementAPI.Validators;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow all origins (Use this only for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// jwt authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:Issuer"], 
            ValidAudience = builder.Configuration["jwt:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])) 
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Prevent default response
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Token is missing or invalid.\"}");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"You do not have permission to access this resource.\"}");
            }
        };
    });


var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<SpmContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

// Repository
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<FacultyRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<TeamMemberRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<EvaluationRepository>();
builder.Services.AddScoped<StudentEvaluationRepository>();
builder.Services.AddScoped<FileRepository>();
builder.Services.AddScoped<DashboardRepository>();

// Validators
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<UserValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<FacultyValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<EvaluationValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<FileValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<ProjectValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<StudentEvaluationValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<TaskValidator>(); });
builder.Services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<TeamMemberValidator>(); });



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });



var app = builder.Build();


app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
