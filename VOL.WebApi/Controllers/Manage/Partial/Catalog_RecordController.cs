/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Catalog_Record",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VOL.Entity.DomainModels;
using Org.Manage.IServices;
using Microsoft.AspNetCore.Authorization;
using Org.Manage.Services;

namespace Org.Manage.Controllers
{
    public partial class Catalog_RecordController
    {
        private readonly ICatalog_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Catalog_RecordController(
            ICatalog_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddRecord")]
        [AllowAnonymous]
        public virtual ActionResult<string> AddRecord([FromBody] Catalog_Record date)
        {
            //添加到数据库
            var result = _service.AddEntity(date, false);
            //Console.WriteLine(result);
            //Console.WriteLine(catalog);
            //_service.GetCode(CatalogIdArrary)
            return Ok("新增成功");
        }
    }
}
