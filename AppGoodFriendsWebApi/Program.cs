using Configuration;
using DbContext;
using DbRepos;
using Services;

var builder = WebApplication.CreateBuilder(args);

#region Insert standard WebApi services
// NOTE: global cors policy needed for JS and React frontends
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#endregion

#region configure swagger
builder.Services.AddSwaggerGen();
#endregion

#region Dependency Inject Custom logger
builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
#endregion

#region Dependency Inject services and repo

builder.Services.AddScoped<csAttractionDbRepos>();

builder.Services.AddScoped<IAttractionDataService, csAttractionDataService>();
#endregion


var app = builder.Build();

#region Configure the HTTP request pipeline


app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion

