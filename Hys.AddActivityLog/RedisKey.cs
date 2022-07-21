using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hys.AddActivityLog
{
    public static class RedisKey
    {
        public const string ActivityDaily = $"HysHsb:Activity:ActityDaily";

        public const string ActivityDailyId = "HysHsb:Activity:ActivityDailyId";
    }
}
