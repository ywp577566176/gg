using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 客户管理系统.Controllers
{
    public class StudentsController : Controller
    {
        Models.CRMEntities db = new Models.CRMEntities();
        // GET: Students
        /// <summary>
        /// 进入导入学生信息界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImportExcel(HttpPostedFileBase file, int? Specialty_ID)
        {
            //int rowNum = sheet.PhysicalNumberOfRows;
            ////遍历excel中的每一样 从第4行 开始 因为前三行是标题
            //for (int t = 3; t < rowNum; t++)
            //{

            //    //根据行号获取当前行
            //    HSSFRow row = sheet.GetRow(t) as HSSFRow;
            //    //得到行中的列数
            //    int cellCount = row.PhysicalNumberOfCells;
            //    //创建一个新的DataTable行
            //    DataRow dtrow = dt.NewRow();
            //    //遍历excel中当前行的每一列 从第三列开始 因为前面几列数据库没有
            //    for (int j = 2; j < cellCount; j++)
            //    {
            //        //获取当前列
            //        HSSFCell cell = row.GetCell(j) as HSSFCell;
            //        //根据单元格中的数据类型给表格中的列赋值
            //        if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
            //        {
            //            dtrow[j + 1] = cell.NumericCellValue;
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(cell.StringCellValue))
            //                dtrow[j + 1] = cell.StringCellValue;
            //        }
            //    }
            //    //将创建好的新行添加到DataTable
            //    dt.Rows.Add(dtrow);
            try
            {
                //获取上传文件的后缀
                string excelLastName = Path.GetExtension(file.FileName);
                //生成文件名称
                string path = "~/Excel/" + DateTime.Now.ToString("yyyyMMddhhmmss") + excelLastName;
                //获取文件物理路径
                string filePath = Server.MapPath(path);
                //将上传的文件保存
                file.SaveAs(filePath);
                //将文件转化成数据流
                FileStream inputStream = new FileStream(filePath, FileMode.Open);
                //将数据流写入hss工具类
                HSSFWorkbook workBook = new HSSFWorkbook(inputStream);
                //得到excel文件中的第一个Sheet工作表
                HSSFSheet sheet = workBook.GetSheetAt(0) as HSSFSheet;
                int rowNum = sheet.PhysicalNumberOfRows;
                List<Models.Students> stuList = new List<Models.Students>();
                //dt.Columns("")
                for (int t = 4; t < rowNum; t++)
                {
                    Models.Students stu = new Models.Students();
                    //根据行号获取当前值
                    HSSFRow row = sheet.GetRow(t) as HSSFRow;
                    //获取行中列数
                    int cellNum = row.PhysicalNumberOfCells;
                    stu.Student_NO = row.GetCell(1).ToString();//学号
                    stu.Student_Name = row.GetCell(2).ToString();//姓名
                    stu.Student_NameSpell = row.GetCell(3).ToString();//姓名拼音
                    stu.Student_Sex = row.GetCell(4).ToString();//性别
                    stu.Student_IdentityNumber = row.GetCell(5).ToString();//身份证号
                                                                           //stu.SC_ID = Convert.ToInt32(row.GetCell(6).ToString());//班级编号
                    stu.Student_State = row.GetCell(7).ToString();//学生状态
                    stu.Student_Exam = row.GetCell(8).ToString();//已通过认证
                    stu.Student_Education = row.GetCell(9).ToString();//学历
                    stu.Student_Specialty = row.GetCell(10).ToString();//专业
                    stu.Student_Schoolofgraduation = row.GetCell(11).ToString();//毕业学校
                    stu.Student_PersonalTel = row.GetCell(12).ToString();//个人电话
                    stu.Student_FamilyTel = row.GetCell(13).ToString();//家庭电话
                    stu.Student_QQ = row.GetCell(14).ToString();//QQ号码
                    stu.Student_Address = row.GetCell(15).ToString();//通讯地址
                    stu.Student_PostCode = row.GetCell(16).ToString();//邮编
                    stu.Student_EducationMoney = row.GetCell(17).ToString();//学历费
                    stu.Student_SkillTrainingMoney = row.GetCell(18).ToString();//技能培养费
                    stu.Student_TrainResideMoney = row.GetCell(19).ToString();//实训住宿费
                    stu.Student_Evaluate1 = row.GetCell(20).ToString();//技术评价
                    stu.Student_Evaluate2 = row.GetCell(21).ToString();//班主任评价
                    stu.Student_Remark = row.GetCell(22).ToString();//备注
                    stu.Stationt_SelectStationCount = 0;
                    stuList.Add(stu);

                }

                db.Students.AddRange(stuList);
                db.SaveChanges();
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content(ex.Message.ToString());
            }
         

            

        }
    }
}