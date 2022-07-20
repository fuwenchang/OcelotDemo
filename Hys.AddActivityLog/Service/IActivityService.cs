using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hys.AddActivityLog.Models;

namespace Hys.AddActivityLog
{
    public interface IActivityService
    {
        /// <summary>
        /// 新增操作日志
        /// </summary>
        /// <param name="activityDailies"></param>
        /// <returns></returns>
        Task AddActivityDaily();
    }
}
