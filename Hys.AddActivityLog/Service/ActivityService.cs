using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSRedis;

using Furion.DatabaseAccessor;

using Hys.AddActivityLog.Models;

namespace Hys.AddActivityLog
{
    public class ActivityService : IActivityService
    {
        private readonly HysHsbDbContext _context;
        private CSRedisClient _csredis;
        public ActivityService(
            HysHsbDbContext context,
            CSRedisClient csredis
            )
        {
            _context = context;
            _csredis = csredis;
        }

        /// <summary>
        /// 新增操作日志
        /// </summary>
        /// <param name="activityDailies"></param>
        /// <returns></returns>
        public async Task AddActivityDaily()
        {
            try
            {
                var activityDailies = new List<ActivityDaily>();
                while (_csredis.LLen(RedisKey.ActivityDaily) > 0 && activityDailies.Count < 50)
                {
                    var result = _csredis.LPop<ActivityDaily>(RedisKey.ActivityDaily);

                    activityDailies.Add(result);
                }

                if (activityDailies.Count > 0)
                {
                    await _context.ActivityDailies.AddRangeAsync(activityDailies);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
