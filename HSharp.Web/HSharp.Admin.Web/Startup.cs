using HSharp.Admin.Web.Controllers;
using HSharp.Admin.Web.Hubs;
using HSharp.Util;
using HSharp.Util.Global; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives; 
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling.Storage;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace HSharp.Admin.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();

            services.AddOptions();
            services.AddSignalR();

            //services.AddSignalR(hubOptions =>
            //{
            //    //如果客户端在定义的时间跨度内没有响应，它将触发OnDisconnectedAsync
            //    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(60 * 60);
            //});

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            #region 初始化系统全局配置

            GlobalContext.Services = services;

            InitConfiguration();

            //全局静态配置热更新
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
            {
                InitConfiguration();
            });

            #endregion

            #region 初始化系统全局配置
            GlobalContext.Services = services;

            InitConfiguration();

            //全局静态配置热更新
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
            {
                InitConfiguration();
            });

            #endregion


            #region 注册MiniProfiler性能分析组件

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = GlobalContext.MiniProfilerConfig.RouteBasePath;

                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(GlobalContext.MiniProfilerConfig.CacheDuration);

                options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

            }).AddEntityFramework();//显示SQL语句及耗时;

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            {
                app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            }
            if (WebHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            FileHelper.CreateDirectory(resource);

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseSession();
            app.UseRouting();

            //MiniProfiler性能分析组件
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chathub");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
        }

        #region 初始化全局静态配置

        /// <summary>
        /// 初始化全局静态配置
        /// </summary>
        private void InitConfiguration()
        {
            GlobalContext.Configuration = Configuration;
            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.RedisConfig = Configuration.GetSection("RedisConfig").Get<RedisConfig>();
            GlobalContext.LogConfig = Configuration.GetSection("LogConfig").Get<LogConfig>();
            GlobalContext.MailConfig = Configuration.GetSection("MailConfig").Get<MailConfig>();    
            GlobalContext.RabbitMQConfig = Configuration.GetSection("RabbitMQConfig").Get<RabbitMQConfig>();     
            GlobalContext.MiniProfilerConfig = Configuration.GetSection("MiniProfilerConfig").Get<MiniProfilerConfig>();           
        }

        #endregion
    }
}