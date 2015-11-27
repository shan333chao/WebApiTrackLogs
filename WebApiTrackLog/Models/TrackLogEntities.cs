namespace WebApiTrackLog.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TrackLogEntities : DbContext
    {
        public TrackLogEntities()
            : base("name=TrackLogEntities")
        {
        }
        /// <summary>
        /// webapi请求记录实体
        /// </summary>
        public IDbSet<WepApiActionLog> WepApiActionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
