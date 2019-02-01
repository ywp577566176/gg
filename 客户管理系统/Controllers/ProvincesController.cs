using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class ProvincesController : Controller
    {
        // GET: Provinces
        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <returns></returns>
        Models.CRMEntities ent = new Models.CRMEntities();
        public ActionResult GetProvinces()
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var list = ent.Provinces.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}