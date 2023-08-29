/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Prod_MaterialMessageService与IProd_MaterialMessageService中编写
 */
using Org.Manage.IRepositories;
using Org.Manage.IServices;
using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;

namespace Org.Manage.Services
{
    public partial class Prod_MaterialMessageService : ServiceBase<Prod_MaterialMessage, IProd_MaterialMessageRepository>
    , IProd_MaterialMessageService, IDependency
    {
    public Prod_MaterialMessageService(IProd_MaterialMessageRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IProd_MaterialMessageService Instance
    {
      get { return AutofacContainerModule.GetService<IProd_MaterialMessageService>(); } }
    }
 }
