using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class StationsController : Controller
    {
        // GET: Stations
        Models.CRMEntities ent = new Models.CRMEntities();
        public ActionResult GetStations(string sName,string pageIndex)
        {
            if(sName==null)
                 return View();
            else
            {
                ent.Configuration.LazyLoadingEnabled = false;
                int page = int.Parse(pageIndex);
                var list = ent.Stations.Where((m) => m.Station_Name.Contains(sName)).ToList().Skip((page - 1) * 5).Take(5);
                int count = ent.Stations.Where((m) => m.Station_Name.Contains(sName)).Count();
                int pageCount = 0;
                if(count%5==0)
                {
                    pageCount = count / 5;
                }
                else
                {
                    pageCount = count / 5 + 1;
                }
                var sList = new
                {
                    rows = list,
                    total = pageCount
                };
            return Json(sList, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <returns></returns>
        public ActionResult Del(int? sID)
        {
            try
            {
                var info = ent.Stations.FirstOrDefault(m => m.Station_ID == sID);
                ent.Stations.Remove(info);
                ent.SaveChanges();
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
           
        }
        /// <summary>
        /// 修改或者添加岗位
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult AddOrUpdate(Models.Stations info)
        {
            try
            {
                if (string.IsNullOrEmpty(info.Station_NeedTechnique))
                    info.Station_NeedTechnique = "";
                if (info.Station_ID != 0)
                //修改
                {
                    Models.Stations station = ent.Stations.FirstOrDefault(m => m.Station_ID == info.Station_ID);
                    UpdateModel(station);
                    ent.SaveChanges();
                    return Content("update");
                }
                else
                //新增
                {

                    ent.Stations.Add(info);
                    ent.SaveChanges();
                    return Content("add");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }

        }
        /// <summary>
        /// 查询单个岗位
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetStation(int? sID)
        {
           
                ent.Configuration.LazyLoadingEnabled = false;
                var list = ent.Stations.FirstOrDefault(m => m.Station_ID == sID);                             
                return Json(list, JsonRequestBehavior.AllowGet);
            
        }
    }
}