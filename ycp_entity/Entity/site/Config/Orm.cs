using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace site.Config {
    public class Orm {

        public static string CachePath = "/Xml";

        private static string xmlUrl = "";
        private static string manageUrl = "";
        private static string desktopUrl = "";
        private static string coreUrl = "";

        public static string CoreUrl {
            get {
                if (coreUrl == "") {

                    //取配置根下的 Database 部分
                    var orm = site.ConfigManager.Root.GetSection("Orm");
                    coreUrl = orm.GetSection("CoreUrl").Value;

                }
                return coreUrl;
            }
        }

        public static string DesktopUrl {
            get {
                if (desktopUrl == "") {

                    //取配置根下的 Database 部分
                    var orm = site.ConfigManager.Root.GetSection("Orm");
                    desktopUrl = orm.GetSection("DesktopUrl").Value;

                }
                return desktopUrl;
            }
        }
        public static string ManageUrl {
            get {
                if (manageUrl == "") {

                    //取配置根下的 Database 部分
                    var orm = site.ConfigManager.Root.GetSection("Orm");
                    manageUrl = orm.GetSection("ManageUrl").Value;

                }
                return manageUrl;
            }
        }

        public static string XmlUrl {
            get {
                if (xmlUrl == "") {

                    //取配置根下的 Database 部分
                    var orm = site.ConfigManager.Root.GetSection("Orm");
                    xmlUrl = orm.GetSection("XmlUrl").Value;

                }
                return xmlUrl;
            }
        }

    }
}
