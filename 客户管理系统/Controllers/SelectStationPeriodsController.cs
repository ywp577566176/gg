using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class SelectStationPeriodsController : Controller
    {
        // GET: SelectStationPeriods
        Models.CRMEntities ent = new Models.CRMEntities();
        /// <summary>
        /// 选岗设置
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="sID"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetView_ChooseJobs(int? pID,int? sID,string pageIndex)
        {
            if(pageIndex == null)
                return View();
            else
            {
                if (pID == null)
                    pID = 0;
                if (sID == null)
                    sID = 0;
                ent.Configuration.LazyLoadingEnabled = false;
                Func<Models.View_SchoolS, bool> bol = (m) =>
                   {
                       bool pD = true;
                       bool sD = true;
                       if (pID != 0)
                           pD = m.P_ID == pID;
                       if (sID != 0)
                           sD = m.School_ID == sID;
                       return pD && sD;

                   };
                int page = int.Parse(pageIndex);
                var a = ent.View_SchoolS.ToList();
                var list = ent.View_SchoolS.Where(bol).Skip((page - 1) * 5).Take(5).ToList();
                int count = ent.View_SchoolS.Where(bol).Count();
                for(int t=0;t<list.Count();t++)
                {
                    if (list[t].School_Remark == null)
                        list[t].School_Remark = "";
                }
                int pageCount = 0;
                if (count % 5 == 0)
                    pageCount = count / 5;
                else
                    pageCount = count / 5+1;
                var sList = new
                {
                    rows = list,
                    total = pageCount,
                };
                return Json(sList, JsonRequestBehavior.AllowGet);
            }
           
            
        }
    }
}