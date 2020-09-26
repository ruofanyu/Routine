using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Routine.API.Data;
using Routine.API.Services;

namespace Routine.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
                
                #region ���������ʽ����һ��д��
                ////�ڷ��صĸ�ʽ������xml��ʽ��ԭ����֧��json��ʽ��
                //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                ////������������ͣ���ԭ����json���ȱ����xml����
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter()); 
                #endregion

            })
                //�˴����õ���patch����Ա��ʱ���ֲ�������Ϣʵ��EmployeeUpdateDto��Ա��ʵ��Employeeʵ����и���ת��ʱ������
                .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
                .AddXmlDataContractSerializerFormatters()     //��MVC3.0֮�����ڿ���ʹ���ں���׷�ӵķ�ʽ��д
                .ConfigureApiBehaviorOptions(setup =>
                {
                    setup.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDeatil = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "http://www.baidu.com",     //��������ͣ�һ����Բ����ĵ���֪���������Ǿ�ָ��ٶ�
                            Title = "�д���",
                            Status = StatusCodes.Status422UnprocessableEntity,     //�����״̬��
                            Detail = "�뿴��ϸ��Ϣ",
                            Instance = context.HttpContext.Request.Path            //�����url
                        };

                        problemDeatil.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);    //ָ�����������Ϣ��IDֵ
                        return new UnprocessableEntityObjectResult(problemDeatil)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });


            //�����Զ���ӳ����֮������automapper����Ϣ
            //  ��ȡ��ǰ�����е�������Ϣ
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddDbContext<RoutineDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
        }


        public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}