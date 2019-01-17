namespace QuickstartIdentityServer.CommonDTO
{
    /// <summary>
    /// 通用数据结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T>: BaseResult where T:class
    {
        public DataResult(T result)
        {
            Data = result;
            Result = true;
        }
        /// <summary>
        /// 结果
        /// </summary>
        public T Data { get; set; }
    }
}
