using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using QuickstartIdentityServer.CommonDTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Filters
{
    /// <summary>
    /// 应用级别异常处理
    /// </summary>
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
       
        public WebApiExceptionFilterAttribute()
        {
        }
        /// <summary>
        /// 重写基类的异常处理方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var log = context.HttpContext.RequestServices.GetService(typeof(ILogger<WebApiExceptionFilterAttribute>)) as ILogger<WebApiExceptionFilterAttribute>;

            Exception ex = context.Exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            // Controller中，当前请求作用域内注册的Service
            //ILoggerProvider provider = context.HttpContext.RequestServices.GetService(typeof(ILoggerProvider)) as ILoggerProvider;
            //var log = provider.CreateLogger($"{context.RouteData.Values["controller"]}:{context.RouteData.Values["action"]}");
            //log.LogError($"请求参数:{ Newtonsoft.Json.JsonConvert.SerializeObject(context.RouteData.Values)}");
            var requestparam = $"请求参数:{ Newtonsoft.Json.JsonConvert.SerializeObject(context.RouteData.Values)}";
            ErrorResult result = new ErrorResult(ex);

            if (ex is SqlException)
            {
                result.Code = "1001";
                result.Message = "不好啦!数据库连接错误,请联系管理员!";
                log.LogError(ex, $"数据库访问异常,Request={requestparam};");
            }
            else if(ex is DbException)
            {
                result.Code = "1002";
                result.Message = "不好啦!数据库连接错误,请联系管理员!";
                log.LogError(ex, $"数据库访问异常,Request={requestparam};");
            }
            else if (ex is NullReferenceException)
            {
                result.Code = "1003";
                result.Message = "不好啦!一个对象某攻城狮忘记实现了,请联系管理员!";
                log.LogError(ex, $"对象未实例化异常,Request={requestparam};");
            }
            else if (ex is DllNotFoundException)
            {
                result.Code = "1004";
                result.Message = "不好啦!一个重要类库丢失,请联系管理员!";
                log.LogError(ex, $"找不到Dll异常,Request={requestparam};");
            }
            else if (ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                result.Code = "1005";
                result.Message = "您修改的记录不存在，或者已被更新，请刷新重新修改!";
                log.LogError(ex, $"您修改的记录不存在，或者已被更新，请刷新重新修改,Request={requestparam};");
            }
            else
            {
                result.Code = "9000";
                result.Message = ex.Message;
                log.LogError(ex, $"发生异常,Request={requestparam};");
            }
            result.Detail = ex.StackTrace;
            context.Result = new JsonResult(result);
        }
    }
}
