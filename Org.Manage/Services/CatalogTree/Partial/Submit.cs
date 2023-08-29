using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Manage.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"> ID数组 </param>
    /// <param name="message">备注信息</param>
    public record Submit(int[] value,string message);

}
