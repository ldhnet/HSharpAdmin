using HSharp.Model.Result.SystemManage;
using HSharp.Util.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Service.SystemManage
{
    public interface IDatabaseTableService
    {
        Task<bool> DatabaseBackup(string database, string backupPath);

        Task<List<TableInfo>> GetTableList(string tableName);

        Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination);

        Task<List<TableFieldInfo>> GetTableFieldList(string tableName);
    }
}