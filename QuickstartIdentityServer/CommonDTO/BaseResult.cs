
namespace QuickstartIdentityServer.CommonDTO
{
    /// <summary>
    /// 返回结果基类
    /// </summary>
    public class BaseResult
    {
        public BaseResult()
        {
            Result = true;
        }

        /// <summary>
        /// 业务处理是否成功
        /// </summary>
        public bool Result { get; set; }
    }
}
