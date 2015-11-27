using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApiTrackLog.Helpers
{
    public class RedisCommon
    {
        private static readonly Lazy<RedisCommon> _instance = new Lazy<RedisCommon>(() => new RedisCommon());
        private static readonly string redisUrl = ConfigurationManager.AppSettings["RedisPath"];

        private RedisCommon()
        {

        }
        public static RedisCommon getInstance
        {
            get
            {

                return _instance.Value;
            }
        }

        public RedisClient getRedisClient()
        {
            return new RedisClient(redisUrl);
        }

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public bool Exist<T>(string hashId, string key)
        {
            bool result = false;
            using (var redis = this.getRedisClient())
            {
                result = redis.HashContainsEntry(hashId, key);
            }
            return result;
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set<T>(string hashId, string key, T t)
        {
            bool result = false;
            using (var redis = this.getRedisClient())
            {
                var value = JsonConvert.SerializeObject(t);
                result = redis.SetEntryInHash(hashId, key, value);
            }
            return result;
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public bool Remove(string hashId, string key)
        {
            bool result = false;
            using (var redis = this.getRedisClient())
            {
                result = redis.RemoveEntryFromHash(hashId, key);

            }
            return result;
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public bool Remove(string key)
        {
            bool result = false;
            using (var redis = this.getRedisClient())
            {
                result = redis.Remove(key);
            }
            return result;

        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public T Get<T>(string hashId, string key)
        {
            using (var redis = this.getRedisClient())
            {
                string value = redis.GetValueFromHash(hashId, key);
                return JsonConvert.DeserializeObject<T>(value);
            }

        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public List<T> GetAll<T>(string hashId)
        {
            using (var redis = this.getRedisClient())
            {
                var result = new List<T>();
                var list = redis.GetHashValues(hashId);
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        var value = JsonConvert.DeserializeObject<T>(x);
                        result.Add(value);
                    });
                }
                return result;
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        public void SetExpire(string key, DateTime datetime)
        {
            using (var redis = this.getRedisClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
    }
}