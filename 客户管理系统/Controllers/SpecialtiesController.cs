using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    /// <summary>
    /// 专业控制器
    /// </summary>
    public class SpecialtiesController : Controller
    {
        // GET: Specialties
        Models.CRMEntities ent = new Models.CRMEntities();
        /// <summary>
        /// 查询专业
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetSpecialties(string pageIndex)
        {
            if(string.IsNullOrEmpty(pageIndex))
            return View();
            else
            {
                ent.Configuration.LazyLoadingEnabled = false;
                int page = int.Parse(pageIndex);
                Func<Models.Specialties, bool> bol = (m) => true;
                var list = ent.Specialties.Where(bol).Skip((page-1)*5).Take(5).ToList();
                for (int t = 0; t < list.Count; t++)
                {
                    if (string.IsNullOrEmpty(list[t].Specialty_Description))
                    {
                        list[t].Specialty_Description = "";
                    }
                }
                var count = ent.Specialties.Count();
                int pageCount = 0;
                if (count % 5 == 0)
                    pageCount = count / 5;
                else
                    pageCount = count / 5+1;
                var sList = new
                {
                    rows = list,
                    total = pageCount
                };
                return Json(sList, JsonRequestBehavior.AllowGet);
                
            }
        }
        public ActionResult AddOrUpdate(Models.Specialties info)
        {
            try
            {
                if (info.Specialty_ID == 0)
                //新增
                {
                    ent.Specialties.Add(info);
                    ent.SaveChanges();
                    return Content("add");
                }
                else
                //修改
                {
                    var con = ent.Specialties.FirstOrDefault(m => m.Specialty_ID == info.Specialty_ID);
                    UpdateModel(con);
                    ent.SaveChanges();
                    return Content("update");
                }
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
           
        }
        public ActionResult Del(int? sID)
        {
            try
            {
                var info = ent.Specialties.FirstOrDefault(m => m.Specialty_ID == sID);
                ent.Specialties.Remove(info);
                ent.SaveChanges();
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
            
        }

        /// <summary>
        /// 绑定专业下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult DdlSpecialties()
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var list = ent.Specialties.ToList();
            return Json(list);
        }


    }
}