/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Prod_MaterialMessageController编写
 */
using Microsoft.AspNetCore.Mvc;
using VOL.Core.Controllers.Basic;
using VOL.Entity.AttributeManager;
using Org.Manage.IServices;
namespace Org.Manage.Controllers
{
    [Route("api/Prod_MaterialMessage")]
    [PermissionTable(Name = "Prod_MaterialMessage")]
    public partial class Prod_MaterialMessageController : ApiBaseController<IProd_MaterialMessageService>
    {
        public Prod_MaterialMessageController(IProd_MaterialMessageService service)
        : base(service)
        {
        }
    }
}

