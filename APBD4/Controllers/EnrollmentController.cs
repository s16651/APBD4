using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD4.DTOs.Requests;
using APBD4.DTOs.Responses;
using APBD4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD4.Controllers
{
    [ApiController]
    [Authorize(Roles ="employee")]
    [Route("api/enrollment")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SDbService _db;
        public EnrollmentsController(SDbService db)
        {
            _db = db;
        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            EnrollStudentResponse enrollStudentResponse = _db.EnrollStudent(request);
            if (enrollStudentResponse == null)
            {
                return BadRequest();
            }
            return this.StatusCode(201, enrollStudentResponse);
        }
        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentRequest promoteStudentRequest)
        {
            PromoteStudentResponse promoteStudentResponse = _db.PromoteStudents(promoteStudentRequest);
            if (promoteStudentResponse == null)
            {
                return NotFound();
            }
            return this.StatusCode(201, promoteStudentResponse);
        }
        
    }
}