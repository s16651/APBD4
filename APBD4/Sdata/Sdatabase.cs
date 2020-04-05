using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Models;

namespace APBD4.Sdata
{
	public class Sdatabase : IDbService
	{
		private static IEnumerable<Student> _students;

		static Sdatabase()
		{
			_students = new List<Student> {
			new Student { IdStudent = 1, FirstName = "Kuba", LastName = "Lewandowski" },
			new Student { IdStudent = 2, FirstName = "Michał", LastName = "Tracz" },
			new Student { IdStudent = 3, FirstName = "Tomek", LastName = "Kot" }
			};
		}
		public IEnumerable<Student> GetStudents()
		{
			return _students;
		}
	}
}
