using Ecomm.Data;
using Ecomm.Infrastructure;
using Ecomm.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvc().AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddIdentity<AppUser, IdentityRole>()//Identity Configure/reg here with Role,Context and Token Providers
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

//Adding Jwt here for Authentication, not pipelining auth here
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // jwt auth scheme used here
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Add Jwt Bearer here
.AddJwtBearer(o => // jwt secret keys,codians, default url used here
{
    o.SaveToken = true; // For Saving Tokens
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, // TokenValidationParameters here, Issuer and Audience here true
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"], // hamein ValidAudience appsetting file se udhani hai, using Configuration prop we can access our appsetting file simply
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// If Swagger not Found use this proccess
builder.Services.AddSwaggerGen(c => // This is a swagger swasbuckel // Install Swashbuckle.AspNetCore package, AddSwaggerGen service me SwaggerDoc ka use kar Documentation teyar ki gai hai yahan
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecomm2", Version = "v1" }); // this is a Swager Title with its version information <- Swagger file isi ke hissab se banti hai jo ki SwaggerEndpoint me defined hai
});

//builder.Services.AddCors(o => // here defined Cors(Cross Origion Resource Sharing) Policy
//{
//    o.AddPolicy("angularApplication", (builder) => // for using our this api on diff client/domain without any restriction we req to provide its domain url here in Core policy compulsory that,s it, taaki ye url ye sabi methods use kar sake and exposeheader also.
//    {
//        builder.WithOrigins("http://localhost:4200") // WithOrigins means perticular Origion(client port no.(4200) with origion(localhost)) <- means ki is domain par agar koi bhi info ho to use vo/ye access karne de
//        .AllowAnyHeader()// Alow what-2 any Header info here
//        .WithMethods("GET", "POST", "PUT", "DELETE")
//        .WithExposedHeaders("*"); // ExposedHeaders me hmare pass wild card aa jaega ki koi bhi header info hai vo sari ki sari info exposed ho
//    });
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => // Here used Swagger swasbuckle as a middelware here with its UI
    {
        c.RouteTemplate = "/swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Ecomm2 v1")); // this is SwaggerEndpoint yahi hamari UI generate karta hai
}

app.UseCors("angularApplication"); // for using Cors, using by name
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
