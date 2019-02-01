﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace 客户管理系统.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Models.CRMEntities ent = new Models.CRMEntities();
       
        /// <summary>
        /// 进入管理员主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 进入校区
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchool()
        {
            return View("ManageSchool");
        }
        /// <summary>
        /// 校区管理
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult ManageSchool(string sCode, string sName,string pageIndex)
        {
            if (sName == null)
            {
                return View();
            }
            else
            {
                ent.Configuration.LazyLoadingEnabled = false;
               
                Func<Models.Schools, bool> bol = (m) =>
                {
                    bool id = string.IsNullOrEmpty(sCode) ? true : m.School_Code == sCode;
                    bool name = string.IsNullOrEmpty(sName) ? true : m.School_Name == sName;
                    return id & name;
                };
                 int page = 1;
                 if (!string.IsNullOrEmpty(pageIndex))
                 {
                    page = int.Parse(pageIndex);
                 }
               
                var list = ent.Schools.Where(bol).Skip((page - 1) * 5).Take(5).ToList();
                var count = ent.Schools.Where(bol).ToList().Count;
                for(int t=0;t<list.Count;t++)
                {
                    if(string.IsNullOrEmpty(list[t].School_Remark))
                    {
                        list[t].School_Remark = "";
                    }
                }
                int pageCount = 0;
                if(count%5==0)
                {
                    pageCount = count / 5;
                }
                else
                {
                    pageCount = count / 5+1;
                }
                var info = new
                {
                    rows = list,
                    total = pageCount
                };
                
                return Json(info, JsonRequestBehavior.AllowGet);
            }
               
           
          
        }
        /// <summary>
        /// 删除学校
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public ActionResult Del(int? sID)
        {
            try {
                Models.Schools info = ent.Schools.FirstOrDefault(m => m.School_ID == sID);
                ent.Schools.Remove(info);
                ent.SaveChanges();
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
          

        }
        /// <summary>
        /// 获取省份用于绑定下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProvinces()
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var info = ent.Provinces.ToList();
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改或者添加学校
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult AddOrUpdate(Models.Schools info)
        {
            try
            {
                if(string.IsNullOrEmpty(info.School_Remark))               
                    info.School_Remark = "";
                if (info.School_ID != 0)
                //修改
                {
                    Models.Schools school = ent.Schools.FirstOrDefault(m => m.School_ID == info.School_ID);
                    UpdateModel(school);
                    ent.SaveChanges();
                    return Content("update");
                }
                else
                //新增
                {
                    
                    ent.Schools.Add(info);
                    ent.SaveChanges();
                    return Content("add");
                }
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
           
        }
        public ActionResult ChooseJobs(string Specialty)
        {
            Models.View_ChooseJobs info = ent.View_ChooseJobs.FirstOrDefault();
            return View();

        }
    }
}