using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HSharp.Util.Model;

namespace HSharp.Business.AutoJob
{
    public interface IJobTask
    {
        Task<TData> Start();
    }
}
