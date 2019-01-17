using System;

namespace QuickstartIdentityServer.CommonDTO
{
    public class ErrorResult : BaseResult
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Detail { get; set; }
        public ErrorResult(string msg)
        {
            Result = false;
            Message = msg;
        }

        public ErrorResult(Exception e)
        {
            Result = false;
            Message = e.Message;
        }

    }
}
