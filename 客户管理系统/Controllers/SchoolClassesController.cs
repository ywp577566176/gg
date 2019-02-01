using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class SchoolClassesController : Controller
    {
        // GET: SchoolClasses
        Models.CRMEntities ent = new Models.CRMEntities();
        //班级
        /// <summary>
        /// 绑定班级下拉狂
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchoolClasses(int? sID)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var list = ent.SchoolClasses.Where(m => sID == 0 ? true : m.School_ID == sID);
            return Json(list);

        }
    }
}