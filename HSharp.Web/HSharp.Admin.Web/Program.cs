using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 
using NLog.Web;
using HSharp.Admin.Web.Controllers; 
using HSharp.Util;
using HSharp.Util.Global; 
using Microsoft.AspNetCore.DataProtection; 
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;  
using Microsoft.Extensions.Hosting; 
using Newtonsoft.Json.Serialization;
//using StackExchange.Profiling.Storage; 
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using HSharp.Admin.Web.Hubs; 

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.WebHost.UseUrls(new[] { "http://*:9000" });
builder.WebHost.ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(LogLevel.Trace);
                   });
builder.WebHost.UseNLog();

var Configuration = builder.Configuration;
var env = builder.Environment;


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
}).AddNewtonsoftJson(options =>
{
    // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOptions();
builder.Services.AddSignalR();

    //builder.Services.AddSignalR(hubOptions =>
    //{
    //    //如果客户端在定义的时间跨度内没有响应，它将触发OnDisconnectedAsync
    //    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(60 * 60);
    //});

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(env.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

#region 初始化系统全局配置

GlobalContext.Services = builder.Services;

InitConfiguration();

    //全局静态配置热更新
    //ChangeToken.OnChange(() => Configuration.GetReloadToken(), () =>
    //{
    //    InitConfiguration();
    //});

#endregion
 
#region 注册MiniProfiler性能分析组件

//builder.Services.AddMiniProfiler(options =>
//{
//    options.RouteBasePath = GlobalContext.MiniProfilerConfig.RouteBasePath;

//    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(GlobalContext.MiniProfilerConfig.CacheDuration);

//    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

//}).AddEntityFramework();//显示SQL语句及耗时;

#endregion

//env.ContentRootPath = env.ContentRootPath + "wwwroot";

GlobalContext.LogWhenStart(env);
GlobalContext.HostingEnvironment = env;

var app= builder.Build();

if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
{
    app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
}
if (env.IsDevelopment())
{
    GlobalContext.SystemConfig.Debug = true;
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

string resource = Path.Combine(env.ContentRootPath, "wwwroot");
//FileHelper.CreateDirectory(resource);
 
//app.UseStaticFiles(new StaticFileOptions
//{
//    RequestPath = "/wwwroot",
//    //FileProvider = new PhysicalFileProvider(resource),
//    OnPrepareResponse = GlobalContext.SetCacheControl
//});

app.UseSession();
app.UseRouting();

//MiniProfiler性能分析组件
//app.UseMiniProfiler();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<ChatHub>("/chathub");
});
GlobalContext.ServiceProvider = app.Services;

app.MapControllers();
app.Run();


#region 初始化全局静态配置

/// <summary>
/// 初始化全局静态配置
/// </summary>
void InitConfiguration()
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