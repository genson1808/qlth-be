using FluentValidation;
using MISA.QLTH.API;
using MISA.QLTH.API.Configurations;
using MISA.QLTH.API.Middleware;
using MISA.QLTH.BL.BaseBL;
using MISA.QLTH.BL.DepartmentBL;
using MISA.QLTH.BL.EmployeeBL;
using MISA.QLTH.BL.RoomBL;
using MISA.QLTH.BL.SubjectBL;
using MISA.QLTH.DL;
using MISA.QLTH.DL.BaseDL;
using MISA.QLTH.DL.DepartmentDL;
using MISA.QLTH.DL.EmployeeDL;
using MISA.QLTH.DL.RoomDL;
using MISA.QLTH.DL.SubjectDL;
using FluentValidation.AspNetCore;
using MISA.QLTH.BL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IRoomDL, RoomDL>();
builder.Services.AddScoped<IRoomBL, RoomBL>();
builder.Services.AddScoped<ISubjectDL, SubjectDL>();
builder.Services.AddScoped<ISubjectBL, SubjectBL>();

//builder.Services.AddFluentValidation();
builder.Services.AddControllers().AddFluentValidation().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = ValidationResponse.MakeValidationResponse;
});
//builder.Services.AddScoped<IValidator<Employee>, CreateEmployeeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<IAssemplyMarker>();

// Add services to the container.
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MariaConnetionString");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services cors
//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddGlobalErrorHandler();

app.Run();