using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dpz;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Entity.Api {
    [Route("Api/[controller]")]
    [ApiController]
    public class OrmController : ApiControllerBase {

        [HttpPost("GetTable")]
        public string GetTable() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    string tabName = "" + Request.Form["table"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[Aos]}}");
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

    }
}