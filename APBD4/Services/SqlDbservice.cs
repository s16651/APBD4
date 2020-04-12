using APBD4.DTOs.Requests;
using APBD4.DTOs.Responses;
using APBD4.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBD4.Services
{
	public class SqlDbservice : SDbService
	{
		private string connString = @"Data Source=db-mssql;Initial Catalog=s16651;Integrated Security=True";
		public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
		{
			using (var conn = new SqlConnection(connString))
			{
				conn.Open();
				int enrollmentId = 1;
				int studyId = 1;
				var transaction = conn.BeginTransaction();
				using (var comm = new SqlCommand())
				{
					comm.CommandText = @"select * from Studies a 
										where a.Name=@Name";
					comm.Parameters.AddWithValue("@Name", request.Studies);
					comm.Transaction = transaction;
					comm.Connection = conn;
					var query = comm.ExecuteReader();
					if (!query.Read())
					{
						query.Close();
						transaction.Rollback();
						conn.Close();
						return null;
					}
					studyId = int.Parse(query["IdStudy"].ToString());
					query.Close();
				}

				using (var comm = new SqlCommand())
				{
					comm.CommandText = @"select * from Enrollment a
										where a.IdStudy=@StudyID and a.Semester=1";
					comm.Parameters.AddWithValue("@StudyID", studyId);
					comm.Transaction = transaction;
					comm.Connection = conn;
					var idEnroll = comm.ExecuteReader();
					{
						if (idEnroll.Read())
						{
							enrollmentId = int.Parse(idEnroll["IdEnrollment"].ToString());
							idEnroll.Close();
						}
						else
						{
							idEnroll.Close();
							using (var enroll = new SqlCommand())
							{
								enroll.CommandText = "Select MAX(a.IdEnrollment) as IdEnrollment From Enrollment a";
								enroll.Transaction = transaction;
								enroll.Connection = conn;
								var max = enroll.ExecuteReader();
								if (max.Read())
								{
									enrollmentId = (int.Parse(max["IdEnrollment"].ToString())) + 1;
								}
								max.Close();
							}
							using (var comm2 = new SqlCommand())
							{
								comm2.CommandText = @"Insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)
													Values(@IdEnrollment, @Semester, @IdStudy, @StartDate)";
								comm2.Parameters.AddWithValue("@IdEnrollment", enrollmentId);
								comm2.Parameters.AddWithValue("@Semester", 1);
								comm2.Parameters.AddWithValue("@IdStudy", studyId);
								comm2.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd"));
								comm2.Connection = conn;
								comm2.Transaction = transaction;
								comm2.ExecuteNonQuery();
							}
						}
					}
				}
				using (var comm = new SqlCommand())
				{
					comm.CommandText = @"Select * From Student a
                                         Where a.IndexNumber=@IndexNumber";
					comm.Parameters.AddWithValue("@IndexNumber", request.IndexNumber);
					comm.Connection = conn;
					comm.Transaction = transaction;

					var index = comm.ExecuteReader();
					{
						if (index.Read())
						{
							index.Close();
							transaction.Rollback();
							conn.Close();
							return null;
						}
					}index.Close();
				}
				using (var comm2 = new SqlCommand())
				{
					comm2.CommandText = @"Insert into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)
                                          Values(@IndexNumber1, @FirstName, @LastName, @BirthDate, @IdEnrollment1)";
					comm2.Parameters.AddWithValue("IndexNumber1", request.IndexNumber);
					comm2.Parameters.AddWithValue("@FirstName", request.FirstName);
					comm2.Parameters.AddWithValue("@LastName", request.LastName);
						comm2.Parameters.AddWithValue("@BirthDate", DateTime.ParseExact(request.BirthDate, "dd.MM.yyyy", null));
					comm2.Parameters.AddWithValue("@IdEnrollment1", enrollmentId);
					comm2.Connection = conn;
					comm2.Transaction = transaction;
					comm2.ExecuteNonQuery();
				}
				transaction.Commit();
				using (var comm3 = new SqlCommand())
				{
					comm3.CommandText = @"Select * From Enrollment a
                                            Where a.IdEnrollment=@IdEnrollment2";
					comm3.Parameters.AddWithValue("@IdEnrollment2", enrollmentId);
					var enrollStudentResponse = new EnrollStudentResponse();
					comm3.Connection = conn;
					using (var reader2 = comm3.ExecuteReader())
					{
						while (reader2.Read())
						{
							enrollStudentResponse.IdEnrollment = int.Parse(reader2["IdEnrollment"].ToString());
							enrollStudentResponse.Semester = int.Parse(reader2["Semester"].ToString());
							enrollStudentResponse.IdStudy = int.Parse(reader2["IdStudy"].ToString());
							enrollStudentResponse.StartDate = DateTime.Parse(reader2["StartDate"].ToString());
						}
					}

					return enrollStudentResponse;
				}
			}
		}
		public List<Enrollment> GetStudent(string index)
		{
			var listOfEnrollments = new List<Enrollment>();
			using (var conn = new SqlConnection(connString))
			{
				using (var comm = new SqlCommand())
				{
					comm.Connection = conn;
					comm.CommandText = @"select a.IdEnrollment, a.Semester, a.IdStudy, a.StartDate
                                            from Student a
                                            join Enrollment b on b.IdEnrollment = a.IdEnrollment
                                            where f.IndexNumber = @index;";
					comm.Parameters.AddWithValue("@index", index);
					conn.Open();
					using (var reader = comm.ExecuteReader())
					{
						while (reader.Read())
						{
							var enrollment = new Enrollment
							{
								IdEnrollment = int.Parse(reader["IdEnrollment"].ToString()),
								Semester = int.Parse(reader["Semester"].ToString()),
								IdStudy = int.Parse(reader["IdStudy"].ToString()),
								StartDate = DateTime.Parse(reader["StartDate"].ToString())
							};
							listOfEnrollments.Add(enrollment);
						}
					}
				}

				return listOfEnrollments;
			}
		}
		public IEnumerable<Student> GetStudents()
		{
			var listOfStudents = new List<Student>();
			using (var conn = new SqlConnection(connString))
			{
				using (var comm = new SqlCommand())
				{
					comm.Connection = conn;
					comm.CommandText = @"select a.FirstName, a.LastName, a.BirthDate, at.Name as Studies, a.Semester
                                            from Student a
                                            join Enrollment b on b.IdEnrollment = a.IdEnrollment
                                            join Studies st on at.IdStudy = b.IdStudy;";
					conn.Open();
					using (var reader = comm.ExecuteReader())
					{
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

				}
			}
			return listOfStudents;
		}
		public PromoteStudentResponse PromoteStudents(PromoteStudentRequest promoteStudentRequest)
		{
			using (var conn = new SqlConnection(connString))
			{
				conn.Open();
				var transaction = conn.BeginTransaction();
				using (var comm = new SqlCommand())
				{
					comm.CommandText = @"select * from 
                                            Enrollment a inner join Studies b
                                            On a.IdStudy=b.IdStudy 
                                            And b.Name=@Name                                            
                                            AND a.Semester=@Semester";
					comm.Parameters.AddWithValue("@Name", promoteStudentRequest.Studies);
					comm.Parameters.AddWithValue("@Semester", promoteStudentRequest.Semester);
					comm.Connection = conn;
					comm.Transaction = transaction;
					using (var reader = comm.ExecuteReader())
					{
						if (!reader.Read())
						{
							reader.Close();
							transaction.Rollback();
							return null;
						}
					}
				}
				using (var comm1 = new SqlCommand("promoteStudent", conn))
				{
					comm1.CommandType = System.Data.CommandType.StoredProcedure;
					comm1.CommandText = "PromoteStudent";
					comm1.Parameters.AddWithValue("@studiesGiven", promoteStudentRequest.Studies);
					comm1.Parameters.AddWithValue("@semesterGiven", promoteStudentRequest.Semester);
					comm1.Connection = conn;
					comm1.Transaction = transaction;
					using (var reader = comm1.ExecuteReader())
					{
						reader.Read();
						PromoteStudentResponse promoteStudentResponse = new PromoteStudentResponse
						{
							IdEnrollment = int.Parse(reader["IdEnrollment"].ToString()),
							Semester = int.Parse(reader["Semester"].ToString()),
							IdStudy = int.Parse(reader["IdStudy"].ToString()),
							StartDate = DateTime.Parse(reader["StartDate"].ToString())
						};
						reader.Close();
						transaction.Commit();
						return promoteStudentResponse;

					}
				}
			}
		}
	}
}

	