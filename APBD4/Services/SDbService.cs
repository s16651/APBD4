using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Controllers;
using APBD4.DTOs.Requests;
using APBD4.DTOs.Responses;
using APBD4.Models;

namespace APBD4.Services
{
	public interface SDbService
	{
		public IEnumerable<Student> GetStudents();
		public List<Enrollment> GetStudent(string index);
		public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
		public PromoteStudentResponse PromoteStudents(PromoteStudentRequest promoteStudentRequest);
	}
}
