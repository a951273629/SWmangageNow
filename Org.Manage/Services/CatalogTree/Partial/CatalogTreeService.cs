/*
 *所有关于CatalogTree类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*CatalogTreeService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/

using EllipticCurve.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Org.Manage.IRepositories;
using Org.Manage.IServices;
using Org.Manage.Serice;
using System.Numerics;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;

namespace Org.Manage.Services
{
    public partial class CatalogTreeService : ICatalogTreeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICatalogTreeRepository _repository;//访问数据库
        private readonly TreeConverter _treeConverter; //自己写的需要注入的类

        private WebResponseContent webResponse = new WebResponseContent();

        [ActivatorUtilitiesConstructor]
        public CatalogTreeService(
            ICatalogTreeRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            TreeConverter treeConverter
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
            _treeConverter = treeConverter;
        }

        public List<CatalogTree> GetCatalogTreeAll()
        {
            //FromSqlInterpolated方法参数化查询，具体参照EF文档
            FormattableString formattableString = $"select * from CatalogTree";

            List<CatalogTree> CatalogTrees = _repository.FromSqlInterpolated(formattableString).ToList();
            //.Where(x => 1 == 1)
            //.Select(s => new
            //{
            //    s.User_Id,
            //    s.UserPwd
            //}).FirstOrDefaultAsync();
            //var Tc= new TreeConverter();
            var tree = _treeConverter.ConvertToTree(CatalogTrees);
            return tree;
        }

        /// <summary>
        /// 返回值为 int类型
        ///  成功返回最后一个流水号 失败返回-1
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public long GetSerialNumberById(long Id)
        {
            FormattableString formattableString = $"select [LastSerialNumber] from CatalogTree where ID={Id}";
            long?[] SerialNumber = _repository.FromSqlInterpolated(formattableString).Select(t => t.LastSerialNumber).ToArray();
            if (SerialNumber.Length == 0)
            {
                return -1;
            }
            if (SerialNumber[0] == null)
            {
                return -1;
            }

            return (long)SerialNumber[0];
        }

        /// <summary>
        /// 返回 一条完整的编码
        /// </summary>
        /// <param name="CatalogIdArrary"></param>
        /// <returns></returns>
        public string GetCode(int[] CatalogIdArrary)
        {
            string result = "";

            long? LastSerialNumber = 0;
            int Id = 0;

            //var result = new Catalog_Record();

            if (CatalogIdArrary.Length == 0)
            {
                result = "Id不正确无法生成编码";
                return result;
            }
            //var results = connection.QueryList<Customer>("SELECT * FROM Customers WHERE FirstName = @FirstName AND LastName = @LastName", parameters);
            for (int index = 0; index < CatalogIdArrary.Length; index++)
            {
                //{ CatalogIdArrary[index]}
                List<CatalogTree> catalogTrees = _repository.DapperContext.QueryList<CatalogTree>("" +
                    "select [CatalogTypeCode],[SeparatorCode],[LastSerialNumber] from CatalogTree where ID=@Id",
                    new { Id = CatalogIdArrary[index] });
                if (catalogTrees.Count == 0)
                {
                    result = "Id不正确无法生成编码";
                    break;
                }
                CatalogTree catalogNow = catalogTrees[0];
                result += $"{catalogNow.CatalogTypeCode}{catalogNow.SeparatorCode}";
                //遍历到最后一个了可以取流水号了
                if (index == CatalogIdArrary.Length - 1)
                {
                    result += catalogNow.LastSerialNumber;
                    //从数据库更新流水号
                    LastSerialNumber = catalogNow.LastSerialNumber + 1;
                    Id = CatalogIdArrary[index];
                    break;
                }
            }
            //更新
            _repository.DapperContext.ExcuteNonQuery("UPDATE [dbo].[CatalogTree] SET [LastSerialNumber] = @LastSerialNumber  WHERE ID=@Id",
                new { Id, LastSerialNumber });
            //返回编码
            return result;
        }

        //申请编码
        public Catalog_Record GetCodeComplete(Submit date)
        {
            Catalog_Record result = new Catalog_Record();

            long? LastSerialNumber = 0;
            int Id = 0;

            //var result = new Catalog_Record();

            if (date.value.Length == 0)
            {
                result.PartNumber = "Id不正确无法生成编码";
                return result;
            }
            int[] IDs = date.value;
            //先获取最后一位流水号，再获取前面的编码字段
            //var results = connection.QueryList<Customer>("SELECT * FROM Customers WHERE FirstName = @FirstName AND LastName = @LastName", parameters);
            for (int index = 0; index < date.value.Length; index++)
            {
                //{ CatalogIdArrary[index]}
                List<CatalogTree> catalogTrees = _repository.DapperContext.QueryList<CatalogTree>("" +
                    "select * from CatalogTree where ID=@Id",
                    new { Id = date.value[index] });
                if (catalogTrees.Count == 0)
                {
                    result.PartNumber = "Id不正确无法生成编码";
                    break;
                }
                CatalogTree catalogNow = catalogTrees[0];
                result.PartNumber += $"{catalogNow.CatalogTypeCode}{catalogNow.SeparatorCode}";
                //遍历到最后一个了可以取流水号了
                if (index == date.value.Length - 1)
                {
                    //完整的分级
                    result.CompletedTypeCode = result.PartNumber.Substring(0, result.PartNumber.Length - 1);

                    //当前流水号
                    LastSerialNumber = catalogNow.LastSerialNumber;

                    Id = date.value[index];
                    // 取物料的ID和名称
                    result.ItemId = Id;
                    result.ItemName = catalogNow.CatalogTypeName;
                    result.ItemVersion = catalogNow.Version;

                    result.CreatedTime = DateTime.Now;
                    result.ModifiedDate = DateTime.Now;
                    result.Remark = date.message;
                    break;
                }
            }
            //找到一个可用的编码
            while (true)
            {
                //用当前生成的完整编码去数据库查看当前编码有没有重复
                result.PartNumber = $"{result.PartNumber}{LastSerialNumber}";
                int sum = (int)_repository.DapperContext.ExecuteScalar("select count(*) from Catalog_Record where [PartNumber]=@PartNumber", new { result.PartNumber });
                if (sum == 0)
                {
                    break;
                }
                LastSerialNumber++;

            }
            //下一次可用的流水号保存起来，下次直接拿
            result.SerialNumber = LastSerialNumber+1;
            //更新
            _repository.DapperContext.ExcuteNonQuery("UPDATE [dbo].[CatalogTree] SET [LastSerialNumber] = @LastSerialNumber  WHERE ID=@Id",
                new { Id, LastSerialNumber= result.SerialNumber });

            result.CreatedBy = "Admin";
            result.ModifiedBy = "Admin";
            //返回编码
            return result;
        }

        public CatalogTree GetRuleTypeCode(int ID)
        {
            return _repository.DapperContext.QueryFirst<CatalogTree>("select [RuleTypeCode] from CatalogTree where ID=@ID", new { ID });
        }

        //判断一个父级下面有没有相同的子级编码，如果有表示这个编码重复了。
        public bool IsSameTypeCode(CatalogTree child)
        {
           int count = (int)_repository.DapperContext.ExecuteScalar("select count(*) from " +
                "(SELECT CatalogTypeCode FROM [CatalogTree] where ParentID=@ID)" +
                " As CatalogTypeCode where CatalogTypeCode=@CatalogTypeCode", new {child.ID, child.CatalogTypeCode });
            //编码重复就返回true 不重复就返回false
            if (count > 0) return true;
            return false;
        }
        /// <summary>
        /// 递归删除所有的子级
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            //删除前处理
            //删除的行的主键
            DelOnExecuting = (object[] _keys) =>
            {
                int count = 0;
                var transaction = _repository.DapperContext.BeginTrans();
                try
                {
                    foreach (var ID in _keys)
                    {
                        // 递归删除 所有子级
                        count = _repository.DapperContext.ExcuteNonQuery("WITH RecursiveCTE AS " +
                        "(SELECT ID, ParentID  FROM CatalogTree   WHERE ID = @ParentID   UNION ALL " +
                        "  SELECT t.ID, t.ParentID FROM CatalogTree t " +
                        "INNER JOIN RecursiveCTE cte ON t.ParentID = cte.ID)" +
                        "DELETE FROM CatalogTree WHERE ID IN (SELECT ID FROM RecursiveCTE)", new { ParentID = _keys });
                        Console.WriteLine(count);
                    }
                    //提交事务
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // 回滚事务
                    transaction.Rollback();
                    return webResponse.Error($"删除失败");
                }
                return webResponse.OK($"删除{count}行成功");
            };
            return DelOnExecuting(keys);
            //return base.Del(keys, delList);
        }
    }
}