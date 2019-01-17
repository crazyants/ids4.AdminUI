using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;
using System.Security.Cryptography;
using QuickstartIdentityServer.CommonDTO;

namespace QuickstartIdentityServer.Filters
{
    /// <summary>
    /// Post 缓存开启
    /// </summary>
    public class PostCacheAttribute : ActionFilterAttribute
    {
        private static string cache_dir = "dmp:";
        private readonly int expiry;

        /// <summary>
        /// 缓存时间  单位秒
        /// </summary>
        /// <param name="expiry"></param>
        public PostCacheAttribute(int expiry = 20)
        {
            this.expiry = expiry;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = cache_dir + EncryptUtil.GetMd5(context.HttpContext.Request.Path + JsonConvert.SerializeObject(context.ActionArguments));
            var db = context.HttpContext.RequestServices.GetService<IDatabase>();
            var cache = await db.StringGetAsync(key);
            if (cache.HasValue)
            {
                context.Result = new JsonResult(JsonConvert.DeserializeObject<object>(cache));
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is Microsoft.AspNetCore.Mvc.ObjectResult objresult)
            {
                DataResult<object> result = new DataResult<object>(objresult.Value);
                if (result.Result)
                {
                    var setting = new JsonSerializerSettings
                    {
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                    };
                    await db.StringSetAsync(key, JsonConvert.SerializeObject(result, setting), TimeSpan.FromSeconds(expiry));
                }
            }
        }
    }

    /// <summary>
    /// 加密帮助类
    /// </summary>
    public class EncryptUtil
    {
        //密钥对
        private const string PublicRsaKey = @"<RSAKeyValue><Modulus>k9q0+nZT1/7Hrwo0f11MVgfD3X74IDY2BHzy+l6uXl6DdDw8CaicNFD+V2ON2SbPUBmsTsCyeW6X5F21F9Sd6zM3DUy+S/3etz0R16aABfbvn9KqrwhnoQCdWDpStNN+t8/XoTVxUnagQvcBXj4ElWSLDIV6ApApI6YrZst7S9U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string PrivateRsaKey = @"<RSAKeyValue><Modulus>k9q0+nZT1/7Hrwo0f11MVgfD3X74IDY2BHzy+l6uXl6DdDw8CaicNFD+V2ON2SbPUBmsTsCyeW6X5F21F9Sd6zM3DUy+S/3etz0R16aABfbvn9KqrwhnoQCdWDpStNN+t8/XoTVxUnagQvcBXj4ElWSLDIV6ApApI6YrZst7S9U=</Modulus><Exponent>AQAB</Exponent><P>ywGlHZJbgJvO30uQ3RMGSklqeWxOR9wJ09ls6ouJMn2JoNDXxLgTzYnMg+Cg1hrhKgUNCR5lMgxJIaILbYhGLQ==</P><Q>unNmfQe1zE94ZIjNUHKH2zD9mz69wdGgIEsqSYx2NZ3tUO5sDbGKlS/qRmdMwRk6jdVEi/4Ivnu09iYDVKsNSQ==</Q><DP>e0isdaEEYL4+i+zgNOHw1/xt5P+ZKpq+fSF1u5qx3y/N/RPXdWg03oXo5c3s3xnD1DjtCbSj0BkV8I7wUbyIoQ==</DP><DQ>pPChocnIUc2bu5QpzRkEhit4rnV6eJNxDCBycE4J5LSo1AeXHyYIaqHQpieMBubCneYklZNNOPVGyNon6CbJwQ==</DQ><InverseQ>bIqUMIfs6GWkR6oFKsgrP5hxwCJYEH/O4EuVI8P0+N+EHweA9hHJwgBDygZbBxz0tvOnH0jQkV3uv/QCKvsdnA==</InverseQ><D>awLLI8/KhTUwvz6KmngjTMzX9RL1cRLra7onfhBS8ZhEWjdSWMdBLxhDzWoPe1B8kYPTUzhitELYCn/MFxBarouFNAcD87qopq1k0es+lMPzq8GHKShl3imXqFTi3vV3Y7abOdP3+lzA60lfeeoyTy4ux0463NvgHTd+LXSiM2E=</D></RSAKeyValue>";

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <returns></returns>
        public static string Rsa(string source)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicRsaKey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(source), true);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="source">待解密字段</param>
        /// <returns></returns>
        public static string UnRsa(string source)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateRsaKey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(source), true);
            return Encoding.UTF8.GetString(cipherbytes);
        }
        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMd5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
