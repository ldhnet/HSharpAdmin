using HSharp.Util.Model;
using System.Threading.Tasks;

namespace HSharp.Business.AutoJob
{
    public interface IJobTask
    {
        Task<TData> Start();
    }
}