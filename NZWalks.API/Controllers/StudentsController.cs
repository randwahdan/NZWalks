using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https//:localhost:portNumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET : https//:localhost:portNumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents() 
        {
            string[] studentsName = new string[] { "Rand", "Abdulrahman", "Rahaf", "Roaa" };
            return Ok();    
        }

    }
}
