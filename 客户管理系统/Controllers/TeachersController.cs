using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class TeachersController : Controller
    {

        // GET: Teachers

        Models.CRMEntities ent = new Models.CRMEntities();
        public ActionResult Index()
        {
            return View();
        }
       /// <summary>
       /// 获取教师列表and分页
       /// </summary>
       /// <param name="lName"></param>
       /// <param name="tName"></param>
       /// <param name="tTel"></param>
       /// <param name="tRole"></param>
       /// <param name="pageIndex"></param>
       /// <returns></returns>
        public ActionResult GetTeachersView(string lName,string tName,string tTel,int? tRole,int pageIndex)
        {

            Func<Models.Teachers, bool> bol = m => {
                if(lName == null)
                {
                    return tRole == 0 ? m.T_Name.Contains(tName): m.T_Name.Contains(tName) && m.T_Role == tRole;
                }
                else
                {
                    bool name = m.T_LoginName.Contains(lName) && m.T_Name.Contains(tName) && m.T_Tel.Contains(tTel);
                    return tRole == 0 ? name : name && m.T_Role == tRole;
                }
              
            };          
            var list = ent.Teachers.Where(bol).Skip((pageIndex-1)*5).Take(5).ToList();
            var count = ent.Teachers.Where(bol).Count();
            int pageCount = 0;
            if (count % 5 == 0)
                pageCount = count / 5;
            else
                pageCount = count / 5+1;
            var tList = new
            {
                rows = list,
                total = pageCount

            };
            return Json(tList,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除教师
        /// </summary>
        /// <param name="tRole"></param>
        /// <returns></returns>
        /// 
        public ActionResult Del(int tID)
        {
            try
            {
                var info = ent.Teachers.FirstOrDefault(m => m.T_ID == tID);
                ent.Teachers.Remove(info);
                ent.SaveChanges();
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
           
        }
        

        /// <summary>
        /// 根据角色ID查询教师表
        /// </summary>
        /// <param name="tRole"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetTeacherTID(int tID)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var info = ent.Teachers.FirstOrDefault(m => m.T_ID == tID);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增或者修改用户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult AddOrUpdate(Models.Teachers info)
        {
            try
            {
                if (info.T_ID == 0)
                //新增
                {
                    info.T_IsEnabled = true;
                    ent.Teachers.Add(info);
                    ent.SaveChanges();
                    return Content("add");
                }
                else
                //修改
                {
                    var con = ent.Teachers.FirstOrDefault(m => m.T_ID == info.T_ID);
                    UpdateModel(con);
                    ent.SaveChanges();
                    return Content("update");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }

        }
        
        /// <summary>
        /// 设置是否启用
        /// </summary>
        /// <param name="tRole"></param>
        /// <returns></returns>
        /// 
        public ActionResult IsEnabled(int tID )
        {
            try
            {
                ent.Configuration.LazyLoadingEnabled = false;
                var info = ent.Teachers.First(m => m.T_ID == tID);
                if (info.T_IsEnabled)
                    info.T_IsEnabled = false;
                else
                    info.T_IsEnabled = true;
                UpdateModel(info);
                ent.SaveChanges();
                return Content("1");
            }
            catch
            {
                return Content("-1");
            }
           
        }
        
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="tRole"></param>
        /// <returns></returns>
        /// 
        public ActionResult UpdatePwd(int tID,string adminPwd,string userPwd)
        {
            try
            {
                ent.Configuration.LazyLoadingEnabled = false;
                var adminInfo = ent.Teachers.FirstOrDefault(m => m.T_ID == 1 && m.T_Pwd== adminPwd);
                if(adminInfo != null)
                {
                   var userInfo= ent.Teachers.FirstOrDefault(m => m.T_ID == tID);
                    userInfo.T_Pwd = userPwd;
                    UpdateModel(userInfo);
                    ent.SaveChanges();
                    return Content("1");
                }
                else
                {
                    return Content("管理员密码错误");
                }                                                             
            }
            catch(Exception ex)
            {
                return Content(ex.Message,ToString());
            }

        }
        /// <summary>
        /// 根据系统角色查询教师表
        /// </summary>
        /// <param name="tRole"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetTeachers(int? tRole)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var info = ent.Teachers.Where(m => m.T_Role == tRole).ToList();
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}