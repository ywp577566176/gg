using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;


namespace 客户管理系统.Controllers
{
    public class SchoolsStationsController : Controller
    {
        // GET: SchoolsStations
        Models.CRMEntities ent = new Models.CRMEntities();
        //给校区设置岗位
        /// <summary>
        /// 转到设置视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取校区数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSchools(string sCode,string sName,int pageIndex)
        {
            ent.Configuration.LazyLoadingEnabled = false;
            Func<Models.Schools, bool> bol = m => m.School_Name.Contains(sName) && m.School_Code.Contains(sCode);
            var list = ent.Schools.Where(bol).Skip((pageIndex-1)*5).Take(5).ToList();
            for(int t=0;t< list.Count;t++)
            {
                if (string.IsNullOrEmpty(list[t].School_Remark))
                    list[t].School_Remark = "无";
            }
            int count = ent.Schools.Where(bol).Count();

            int pageCount = 0;
            if (count % 5 == 0)
            {
                pageCount = count / 5;
            }
            else
            {
                pageCount = count / 5 + 1;
            }
            var info = new
            {
                rows = list,
                total = pageCount
            };
            
           
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取专业数据
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetSpecialties(int sID)
        {
            ent.Configuration.LazyLoadingEnabled = false;
           
            //select* from  (select distinct(Specialty_ID) from SchoolClasses  where School_ID = 2)t left join Specialties on t.Specialty_ID = Specialties.Specialty_ID
            var result = from spyID in (
                         from spy in ent.SchoolClasses
                         where spy.School_ID == sID
                         group spy by spy.Specialty_ID into spyID
                         select new
                         {
                             Specialty_ID = spyID.Key
                         })
                         join spe in ent.Specialties
                         on spyID.Specialty_ID equals spe.Specialty_ID
                         into Specialty
                         from Specialties in Specialty.DefaultIfEmpty()
                         select new
                         {                            
                             Specialty_ID = Specialties.Specialty_ID,
                             Specialty_Name = Specialties.Specialty_Name,
                             Specialty_Description = Specialties.Specialty_Description
                         };
                       
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取岗位数据
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetStations(int? type,int? styID, int? sID )
        {
            ent.Configuration.LazyLoadingEnabled = false;
            var stList = ent.Stations.ToList();
            var SpecialtyStation = ent.SpecialtyStations.Where(m => m.School_ID == sID && m.Specialty_ID == styID);
            var list = from son in ent.Stations
                       join styston in SpecialtyStation
                       on son.Station_ID equals styston.Station_ID
                       into Station
                       from spe in Station.DefaultIfEmpty()
                       select new
                       {
                           Station_ID = son.Station_ID,
                           Station_Name = son.Station_Name,
                           Station_NeedTechnique = son.Station_NeedTechnique,
                           Station_Duty = son.Station_Duty,
                           sID = spe.Station_ID

                       };

            if (type == 1)
                list = list.Where(m => m.sID != null);
            else if(type == 2)
                list = list.Where(m => m.sID == null);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveStations(string jsonList, int sID, int styID )
        {
            using (var tran = ent.Database.BeginTransaction())
            {                                        
                try
                {
                    var delList = ent.SpecialtyStations.Where(m => m.School_ID == sID && m.Specialty_ID == styID);
                    ent.SpecialtyStations.RemoveRange(delList);
                    ent.SaveChanges();
                    if (jsonList != "[0]")
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Models.SpecialtyStations>));
                        MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonList));
                        List<Models.SpecialtyStations> List = (List<Models.SpecialtyStations>)ser.ReadObject(ms);
                        ent.SpecialtyStations.AddRange(List);
                        ent.SaveChanges();                     
                    }                  
                     tran.Commit();
                    return Content("1");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return Content("-1");
                }

            }
            
              
        }



    }
}