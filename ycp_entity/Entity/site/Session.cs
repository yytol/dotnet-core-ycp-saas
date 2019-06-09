using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site {
    public class Session : dpz.Mvc.Sessions.MemorySessionManager {

        //private dpz.Mvc.Sessions.Memorybase base;
        public const string S_YccSessionId = "ycc_session_id";
        public const string S_YccUserId = "ycc_user_id";
        public const string S_YccUserName = "ycc_user_name";
        public const string S_YccUserNick = "ycc_user_nick";

        public string YccSessionId { get { return base[S_YccSessionId]; } set { base[S_YccSessionId] = value; } }

        public string YccUserId { get { return base[S_YccUserId]; } set { base[S_YccUserId] = value; } }

        public string YccUserName { get { return base[S_YccUserName]; } set { base[S_YccUserName] = value; } }

        public string YccUserNick { get { return base[S_YccUserNick]; } set { base[S_YccUserNick] = value; } }

        public Session(dpz.Mvc.Sessions.MemorySessionManager.CacheManager cm = null, string sid = "") : base(cm, sid) {
            //base = new dpz.Mvc.Sessions.Memorybase(sid);
        }

        //public SiteSession(string sid = "") : base(site.Config.Redis.ConnectionString, false, sid, 60, "ycp.entities.manage:") {
        //    if (sid != "") {
        //        if (!CheckSessionId(sid, false)) throw new Exception("交互信息无效或已过期");
        //    }
        //}

    }
}
