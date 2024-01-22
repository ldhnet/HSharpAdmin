using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Util.Global
{
    /// <summary>
    /// MiniProfiler配置
    /// </summary>
    public class MiniProfilerConfig
    {
        /// <summary>
        /// profiler URL
        /// </summary>
        public String RouteBasePath { get; set; }

        /// <summary>
        /// CacheDuration
        /// </summary>
        public int CacheDuration { get; set; }
    }
}
