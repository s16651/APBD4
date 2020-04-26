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
		public Student GetStudentByIndex(string index);
		public void SaveLogData(string method, string query, string path,string body);
		public LoginResp Login(LoginRequestDto loginRequest);
		public void SaveToken(string login, string name, string token);
		public TokenResp CheckToken(string token);
	}
}
