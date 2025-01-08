using Microsoft.AspNetCore.Mvc; 
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;  
using HSharp.Util;

namespace HSharp.Admin.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerStatusController : ControllerBase
    {  
        [HttpGet("status")]
        public async Task GetServerStatus()
        {
            // 设置响应头，声明是 SSE 流
            Response.ContentType = "text/event-stream";
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive"); 
            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);
            // 获取当前进程的基本信息
            var process = Process.GetCurrentProcess();
            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {  
                var computerInfo = ComputerHelper.GetComputerInfo(); 
                // 获取 CPU 使用率
                var cpuUsage = computerInfo.CPURate; // CPU 使用率百分比
                var memoryUsage = computerInfo.RAMRate; // 内存使用（MB）
                var uptime = computerInfo.RunTime; // 服务器运行时间  
                var diskUsage = computerInfo.DiskUsage;  // 获取系统的磁盘使用情况 
                var status = new
                {
                    CPU = $"{cpuUsage:F2}",
                    Memory = $"{memoryUsage}",
                    Uptime = uptime,
                    DiskUsage = diskUsage,
                    MachineName = process.MachineName,
                    ProcessID = process.Id,
                    ProcessName = process.ProcessName, 
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }; 
                // 将状态信息转化为 JSON 格式并发送
                await writer.WriteLineAsync($"data: {System.Text.Json.JsonSerializer.Serialize(status)}\n");
                await writer.FlushAsync(); // 确保立即推送数据
                await Task.Delay(1000 * 2); // 每秒更新一次
            }
        }


    }
}
