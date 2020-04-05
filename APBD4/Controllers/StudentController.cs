using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Models;
using APBD4.Sdata;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly Sdatabase studentdata;
        public StudentController(Sdatabase db)
        {
            studentdata = db;
        }
        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {

            return Ok(studentdata.GetStudents());
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Lewandowski");
            }
            else if (id == 2)
            {
                return Ok("Tracz");
            }
            return NotFound("Student not found");
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            if (id == 1) { return Ok("Update completed"); }
            else return NotFound("Student not found");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            if (id == 1) { return Ok("Delete completed"); }
            else return NotFound("Student not found");
        }
    }

}
