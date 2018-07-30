using APIDemo.App_Code;
using APIDemo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace APIDemo.Controllers
{
    public class StudentProfilesController : ApiController
    {
        private StudentDB db = new StudentDB();
        private string connStr = Util.getconnectionString(Const.connID);

        // GET: api/StudentProfiles  查詢全部
        public IHttpActionResult GetStudentProfile()
        {
            var ds = (IQueryable<StudentProfile>)db.StudentProfile;
            string msg = getDbCountMsg(ref ds);
            return Ok(new { msg, ds });
        }

        private string getDbCountMsg(ref IQueryable<StudentProfile> ds)
        {
            int maxSize = 1000;
            int recordSize = ds.Count();
            ds = ds.Take(maxSize);
            return recordSize > maxSize ? "Data has " + recordSize + " records, only show top " + maxSize + " results." : "Data has " + recordSize + " records.";
        }

        // GET: api/StudentProfiles?guid=XXX
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult GetStudentProfile(Guid guid)
        {
            var ds = db.StudentProfile.Find(guid);
            if (ds == null)
            {
                return NotFound();
            }

            return Ok(ds);
        }

        // GET: api/StudentProfiles/XXX
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult GetStudentProfile(string Id)
        {

            var ds = Id == "null" ? db.StudentProfile.Where(x => x.Id.Equals(null)) : db.StudentProfile.Where(x => x.Id == Id);
            if (ds == null)
            {
                return NotFound();
            }

            string msg = getDbCountMsg(ref ds);
            return Ok(new { msg, ds });
        }

        // POST: api/StudentProfiles  新增一筆
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult PostStudentProfile(StudentProfile studentProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StudentProfile.Add(studentProfile);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudentProfileExists(studentProfile.guid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = studentProfile.guid }, studentProfile);
        }

        // POST: api/StudentProfiles?num=N&type=XXX  新增N筆
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult PostStudentProfile(int num, string type)
        {
            string result = "";
            DateTime time_start = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentProfileList = new List<StudentProfile>();
            var randomName = new RandomName(new Random());
            List<string> randomNames = randomName.RandomNames(num, 2, null, null);

            DateTime time_name_done = DateTime.Now;
            result += "generate Name costs " + Util.getSecond(time_start, time_name_done) + " sec, ";

            for (int i = 0; i < num; i++)
            {
                var item = new StudentProfile();
                item.guid = Guid.NewGuid();
                item.Name = randomNames[i];
                studentProfileList.Add(item);
            }

            DateTime time_list_done = DateTime.Now;
            result += "put into List costs " + Util.getSecond(time_name_done, time_list_done) + " sec, ";

            bool db_result = false;
            switch (type)
            {
                case "ADO_For":
                    db_result = MassInsert_ADO_For(studentProfileList);
                    break;
                case "EF_AddRange":
                    db_result = MassInsert_EF_AddRange(studentProfileList);
                    break;
                case "SqlBulkCopy":
                    db_result = MassInsert_SqlBulkCopy(studentProfileList);
                    break;
                case "TVP":
                    db_result = MassInsert_TVP(studentProfileList);
                    break;
                default:
                    return BadRequest("invalid type: " + type);
            }

            if (!db_result)
            {
                result += "save DB has error, see detail in EventLog";
                return Ok(new { result });
            }

            DateTime time_db_done = DateTime.Now;
            result += "save to DB costs " + Util.getSecond(time_list_done, time_db_done) + "sec, ";

            DateTime time_end = DateTime.Now;
            result += "total costs " + Util.getSecond(time_start, time_end) + " sec.";

            return Ok(new { result });
        }

        private bool MassInsert_ADO_For(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            var sc = new SqlConnection();

            try
            {
                sc = new SqlConnection(connStr);
                string sql = @"insert into StudentProfile (guid, Name)
                    values ({0}, {1})";

                foreach (var studentProfile in studentProfileList)
                {
                    string[] param = new string[] { studentProfile.guid.ToString(), studentProfile.Id };
                    DbUtil.ExecuteSqlNoReturn(sql, param, sc);
                }

                result = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
            }
            finally
            {
                sc.Dispose();
            }

            return result;
        }

        private bool MassInsert_EF_AddRange(List<StudentProfile> studentProfileList)
        {
            bool result = false;

            try
            {
                db.StudentProfile.AddRange(studentProfileList);
                db.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
            }

            return result;
        }

        private bool MassInsert_SqlBulkCopy(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            string[] columnNames = new string[] { };
            var dt = new DataTable();

            try
            {
                if (setDataTable(studentProfileList, ref columnNames, ref dt))
                {
                    result = DbUtil.MySqlBulkCopy("StudentProfile", columnNames, dt, connStr);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
            }

            return result;
        }

        private bool setDataTable(List<StudentProfile> studentProfileList, ref string[] columnNames, ref DataTable dt)
        {
            bool result = false;

            try
            {
                //欄位名稱與型態
                columnNames = new string[] { "guid", "Id", "Name", "Address", "Email", "Tel", "Message", "Memo" };
                Type[] columnTypes = new Type[] { typeof(Guid), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) };
                if (columnNames.Length != columnTypes.Length)
                {
                    throw new ArgumentException("columnNames and columnTypes are not mapping.");
                }

                //DataTable的欄位
                for (int i = 0; i < columnNames.Length; i++)
                {
                    dt.Columns.Add(columnNames[i], columnTypes[i]);
                }

                //DataTable的資料列
                for (int i = 0; i < studentProfileList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["guid"] = studentProfileList[i].guid;
                    dr["Name"] = studentProfileList[i].Name;
                    dt.Rows.Add(dr);
                }

                result = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
            }

            return result;
        }

        private bool MassInsert_TVP(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            string[] columnNames = new string[] { };
            var dt = new DataTable();

            try
            {
                if (setDataTable(studentProfileList, ref columnNames, ref dt))
                {
                    string typeName = "TVP_StudentProfile";
                    string sql = "insert into StudentProfile select * FROM @" + typeName;
                    result = DbUtil.TVP_insert(sql, typeName, dt, connStr);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
            }

            return result;
        }

        // PUT: api/StudentProfiles  更新一筆
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudentProfile(StudentProfile studentProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(studentProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentProfileExists(studentProfile.guid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //TODO PUT: api/StudentProfiles?num=N&type=XXX  更新N筆
        /* 
         * ADO.NET for
         * EF db.Books.AddOrUpdate(book); //requires using System.Data.Entity.Migrations;
         * SqlDataAdapter update
         * TVP update
         */


        // DELETE: api/StudentProfiles?guid=XXX
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult DeleteStudentProfile(string guid)
        {
            var studentProfile = db.StudentProfile.Where(x => x.guid.ToString() == guid);
            if (studentProfile == null)
            {
                return NotFound();
            }

            db.StudentProfile.RemoveRange(studentProfile);
            db.SaveChanges();

            return Ok(studentProfile);
        }

        // DELETE: api/StudentProfiles?all=XXX
        [ResponseType(typeof(String))]
        public IHttpActionResult DeleteStudentProfileAll(string all)
        {
            if (all == "yes")
            {
                try
                {
                    string sql = @"truncate table StudentProfile";
                    DbUtil.ExecuteSqlNoReturn(sql, new string[] { }, connStr);
                    return Ok("delete ok");
                }
                catch (Exception e)
                {
                    EventLog.WriteEntry(Const.AP_ID, e.ToString(), EventLogEntryType.Error);
                    return BadRequest("delete DB has error, see detail in EventLog");
                }
            }
            else
            {
                return BadRequest("wrong cmd!");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentProfileExists(Guid id)
        {
            return db.StudentProfile.Count(e => e.guid == id) > 0;
        }
    }
}