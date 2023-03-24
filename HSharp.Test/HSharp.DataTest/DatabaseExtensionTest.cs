using HSharp.Data;
using HSharp.Data.Repository;
using HSharp.Entity.SystemManage;
using HSharp.Model.Param.SystemManage;
using HSharp.Service.SystemManage;
using HSharp.Util.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSharp.DataTest
{
    public class DatabaseExtensionTest
    {
        /// <summary>
        /// 测试单字段排序
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SortTest()
        {
            RoleService roleService = new RoleService();
            RoleListParam roleListParam = new RoleListParam { };
            Pagination pagination = new Pagination
            {
                Sort = "RoleSort asc"
            };
            List<RoleEntity> list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].RoleSort < list[1].RoleSort);
        }

        /// <summary>
        /// 测试多字段排序
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task MultiSortTest()
        {
            RoleService roleService = new RoleService();
            RoleListParam roleListParam = new RoleListParam { };
            Pagination pagination = new Pagination
            {
                Sort = "Id desc,RoleSort asc"
            };
            List<RoleEntity> list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].Id > list[1].Id);
        }

        /// <summary>
        /// 测试如何获取Linq查询时对应的Sql
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task LinqGetSqlTest()
        {
            string sort = "RoleSort";
            bool isAsc = false;
            int pageSize = 10;
            int pageIndex = 1;
            RepositoryFactory repositoryFactory = new RepositoryFactory();
            var tempData = repositoryFactory.BaseRepository().db.dbContext.Set<RoleEntity>().AsQueryable();
            tempData = DatabasesExtension.AppendSort<RoleEntity>(tempData, sort, isAsc);
            tempData = tempData.Skip<RoleEntity>(pageSize * (pageIndex - 1)).Take<RoleEntity>(pageSize).AsQueryable();
            string strSql = DatabasesExtension.GetSql<RoleEntity>(tempData);
            Assert.IsTrue(strSql.ToUpper().Contains("SELECT"));
        }
    }
}