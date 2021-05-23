using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atest.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Atest.Controllers
{
    public class HomeController : Controller
    {
        [SessionAuthorize]
        public ActionResult Index()
        {
            dataEntities db = new dataEntities();
            var query = db.DBtestUsers.ToList();
            return View(query);
        }

        public ActionResult Login()
        {
            return View();

        }


        [HttpPost]
        public async Task<ActionResult> Login(DBtestUser objEntity)
        {
            Session["IS_ADMIN"] = null;
            Session["UId"] = 0;
            if (ModelState.IsValid)
            {
                objEntity.FirstName = objEntity.FirstName.Trim();
                objEntity.Password = objEntity.Password.Trim();

                var login = await DataBaseUtil.validateLogIn(objEntity.FirstName, objEntity.Password);
                if (login.Equals("valid"))
                {
                    Session["IS_ADMIN"] = objEntity.FirstName;
                    //var appt = await DataBaseUtil.GetUser(objEntity.Id);

                    //if (appt.Designation == "Admin")
                    //{
                    //    Session["IS_ADMIN"] = "true";
                    //}
                    //else
                    //{
                    //    Session["IS_ADMIN"] = "false";
                    //}

                    //Session["UId"] = appt.Id;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewData["message"] = "Invalid User ID/Password";
            }
            return View(objEntity);
        }

        //[HttpPost]
        //public ActionResult Login(DBtestUser log)
        //{
        //    log.FirstName = log.FirstName.Trim();
        //    log.Password = log.Password.Trim();
        //    dataEntities logDB = new dataEntities();
        //    var user = logDB.DBtestUsers.Where(x => x.FirstName == log.FirstName && x.Password == log.Password).Count();
        //    if (user > 0)
        //    {
        //        return RedirectToAction("Home");
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}


        [HttpGet]
        public ActionResult AddSet(int id = 0)
        {
            if (id == 0)
            {
                return View(new DBtestUser());
            }
            else
            {
                using (dataEntities db = new dataEntities())
                {
                    return View(db.DBtestUsers.Where(x => x.Id == id).FirstOrDefault<DBtestUser>());
                }
            }
        }
        [SessionAuthorize]
        [HttpPost]
        public ActionResult AddSet(DBtestUser emp)
        {

            using (dataEntities db = new dataEntities())
            {
                if (emp.Id == 0)
                {
                    /*
                    string fileName = Path.GetFileNameWithoutExtension(emp.Ifile.FileName);
                    string extention = Path.GetExtension(emp.Ifile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extention;
                    emp.UAvatar = fileName;
                    emp.Ifile.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images"), fileName));*/
                    db.DBtestUsers.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Update Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [SessionAuthorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (dataEntities db = new dataEntities())
            {
                DBtestUser emp = db.DBtestUsers.Where(x => x.Id == id).FirstOrDefault<DBtestUser>();
                db.DBtestUsers.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Data Successfully Deleted" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}