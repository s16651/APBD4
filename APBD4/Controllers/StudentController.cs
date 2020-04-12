using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private string connString = @"Data Source=db-mssql;Initial Catalog=s16651;Integrated Security=True";
        public StudentController() { }
        [HttpGet]
        public IActionResult GetStudents()
        {
            var listOfStudents = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connString)) 
            using (SqlCommand command = new SqlCommand()) 
                {
                    command.Connection = connection;
                    command.CommandText = @"select s.FirstName, s.LastName, s.BirthDate, st.Name as Studies, e.Semester
                                            from Student s
                                            join Enrollment e on e.IdEnrollment = s.IdEnrollment
                                            join Studies st on st.IdStudy = e.IdStudy;";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var st = new Student
                        {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DateOfBirth = DateTime.Parse(reader["BirthDate"].ToString()),
                        Studies = reader["Studies"].ToString(),
                        Semester = int.Parse(reader["Semester"].ToString())
                    };
                        listOfStudents.Add(st);
                    }
                }
            return Ok(listOfStudents);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetSemester(string indexNumber)
        {
            string semester = null;
            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"select e.semester from student as s 
                                     join Enrollment as e on s.idenrollment = e.idenrollment 
                                     where s.indexnumber=@index;";
                command.Parameters.AddWithValue("index", indexNumber);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    semester = reader["Semester"].ToString();
                }
                else
                {
                    semester = "Index number not assigned";
                }
                return Ok("Semester entry " + semester + " for Student with number: " + indexNumber); 
            }
        }
        /*
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
        }*/
    }

}
