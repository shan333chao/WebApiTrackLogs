using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiTrackLog.Models;

namespace WebApiTrackLog.Helpers
{
    public class TokenManager
    {

        public static RedisOpearteResult getToken(UserInfo val)
        {
            string tokenID = Guid.NewGuid().ToString();
            RedisOpearteResult result = new RedisOpearteResult
            {
                isok = RedisCommon.getInstance.Set<UserInfo>(RedisHashEnum.userinfo, tokenID, val),
                token = tokenID,
                result = JsonConvert.SerializeObject(val)
            };
            RedisCommon.getInstance.SetExpire(tokenID, DateTime.Now.AddMinutes(30));
            return result;
        }

        public static RedisOpearteResult RefreshLoginTokenData(String tokenID, UserInfo val)
        {
            RedisOpearteResult result = new RedisOpearteResult
            {
                isok = RedisCommon.getInstance.Set<UserInfo>(RedisHashEnum.userinfo, tokenID, val),
                token = tokenID,
                result = JsonConvert.SerializeObject(val)
            };
            RedisCommon.getInstance.SetExpire(tokenID, DateTime.Now.AddMinutes(30));
            return result;
        }

        /// <summary>
        /// 刷新用户token
        /// </summary>
        /// <param name="tokenID"></param>
        public static RedisOpearteResult RefreshUserToken(string tokenID)
        {
            var isExist = RedisCommon.getInstance.Exist<UserInfo>(RedisHashEnum.userinfo, tokenID);
            RedisOpearteResult result = new RedisOpearteResult
            {
                isok = isExist,
                token = tokenID,
                result = "Token过期"
            };
            if (isExist)
            {
                result.result = "成功延迟30分钟";
                RedisCommon.getInstance.SetExpire(tokenID, DateTime.Now.AddMinutes(30));
            }
            return result;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="tokenID"></param>
        /// <returns></returns>
        public static RedisOpearteResult LoginOff(string tokenID)
        {
            var isExist = RedisCommon.getInstance.Exist<UserInfo>(RedisHashEnum.userinfo, tokenID);
            RedisOpearteResult result = new RedisOpearteResult
            {
                isok = isExist,
                token = tokenID,
                result = "Token过期"
            };
            if (isExist)
            {
                result.result = "退出成功";
                RedisCommon.getInstance.Remove(RedisHashEnum.userinfo, tokenID);
            }

            return result;
        }

        /// <summary>
        /// 通过token 获取用户信息
        /// </summary>
        /// <param name="token">tokenID</param>
        /// <returns></returns>
        public static bool getUserByToken(string token, out UserInfo user)
        {
            bool isok = false;
            user = null;
            if (!string.IsNullOrEmpty(token) && RedisCommon.getInstance.Exist<UserInfo>(RedisHashEnum.userinfo, token))
            {
                user = RedisCommon.getInstance.Get<UserInfo>(RedisHashEnum.userinfo, token);
                isok = true;
            }
            return isok;
        }
    }
}