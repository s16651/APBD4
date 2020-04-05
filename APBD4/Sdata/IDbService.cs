using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD4.Models;

namespace APBD4.Sdata
{

	
		public interface IDbService
		{
			public IEnumerable<Student> GetStudents();
		}
	}

