using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace 客户管理系统.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        Models.CRMEntities contex = new Models.CRMEntities();
        public ActionResult Index(string userName, string pwd)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            else
            {
                Models.Teachers teacher = contex.Teachers.FirstOrDefault(m => m.T_LoginName == userName && m.T_Pwd == pwd);
                if(teacher==null)
                {
                    ViewBag.err = "用户或密码错误，请重试";
                    ViewBag.name = userName;
                    return View();
                }
                else
                {
                    if (teacher.T_Role == 1)
                    {
                        ViewBag.Role = 1;
                        return RedirectToAction("UserIndex", "Home");
                    }
                       
                    else if (teacher.T_Role == 2)
                    {
                        ViewBag.Role = 2;
                        return RedirectToAction("UserIndex", "Home");
                    }
                       
                    else if (teacher.T_Role == 3)
                    {
                        ViewBag.Role = 3;
                        return RedirectToAction("AdminIndex","Home");
                    }
                       
                    else
                    {
                        ViewBag.Role = "错误";
                        return View();
                    }
                       
                }
               

            } 
        }


    }      
    
}