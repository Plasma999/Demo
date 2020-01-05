using APIDemo.App;
using APIDemo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace APIDemo.Controllers
{
    public class StudentProfilesController : ApiController
    {
        private StudentDB db = new StudentDB();
        private string connStr = Util.getconnectionString(Const.connID);
        private ILogger myLogger = new EventLogger();

        // GET: api/StudentProfiles  查詢全部
        public IHttpActionResult GetStudentProfile()
        {
            var data = (IQueryable<StudentProfile>)db.StudentProfile;
            int recordSize = data.Count();
            data = data.Take(DbUtil.searchSize);
            string msg = DbUtil.getDbCountMsg(recordSize);
            return Ok(new { msg, data });
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

        // GET api/StudentProfiles?Coupon={Coupon}
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult GetStudentProfile(string Coupon)
        {
            Coupon = Coupon.ReplaceNull();
            int count = db.StudentProfile.Count(s => s.Coupon == Coupon);
            return Ok(count);
        }

        // GET api/StudentProfiles?columnName={columnName}
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult GetStudentProfileSummary(string columnName)
        {
            var ds = db.StudentProfile_summary(columnName).ToList().AsQueryable();
            return Ok(ds);
        }

        // GET: api/StudentProfiles/{Id}?Name={Name}&Gender={Gender}&Blood={Blood}&Height={Height}&Weight={Weight}&Coupon={Coupon}
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult GetStudentProfile(string Id, string Name, string Gender, string Blood, string Height, string Weight, string Coupon)
        {
            var obj = new ServerSideProcessingModel();
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["start"]))
            {
                obj = new ServerSideProcessingModel()
                {
                    Start = int.Parse(HttpContext.Current.Request.QueryString["start"]),
                    Length = int.Parse(HttpContext.Current.Request.QueryString["length"]),
                    OrderIndex = int.Parse(HttpContext.Current.Request.QueryString["order[0][column]"]),
                    OrderByAsc = HttpContext.Current.Request.QueryString["order[0][dir]"]
                };
                obj.OrderBy = obj.ConvertOrderBy(obj.OrderIndex);
            }

            string errMsg = "";
            string idOperator = "";
            string idValue = "";
            string nameOperator = "";
            string nameValue = "";
            string couponOperator = "";
            string couponValue = "";
            string heightOperator = "";
            decimal heightValue = 0;
            decimal heightValue2 = 0;
            string weightOperator = "";
            decimal weightValue = 0;
            decimal weightValue2 = 0;
            string genderOperator = "";
            string genderValue = "";
            string bloodOperator = "";
            string bloodValue = "";

            if (!ParseSyntax(Id, InputType.String, ref idOperator, ref idValue, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Name, InputType.String, ref nameOperator, ref nameValue, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Coupon, InputType.String, ref couponOperator, ref couponValue, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Height, InputType.Decimal, ref heightOperator, ref heightValue, ref heightValue2, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Weight, InputType.Decimal, ref weightOperator, ref weightValue, ref weightValue2, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Gender, InputType.String, ref genderOperator, ref genderValue, ref errMsg))
            {
                return BadRequest(errMsg);
            }
            if (!ParseSyntax(Blood, InputType.String, ref bloodOperator, ref bloodValue, ref errMsg))
            {
                return BadRequest(errMsg);
            }

            string sql = @"exec StudentProfile_Sel @Id_operator = {0}, @Id_value = {1}, @Name_operator = {2}, @Name_value = {3}, @Coupon_operator = {4}, @Coupon_value = {5}, 
                @Height_operator = {6}, @Height_value = {7}, @Height_value2 = {8}, @Weight_operator = {9}, @Weight_value = {10}, @Weight_value2 = {11}, @Gender_value = {12}, @Blood_value = {13},
                @Start = {14}, @Length = {15}, @OrderBy = {16}, @OrderByAsc = {17}";
            string[] paramValue =
            {
                idOperator, idValue, nameOperator, nameValue, couponOperator, couponValue, heightOperator,
                heightValue.ToString(CultureInfo.InvariantCulture), heightValue2.ToString(CultureInfo.InvariantCulture),
                weightOperator, weightValue.ToString(CultureInfo.InvariantCulture),
                weightValue2.ToString(CultureInfo.InvariantCulture), genderValue, bloodValue, obj.Start.ToString(),
                obj.Length.ToString(), obj.OrderBy, obj.OrderByAsc
            };
            DataSet dataSet = DbUtil.ExecuteSql(sql, paramValue, connStr);

            if (dataSet == null || dataSet.Tables.Count < 2)
            {
                return NotFound();
            }

            int recordSize = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
            string msg = DbUtil.getDbCountMsg(recordSize);
            DataTable data = dataSet.Tables[1];
            int recordsTotal = recordSize;
            int recordsFiltered = recordSize;
            return Ok(new { msg, recordsTotal, recordsFiltered, data });
        }

        private bool ParseSyntax(string syntax, InputType inputType, ref string xxxOperator, ref decimal xxxValue, ref decimal xxxValue2, ref string errMsg)
        {
            string temp = "";
            bool result = ParseSyntax(syntax, inputType, ref xxxOperator, ref temp, ref errMsg);

            if (result)
            {
                if (inputType == InputType.Decimal && xxxOperator == new Operator_between().Name)  //遇到between，要將原本的xxx.x,xxx.x，解析成兩個數值
                {
                    string[] decimalArray = temp.Split(',');
                    if (decimalArray.Length != 2)
                    {
                        errMsg = "invalid decimal format! " + temp;
                        return false;
                    }
                    xxxValue = decimal.Parse(decimalArray[0]);
                    xxxValue2 = decimal.Parse(decimalArray[1]);
                }
                else if (!string.IsNullOrEmpty(temp))
                {
                    xxxValue = decimal.Parse(temp);
                }
            }

            return result;
        }

        private bool ParseSyntax(string syntax, InputType inputType, ref string xxxOperator, ref string xxxValue, ref string errMsg)
        {
            if (syntax == "noNeed")
            {
                return true;
            }

            string[] array = syntax.Split('|');
            if (array.Length != 2)
            {
                errMsg = "invalid input! " + syntax;
                return false;
            }

            if (!checkOperator(array[0], inputType))
            {
                errMsg = "invalid operator: " + array[0];
                return false;
            }
            else
            {
                array[0] = convertOperator(array[0]);
                if (inputType == InputType.String && array[0] == new Operator_like().Name)  //遇到like，要做%轉換
                {
                    array[1] = array[1].Replace(Const.percentage, "%");
                }
                if (inputType == InputType.String && array[0] == new Operator_in().Name)  //遇到in，要變成xxx','xxx','xxx
                {
                    array[1] = array[1].Replace(",", "','");
                }

                xxxOperator = array[0];
                xxxValue = array[1];
                return true;
            }
        }

        private string convertOperator(string idOperator)
        {
            var operators = new List<Operator>();
            operators.Add(new Operator_equal());
            operators.Add(new Operator_like());
            operators.Add(new Operator_in());
            operators.Add(new Operator_moreThan());
            operators.Add(new Operator_moreThanOrEqual());
            operators.Add(new Operator_lessThan());
            operators.Add(new Operator_lessThanOrEqual());
            operators.Add(new Operator_between());

            foreach (var o in operators)
            {
                if (idOperator == o.Name)
                {
                    return o.Value;
                }
            }

            return idOperator;
        }

        private enum InputType
        {
            String,
            Decimal
        }

        private bool checkOperator(string value, InputType inputType)
        {
            var operators = new List<Operator>();

            switch (inputType)
            {
                case InputType.String:
                    operators.Add(new Operator_equal());
                    operators.Add(new Operator_like());
                    operators.Add(new Operator_in());
                    break;
                case InputType.Decimal:
                    operators.Add(new Operator_equal());
                    operators.Add(new Operator_moreThan());
                    operators.Add(new Operator_moreThanOrEqual());
                    operators.Add(new Operator_lessThan());
                    operators.Add(new Operator_lessThanOrEqual());
                    operators.Add(new Operator_between());
                    break;
            }

            foreach (var o in operators)
            {
                if (value == o.Name)
                {
                    return true;
                }
            }

            return false;
        }

        // POST: api/StudentProfiles  新增一筆
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult PostStudentProfile(StudentProfile studentProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (studentProfile.Height == null)
            {
                studentProfile.Height = 0;
            }
            if (studentProfile.Weight == null)
            {
                studentProfile.Weight = 0;
            }
            if (studentProfile.Id != null)
            {
                studentProfile.Id = studentProfile.Id.ToUpper();
            }

            string errMsg = "";
            var studentProfileValidator = new StudentProfileValidator(studentProfile);
            if (!studentProfileValidator.ValidateAll(ref errMsg))
            {
                return BadRequest(errMsg);
            }

            studentProfile.CreateDate = DateTime.Now;
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
            DateTime timeStart = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentProfileList = new List<StudentProfile>();

            var randomId = new RandomID();
            List<string> randomIDs = randomId.getRandomIDs(num);

            DateTime timeIdDone = DateTime.Now;
            result += "generate ID costs " + Util.getSecond(timeStart, timeIdDone) + " sec, ";

            var randomName = new RandomName(new Random());
            List<string> randomNames = randomName.RandomNames(num, 2);

            DateTime timeNameDone = DateTime.Now;
            result += "generate Name costs " + Util.getSecond(timeIdDone, timeNameDone) + " sec, ";

            List<string> randomGenders = Util.getRandomList(Const.Gender, num);
            List<string> randomBloods = Util.getRandomList(Const.Blood, num);
            List<decimal> randomHeights = Util.getRandomDecimalList(150, 200, 1, num);
            List<decimal> randomWeights = Util.getRandomDecimalList(40, 100, 1, num);

            DateTime timeOtherDone = DateTime.Now;
            result += "generate other columns costs " + Util.getSecond(timeNameDone, timeOtherDone) + " sec, ";

            for (int i = 0; i < num; i++)
            {
                var item = new StudentProfile();
                item.guid = Guid.NewGuid();
                item.Id = randomIDs[i];
                item.Name = randomNames[i];
                item.Gender = randomGenders[i];
                item.Blood = randomBloods[i];
                item.Height = randomHeights[i];
                item.Weight = randomWeights[i];
                item.CreateDate = DateTime.Now;
                studentProfileList.Add(item);
            }

            randomIDs.Clear();
            randomIDs.TrimExcess();
            randomNames.Clear();
            randomNames.TrimExcess();
            randomGenders.Clear();
            randomGenders.TrimExcess();
            randomBloods.Clear();
            randomBloods.TrimExcess();
            randomHeights.Clear();
            randomHeights.TrimExcess();
            randomWeights.Clear();
            randomWeights.TrimExcess();
            GC.Collect();

            DateTime timeListDone = DateTime.Now;
            result += "put into List costs " + Util.getSecond(timeOtherDone, timeListDone) + " sec, ";

            bool dbResult;
            switch (type)
            {
                case "ADO_For":
                    dbResult = MassInsert_ADO_For(studentProfileList);
                    break;
                case "EF_AddRange":
                    dbResult = MassInsert_EF_AddRange(studentProfileList);
                    break;
                case "SqlBulkCopy":
                    dbResult = MassInsert_SqlBulkCopy(studentProfileList);
                    break;
                case "TVP":
                    dbResult = MassInsert_TVP(studentProfileList);
                    break;
                default:
                    return BadRequest("invalid type: " + type);
            }

            studentProfileList.Clear();
            studentProfileList.TrimExcess();
            GC.Collect();

            if (!dbResult)
            {
                result += "save DB has error, see detail in EventLog";
                return Ok(new { result });
            }

            DateTime timeDbDone = DateTime.Now;
            result += "save to DB costs " + Util.getSecond(timeListDone, timeDbDone) + " sec, ";

            DateTime timeEnd = DateTime.Now;
            result += "total costs " + Util.getSecond(timeStart, timeEnd) + " sec.";

            return Ok(new { result });
        }

        private bool MassInsert_ADO_For(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            var sc = new SqlConnection();

            try
            {
                sc = new SqlConnection(connStr);
                string sql = @"insert into StudentProfile (guid, Id, Name, Gender, Blood, Height, Weight)
                    values ({0}, {1}, {2}, {3}, {4}, {5}, {6})";

                foreach (var studentProfile in studentProfileList)
                {
                    string[] param = new string[] { studentProfile.guid.ToString(), studentProfile.Id, studentProfile.Name, studentProfile.Gender, studentProfile.Blood,
                        studentProfile.Height.ToString(), studentProfile.Weight.ToString() };
                    DbUtil.ExecuteSqlNoReturn(sql, param, sc);
                }

                result = true;
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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
                var splitList = Util.splitList(studentProfileList, DbUtil.maxRowSize);
                foreach (var list in splitList)
                {
                    if (SetDataTable(list, ref columnNames, ref dt))
                    {
                        result = DbUtil.MySqlBulkCopy("StudentProfile", columnNames, dt, connStr);
                    }
                }
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
            }

            return result;
        }

        private bool SetDataTable(List<StudentProfile> studentProfileList, ref string[] columnNames, ref DataTable dt)
        {
            bool result = false;

            try
            {
                //欄位名稱與型態
                columnNames = new[] { "guid", "Id", "Name", "Gender", "Blood", "Height", "Weight", "Coupon", "CreateDate", "UpdateDate" };
                Type[] columnTypes = new Type[] { typeof(Guid), typeof(string), typeof(string), typeof(string), typeof(string), typeof(decimal), typeof(decimal), typeof(string), typeof(DateTime), typeof(DateTime) };

                dt = DbUtil.setDataColumn(columnNames, columnTypes);

                //DataTable的資料列
                for (int i = 0; i < studentProfileList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["guid"] = studentProfileList[i].guid;
                    dr["Id"] = studentProfileList[i].Id;
                    dr["Name"] = studentProfileList[i].Name;
                    dr["Gender"] = studentProfileList[i].Gender;
                    dr["Blood"] = studentProfileList[i].Blood;
                    dr["Height"] = studentProfileList[i].Height == null ? (object)DBNull.Value : studentProfileList[i].Height;
                    dr["Weight"] = studentProfileList[i].Weight == null ? (object)DBNull.Value : studentProfileList[i].Weight;
                    dr["Coupon"] = studentProfileList[i].Coupon;
                    dr["CreateDate"] = studentProfileList[i].CreateDate == null ? (object)DBNull.Value : studentProfileList[i].CreateDate;
                    dr["UpdateDate"] = studentProfileList[i].UpdateDate == null ? (object)DBNull.Value : studentProfileList[i].UpdateDate;
                    dt.Rows.Add(dr);
                }

                result = true;
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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
                var splitList = Util.splitList(studentProfileList, DbUtil.maxRowSize);
                foreach (var list in splitList)
                {
                    if (SetDataTable(list, ref columnNames, ref dt))
                    {
                        string typeName = "TVP_StudentProfile";
                        string sql = "insert into StudentProfile select * FROM @" + typeName;
                        result = DbUtil.TVP_process(sql, typeName, dt, connStr);
                    }
                }
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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

            replaceStudentProfileNull(studentProfile);

            try
            {
                var record = db.StudentProfile.First(s => s.guid == studentProfile.guid);
                record.Id = studentProfile.Id;
                record.Name = studentProfile.Name;
                record.Gender = studentProfile.Gender;
                record.Blood = studentProfile.Blood;
                record.Height = studentProfile.Height;
                record.Weight = studentProfile.Weight;
                record.Coupon = studentProfile.Coupon;
                record.UpdateDate = DateTime.Now;
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

        private void replaceStudentProfileNull(StudentProfile studentProfile)
        {
            studentProfile.Id = studentProfile.Id.ReplaceNull();
            studentProfile.Name = studentProfile.Name.ReplaceNull();
            studentProfile.Gender = studentProfile.Gender.ReplaceNull();
            studentProfile.Blood = studentProfile.Blood.ReplaceNull();
            studentProfile.Coupon = studentProfile.Coupon.ReplaceNull();
        }

        // PUT: api/StudentProfiles?num=N&type=XXX  更新N筆
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult PutStudentProfile(int num, string type)
        {
            string result = "";
            DateTime timeStart = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentProfileList = new List<StudentProfile>();

            var ds = db.StudentProfile.Where(s => s.Coupon == null).OrderBy(s => s.CreateDate).Take(num).Select(s => new { s.guid, s.Id, s.Name }).ToList();

            DateTime timeGuidDone = DateTime.Now;
            result += "get DB target costs " + Util.getSecond(timeStart, timeGuidDone) + " sec, ";

            foreach (var column in ds)
            {
                var item = new StudentProfile();
                item.guid = column.guid;
                item.Coupon = Util.truncateString(Util.getMD5(column.Id + column.Name), 15);
                item.UpdateDate = DateTime.Now;
                studentProfileList.Add(item);
            }

            DateTime timeCouponDone = DateTime.Now;
            result += "generate Coupon and put into List costs " + Util.getSecond(timeGuidDone, timeCouponDone) + " sec, ";

            bool dbResult;
            switch (type)
            {
                case "ADO_For":
                    dbResult = MassUpdate_ADO_For(studentProfileList);
                    break;
                case "EF_For":
                    dbResult = MassUpdate_EF_For(studentProfileList);
                    break;
                case "Z_EF_Ext":
                    dbResult = MassUpdate_Z_EF_Ext(studentProfileList);
                    break;
                case "TVP":
                    dbResult = MassUpdate_TVP(studentProfileList);
                    break;
                default:
                    return BadRequest("invalid type: " + type);
            }

            ds.Clear();
            ds.TrimExcess();
            studentProfileList.Clear();
            studentProfileList.TrimExcess();
            GC.Collect();

            if (!dbResult)
            {
                result += "save DB has error, see detail in EventLog";
                return Ok(new { result });
            }

            DateTime timeDbDone = DateTime.Now;
            result += "save to DB costs " + Util.getSecond(timeCouponDone, timeDbDone) + " sec, ";

            DateTime timeEnd = DateTime.Now;
            result += "total costs " + Util.getSecond(timeStart, timeEnd) + " sec.";

            return Ok(new { result });
        }

        private bool MassUpdate_ADO_For(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            var sc = new SqlConnection();

            try
            {
                sc = new SqlConnection(connStr);
                string sql = @"update StudentProfile 
                    set Coupon = {0}, UpdateDate = getdate()
                    where guid = {1}";

                foreach (var studentProfile in studentProfileList)
                {
                    string[] param = new string[] { studentProfile.Coupon, studentProfile.guid.ToString() };
                    DbUtil.ExecuteSqlNoReturn(sql, param, sc);
                }

                result = true;
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
            }
            finally
            {
                sc.Dispose();
            }

            return result;
        }

        private bool MassUpdate_EF_For(List<StudentProfile> studentProfileList)
        {
            bool result = false;

            try
            {
                foreach (var studentProfile in studentProfileList)
                {
                    var record = db.StudentProfile.Find(studentProfile.guid);
                    if (record != null)
                    {
                        record.Coupon = studentProfile.Coupon;
                        record.UpdateDate = studentProfile.UpdateDate;
                    }
                }

                db.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
            }

            return result;
        }

        private bool MassUpdate_Z_EF_Ext(List<StudentProfile> studentProfileList)
        {
            bool result = false;

            try
            {
                db.BulkUpdate(studentProfileList, options => options.ColumnInputExpression = c => new { c.guid, c.Coupon, c.UpdateDate });
                result = true;
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
            }

            return result;
        }

        private bool MassUpdate_TVP(List<StudentProfile> studentProfileList)
        {
            bool result = false;
            string[] columnNames = new string[] { };
            var dt = new DataTable();

            try
            {
                var splitList = Util.splitList(studentProfileList, DbUtil.maxRowSize);
                foreach (var list in splitList)
                {
                    if (SetDataTable(list, ref columnNames, ref dt))
                    {
                        string typeName = "TVP_StudentProfile";
                        string sql = @"update StudentProfile 
                        set Coupon = t.Coupon, UpdateDate = getdate()
                        from StudentProfile s
                        join @" + typeName + " t on s.guid = t.guid";
                        result = DbUtil.TVP_process(sql, typeName, dt, connStr);
                    }
                }
            }
            catch (Exception e)
            {
                myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
            }

            return result;
        }

        // DELETE: api/StudentProfiles?guid=XXX
        [ResponseType(typeof(StudentProfile))]
        public IHttpActionResult DeleteStudentProfile(string guid)
        {
            var studentProfile = db.StudentProfile.Where(x => x.guid.ToString() == guid);

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
                    myLogger.Log(Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()));
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