using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection; 
namespace HSharp.Util;
public class HttpClientFactoryUtil
{
    private static IServiceProvider _serviceProvider;
    private static HttpClientFactoryUtil _instance;
    private static readonly object _lock = new object();
    private IHttpClientFactory _httpClientFactory;

    private HttpClientFactoryUtil()
    {
        // 初始化服务提供者
        var services = new ServiceCollection();
        services.AddHttpClient();
        _serviceProvider = services.BuildServiceProvider();
        _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
    }

    public static HttpClientFactoryUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HttpClientFactoryUtil();
                    }
                }
            }
            return _instance;
        }
    }

    public HttpClient CreateClient(string name = null)
    {
        return name == null ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(name);
    }
}