using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Order_Application;
using Order_Application.Command;
using Order_Persistence;

namespace WebApplication_Order
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:7172"; // Duende IdentityServer
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("OrderScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "order.api");
                });
            });


            builder.Services.AddMessaging();
            builder.Services.AddApplication();
            builder.Services.AddPersistence(builder.Configuration);
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
        }
    }
}
