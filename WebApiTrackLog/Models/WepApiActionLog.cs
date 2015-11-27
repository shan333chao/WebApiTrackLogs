using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;

namespace WebApiTrackLog.Models
{
    /// <summary>
    /// webApi 请求记录
    /// </summary>
    [Table("WepApiActionLog")]
    public class WepApiActionLog
    {
        /// <summary>
        /// logId
        /// </summary> 
 
        
        public Guid Id { get; set; }
        /// <summary>
        /// 请求的地址
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// controller名字
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 操作的action
        /// </summary>
        public string actionName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 浏览器标示
        /// </summary>
        public string navigator { get; set; }
        /// <summary>
        /// 接口执行消耗毫秒的时间
        /// </summary>
        public double costTime { get; set; }
        /// <summary>
        /// 提交开始执行接口的时间
        /// </summary>
        public DateTime enterTime { get; set; }
        /// <summary>
        ///操作人的id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 访问时的token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comments { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        public string executeResult { get; set; }
        /// <summary>
        /// 客户端hostname
        /// </summary>
        public string userHostName { get; set; }
        /// <summary>
        /// 范围的前一个页面
        /// </summary>
        public string urlReferrer { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }
        /// <summary>
        /// 请求携带的参数
        /// </summary>
        public string paramaters { get; set; }
    }
}