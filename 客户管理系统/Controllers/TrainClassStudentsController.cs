using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class TrainClassStudentsController : Controller
    {
        // GET: TrainClassStudents
        // 分班管理
        /// <summary>
        /// 进入分班的视图
        /// </summary>
        /// <returns></returns>
        Models.CRMEntities ent = new Models.CRMEntities();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="sID"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public ActionResult GetViewStudents(int pID, int sID, int classID, int type,int pageIndex)
        {
            Func<Models.View_Students, bool> bol = m =>
               {
                   bool bP = true;
                   bool bS = true;
                   bool bC = true;
                   bool bT = true;
                   if (pID != 0)
                       bP = m.P_ID == pID ? true : false;
                   if (sID != 0)
                       bS = m.School_ID == sID ? true : false;
                   if (classID != 0)
                       bC = m.SC_ID == classID ? true : false;
                   if (type == 0)
                       bT = m.TC_Name == null ? true : false;
                   else if(type == 1)
                       bT= m.TC_Name != null ? true : false;
                   return bP && bS && bC &&bT;
               };
            var list = ent.View_Students.Where(bol).Skip((pageIndex-1)*10).Take(10).ToList();
            int count = ent.View_Students.Where(bol).Count();
            int page = 0;
            if (count % 10 == 0)
                page = count / 10;
            else
                page = count / 10 + 1;
            var viewList = new {
                rows = list,
                pageCount=page
            };
            return Json(viewList);
        }
        /// <summary>
        /// 根据实训班级查找学生
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ActionResult GeCIDStudents(int cID)
        {
            var list = ent.View_Students.Where(m=>m.TC_ID== cID).ToList();                
            return Json(list);
        }
        //换班或者分班
        public ActionResult addOrUpdateClass(List<int> check, int tID,bool isUpdateClass)
        {
           
                try
                {
                    ent.Configuration.LazyLoadingEnabled = false;
                    var cls = ent.TrainClasses.FirstOrDefault(m => m.TC_ID == tID);
                    var stu = ent.TrainClassStudents.Where(m => m.TC_ID == tID).ToList();
                if (isUpdateClass)
                    {
                        if(stu.Count< cls.TC_MaxAmount)
                        {
                            int stuID = check[0];
                            var updateList = ent.TrainClassStudents.FirstOrDefault(m => m.Student_ID == stuID);
                            updateList.TC_ID = tID;
                            UpdateModel(updateList);
                            ent.SaveChanges();
                            return Content("换班成功");
                         }
                        else
                        {
                            return Content("该班人数已满，请另选其他班");
                        }                                                                 
                    }
                    else
                    {
                                                                  
                        List<Models.TrainClassStudents> addLists = new List<Models.TrainClassStudents>();
                        for (int t = 0; t < check.Count(); t++)
                        {
                            Models.TrainClassStudents list = new Models.TrainClassStudents();
                            list.Student_ID = check[t];
                            list.TC_ID = tID;
                            addLists.Add(list);
                        }

                        if((stu.Count+ addLists.Count)> cls.TC_MaxAmount)
                         {
                             int num = cls.TC_MaxAmount - stu.Count;
                        return Content("该班人数过多，最多在编入"+ num+"人");
                         }
                        else
                         {
                            ent.TrainClassStudents.AddRange(addLists);
                            ent.SaveChanges();
                            return Content("完成");
                         }                                            
                    }
                }
                catch(Exception ex)
                {
                   
                    return Content(ex.Message.ToString());
                }
              
            }
               
        //导出学生数据视图
        public ActionResult ExportClassStudent()
        {

            return View();
        }





    }
}