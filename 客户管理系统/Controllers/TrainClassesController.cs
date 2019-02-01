using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class TrainClassesController : Controller
    {
        // GET: TrainClasses
        Models.CRMEntities ent = new Models.CRMEntities();
        /// <summary>
        /// 查询班级表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTrainClasses(string tName, string teacher, string pageIndex)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            if (pageIndex == null)
                return View();
            else
            {
                int page = int.Parse(pageIndex);
                Func<Models.View_TrainClasses, bool> bol = m =>
                {
                    if (teacher!="==请选择==")
                        return m.TC_Name.Contains(tName) && (m.Teacher == teacher || m.Councilor == teacher);
                    else
                        return m.TC_Name.Contains(tName);
                };
                var list = ent.View_TrainClasses.Where(bol).Skip((page - 1) * 5).Take(5).ToList();
                for (int t = 0; t < list.Count; t++)
                {
                    if (list[t].Teacher == null)
                        list[t].Teacher = "";
                    if (list[t].Councilor == null)
                        list[t].Councilor = "";
                    if(list[t].TC_MaleAmount==null)
                        list[t].TC_MaleAmount = 0;

                    if (list[t].TC_FemaleAmount == null)
                        list[t].TC_FemaleAmount = 0;
                }
                int count = ent.View_TrainClasses.Where(bol).Count();
                int pageCount = 0;

                if (count % 5 == 0)
                    pageCount = count / 5;
                else
                    pageCount = count / 5 + 1;
                var sLIst = new
                {
                    rows = list,
                    total = pageCount
                };
                return Json(sLIst, JsonRequestBehavior.AllowGet);
            }



        }
        /// <summary>
        /// 删除班级
        /// </summary>
        /// <returns></returns>
        public ActionResult Del(int? tID)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {

                    var tcInfo = ent.TrainClasses.FirstOrDefault(m => m.TC_ID == tID);
                    ent.TrainClasses.Remove(tcInfo);
                    ent.SaveChanges();

                    var tctInfo = ent.TrainClassTeachers.Where(m => m.TC_ID == tID);
                    if (tctInfo != null)
                    {
                        ent.TrainClassTeachers.RemoveRange(tctInfo);
                        ent.SaveChanges();
                    }
                    tran.Commit();
                    return Content("true");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return Content(ex.Message.ToString());
                }
            }


        }
        public ActionResult AddOrUpdate(Models.TrainClasses tcInfo,int? Teacher,int? Councilor)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    if(tcInfo.TC_ID==0)
                    //新增班级
                    {
                        ent.TrainClasses.Add(tcInfo);
                        ent.SaveChanges();
                        tran.Commit();
                        return Content("add");
                    }
                    else
                    //修改班级
                    {
                        UpdateModel(tcInfo);
                        ent.SaveChanges();

                        var tctList = ent.TrainClassTeachers.Where(m => m.TC_ID == tcInfo.TC_ID);
                        if(tctList!=null)
                        {
                            ent.TrainClassTeachers.RemoveRange(tctList);
                            ent.SaveChanges();
                        }

                        List<Models.TrainClassTeachers> newTCTInfo = new List<Models.TrainClassTeachers>();
                        Models.TrainClassTeachers info1 = new Models.TrainClassTeachers();
                        info1.TC_ID = tcInfo.TC_ID;
                        info1.T_ID = Teacher;
                        newTCTInfo.Add(info1);
                        Models.TrainClassTeachers info2 = new Models.TrainClassTeachers();
                        info2.TC_ID = tcInfo.TC_ID;
                        info2.T_ID = Councilor;
                        newTCTInfo.Add(info2);

                        ent.TrainClassTeachers.AddRange(newTCTInfo);
                        ent.SaveChanges();

                        tran.Commit();
                        return Content("update");
                    }
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return Content(ex.Message.ToString());
                }
            }
       }
        /// <summary>
        /// 根据id获取班级视图表中的数据
        /// </summary>
        /// <param name="tID"></param>
        /// <returns></returns>
        public ActionResult GetVTrainClasses(int tID)
        {
            var vtcList = ent.View_TrainClasses.FirstOrDefault(m => m.TC_ID == tID);
            var teacher = ent.Teachers.FirstOrDefault(m => m.T_Name == vtcList.Teacher);
            string teacherID ="";
           
            if (teacher!=null)
                teacherID= teacher.T_ID.ToString();
            string councilorID = "";
            var councilor = ent.Teachers.FirstOrDefault(m => m.T_Name == vtcList.Councilor);
            if (councilor != null)
                councilorID = councilor.T_ID.ToString();
            var sList = new
            {
                rows = vtcList,
                tID = teacherID,               
                cID = councilorID
            };
            return Json(sList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取班级信息用来绑定下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTrainClassesDDL()
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var list = ent.TrainClasses.ToList();
            return Json(list);
            string a = "";
        }



    }
}