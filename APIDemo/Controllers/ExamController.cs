using APIDemo.App;
using System.Web.Http;

namespace APIDemo.Controllers
{
    public class ExamController : ApiController
    {
        // GET api/Exam?examNo={examNo}
        public IHttpActionResult GetExam(string examNo)
        {
            string result = "";
            var exam = new Exam();

            switch (examNo)
            {
                case "1":
                    result = exam.StringReverse();
                    break;
                case "2":
                    result = exam.TransactionRollBack();
                    break;
                case "3":
                    result = exam.Fibonacci_Test();
                    break;
                default:
                    return BadRequest("invalid examNo: " + examNo);
            }

            return Ok(new { result });
        }
    }
}