using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Routine.API.Data;
using Routine.API.Models;
using Routine.API.Services;

namespace Routine.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var tokenOption = Configuration.GetSection("TokenOption");
            var val = tokenOption.GetValue<string>("Role");//val = "IT"

            //方法二：配置绑定实例，以便在其他地方以注入方式使用
            services.Configure<TokenOption>(tokenOption);
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = false;

                #region 添加输出样式的另一种写法
                ////在返回的格式中添加xml格式（原本仅支持json格式）
                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                ////调整输出的类型，由原本的json优先变成了xml有限
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
                #endregion

            })
                //此处配置的是patch更新员工时，局部更新信息实体EmployeeUpdateDto和员工实体Employee实体进行更新转换时的配置
                .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
                .AddXmlDataContractSerializerFormatters()     //在MVC3.0之后，现在可以使用在后面追加的方式来写
                .ConfigureApiBehaviorOptions(setup =>
                {
                    setup.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetail = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "http://www.baidu.com",     //错误的类型，一般可以查阅文档得知，这里我们就指向百度
                            Title = "有错误！",
                            Status = StatusCodes.Status422UnprocessableEntity,     //错误的状态码
                            Detail = "请看详细信息",
                            Instance = context.HttpContext.Request.Path            //错误的url
                        };

                        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);    //指向这个错误信息的ID值
                        return new UnprocessableEntityObjectResult(problemDetail)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });
            //添加自动化映射插件之后，配置automapper的信息
            //获取当前的所有的配置信息
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddScoped<DbContext, RoutineDbContext>();

            services.AddDbContext<RoutineDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "测试智能",
                    Description = ".net core & linux",
                    Version = "1.0.0"
                });
            });
        }


        //public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
        //{

        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Linux Project");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
