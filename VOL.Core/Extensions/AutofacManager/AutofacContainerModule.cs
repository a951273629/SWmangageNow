using VOL.Core.Extensions;
using System;
using VOL.Core.Configuration;

namespace VOL.Core.Extensions.AutofacManager
{
    public class AutofacContainerModule
    {
        // where是一个约束 约束TService必须是一个class引用类型
        public static TService GetService<TService>() where TService:class
        {
            return typeof(TService).GetService() as TService;
        }
    }
}
