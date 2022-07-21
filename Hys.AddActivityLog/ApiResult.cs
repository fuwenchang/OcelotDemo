using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hys.AddActivityLog
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public string ErrMsg { get; set; }
        public string ErrCode { get; set; }
        public bool IsSuccess { get; set; }
    }

    public static class CommonResult
    {
        public static ApiResult<T> Success<T>(T data)
        {
            return new ApiResult<T>()
            {
                Data = data,
                IsSuccess = true
            };
        }

        public static ApiResult<T> Failed<T>(string errMsg, string errCode = "")
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrMsg = errMsg,
                ErrCode = errCode
            };
        }
    }
}
