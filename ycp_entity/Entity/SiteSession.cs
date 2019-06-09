using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity {
    public class SiteSession : site.Session {
        public SiteSession(string sid = "") : base(Program.CacheManager, sid) {
            //base = new dpz.Mvc.Sessions.Memorybase(sid);
        }
    }
}
