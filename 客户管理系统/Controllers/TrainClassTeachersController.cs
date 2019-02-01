using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.IO;

namespace 客户管理系统.Controllers
{
    public class TrainClassTeachersController : Controller
    {
        Models.CRMEntities ent = new Models.CRMEntities();
        // GET: TrainClassTeachers
        //教师带班
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 教师所带班级信息
        /// </summary>
        /// <param name="tID">教师ID</param>
        /// <param name="tCate">教师身份</param>
        /// <returns></returns>
        public ActionResult GetTrainClass(int tID,int tCate,int type)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var info = ent.TrainClasses.ToList();
            //所有教师的带课情况
            var teachList = ent.View_TrainTeachers.Where(m => m.T_Role == tCate);

            var list = from tClass in ent.TrainClasses
                       join tList in teachList
                       on tClass.TC_ID equals tList.TC_ID
                       into toList
                       from newList in toList.DefaultIfEmpty()
                       select new
                       {
                           TC_ID = tClass.TC_ID,
                           TC_Name = tClass.TC_Name,
                           T_Name = newList.T_Name, 
                           T_ID= newList.T_ID,
                       };

            if (type == 0)
                list = list.Where(m => m.T_ID == null);
            else if (type == 1)
                list = list.Where(m => m.T_ID == tID);


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="tID"></param>
        /// <param name="tRole"></param>
        /// <returns></returns>
        public ActionResult Save(string json,int tID,int tRole)
        {
            using(var tran=ent.Database.BeginTransaction())
            {
                try
                {
                    var delList1 = ent.TrainClassTeachers.Where(m => m.T_ID == tID);
                    ent.TrainClassTeachers.RemoveRange(delList1);
                    ent.SaveChanges();

                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Models.TrainClassTeachers>));
                    MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
                    List<Models.TrainClassTeachers> List = (List<Models.TrainClassTeachers>)ser.ReadObject(ms);
                
                    var n = ent.View_TrainTeachers.ToList();
                    Func<Models.View_TrainTeachers, bool> bol = m =>
                       {

                           for (int t = 0; t < List.Count; t++)
                           {
                               if(t==2)
                               { string a = ""; }
                               if (m.T_Role == tRole && m.TC_ID == List[t].TC_ID)
                                   return true;
                           }
                           return false;
                       };
                    var obj = ent.View_TrainTeachers.Where(bol).ToList();
                    Func<Models.TrainClassTeachers, bool> b = m =>
                     {
                        
                         for (int t = 0; t < obj.Count; t++)
                         {
                             if (m.TC_ID == obj[t].TC_ID && m.T_ID== obj[t].T_ID)
                                 return true;
                         }
                         return false;

                     };

                    var delList3 = ent.TrainClassTeachers.Where(b).ToList();

                   
                    ent.TrainClassTeachers.RemoveRange(delList3);
                    ent.SaveChanges();

                    ent.TrainClassTeachers.AddRange(List);
                    ent.SaveChanges();

                    tran.Commit();
                    return Content("1");
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return Content(ex.Message.ToString());
                }
            }

        }
    }
}