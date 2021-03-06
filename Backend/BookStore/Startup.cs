using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using RepositoryLayer.Implementation;
using BusinessLayer.Implementation;
using BusinessLayer.Interface;
using Fundoo.Utilities;
using AutoMapper;
using TokenAuthorization;
using Greeting.TokenAuthorization;
using EmailService;
using BusinessLayer.MQServices;
using Caching;
using BusinessLayer.Utility;
using BusinessLayer.CloudServices;

namespace BookStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            DatabaseConfigurations connectionString = Configuration.GetSection("ConnectionStrings")
                                        .Get<DatabaseConfigurations>();
            EmailConfiguration emailConfiguration = Configuration.GetSection("EmailConfiguration")
                                        .Get<EmailConfiguration>();
            CacheConfiguration cacheConfiguration = Configuration.GetSection("CacheConfiguration")
                                        .Get<CacheConfiguration>();
            if (cacheConfiguration.IsEnabled)
            {
                services.AddStackExchangeRedisCache(options => options.Configuration = cacheConfiguration.ConnectionString);
                services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            }
            CloudConfiguration cloudConfiguration = Configuration.GetSection("CLOUDINARY")
                                        .Get<CloudConfiguration>();
            services.AddSingleton(cacheConfiguration);
            services.AddSingleton(emailConfiguration);
            services.AddSingleton(connectionString);
            services.AddSingleton(cloudConfiguration);
            services.AddTransient<IDBContext, DBContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IMqServices, MsmqServices>();
            services.AddScoped<IEmailItemDetails, EmailItemDetails>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICloudService, CloudService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwagger();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            });

            app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
