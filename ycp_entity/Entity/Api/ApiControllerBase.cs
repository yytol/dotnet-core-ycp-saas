using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Api {
    public class ApiControllerBase : ControllerBase {

        /// <summary>
        /// 输出对象
        /// </summary>
        protected dpz.Jsons.Jttp JResponse { get; private set; }

        /// <summary>
        /// 输出用的数据对象
        /// </summary>
        public dynamic JData { get; private set; }

        public ApiControllerBase() {
            JResponse = new dpz.Jsons.Jttp();
            JData = JResponse.Data;
        }

        /// <summary>
        /// 获取表单数据填充后的字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetFormData() {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            ICollection<string> keys = Request.Form.Keys;
            foreach (string key in keys) {
                pairs.Add(key, Request.Form[key]);
            }
            //for (int i = 0; i < Request.Form.Count; i++) {
            //    pairs[[i]] = Request.Form[i];
            //}
            return pairs;
        }

        protected string Success() {
            JResponse.Header.Status = "1";
            return JResponse.ToJson();
        }

        protected string Fail(string msg = "") {
            JResponse.Header.Status = "0";
            JResponse.Message = msg;
            return JResponse.ToJson();
        }

        protected string Error(int code = 0, string msg = "") {
            JResponse.Header.Status = "-1";
            JResponse.Header.Error = "" + code;
            JResponse.Message = msg;
            return JResponse.ToJson();
        }

    }
}
