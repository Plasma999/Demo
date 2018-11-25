using APIDemo.Models;
using System;
using System.Reflection;

namespace APIDemo.App_Code
{
    public class StudentProfileInsert : ITransaction
    {
        public string ErrMsg { get; set; }

        public bool Execute()
        {
            bool result = false;

            try
            {
                var db = new StudentDB();
                var studentProfile = new StudentProfile() { Id = "A111222333", Name = "test Trans", CreateDate = DateTime.Now };
                db.StudentProfile.Add(studentProfile);
                db.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                ErrMsg = Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.InnerException.InnerException.Message);
            }

            return result;
        }
    }

    public class StudentProfileInsert2 : ITransaction
    {
        public string ErrMsg { get; set; }

        public bool Execute()
        {
            bool result = false;

            try
            {
                var db = new StudentDB();
                var studentProfile = new StudentProfile() { Id = "A111222333", Name = "test Trans2", CreateDate = DateTime.Now };
                db.StudentProfile.Add(studentProfile);
                db.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                ErrMsg = Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.InnerException.InnerException.Message);
            }

            return result;
        }
    }

    public class StudentProfileInsert3 : ITransaction
    {
        public string ErrMsg { get; set; }

        public bool Execute()
        {
            bool result = false;

            try
            {
                var db = new StudentDB();
                var studentProfile = new StudentProfile() { Id = "A111222333", Name = "test Trans", CreateDate = DateTime.Now };
                var studentProfile2 = new StudentProfile() { Id = "A111222333", Name = "test Trans2", CreateDate = DateTime.Now };
                db.StudentProfile.Add(studentProfile);
                db.StudentProfile.Add(studentProfile2);
                db.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                ErrMsg = Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.InnerException.InnerException.Message);
            }

            return result;
        }
    }
}