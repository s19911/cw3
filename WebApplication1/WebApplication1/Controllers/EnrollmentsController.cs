﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateStudent(EnrollmentStudentOdp request)
        {
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19562;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                //idStudies
                com.CommandText = "select IdStudy from Studies where name=@StudyName";
                com.Parameters.AddWithValue("StudyName", request.Studies);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    tran.Rollback();
                    return BadRequest("Studia nie istnieja");
                }
                int idStudies = (int)dr["IdStudy"];
                dr.Close();

                // Enrollment
                com.CommandText =
                    "select * from Enrollment where Semester=1 and IdStudy=@idStudies and StartDate=(select max(StartDate) from Enrollment)";
                com.Parameters.AddWithValue("idStudies", idStudies);
                dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    com.CommandText =
                        "insert into Enrollment(IdEnrollment,Semester,IdStudy,StartDate) values ((select max(IdEnrollment)from Enrollment)+1,1,@idStudies,(SELECT CAST(GETDATE() AS DATE)))";
                    com.ExecuteNonQuery();
                    dr.Close();
                    com.CommandText =
                        "select IdEnrollment from Enrollment where Semester=1 and IdStudy=@idStudies and StartDate=(select max(StartDate) from Enrollment)";
                    dr = com.ExecuteReader();
                    dr.Read();
                }
                int idEnrollment = (int)dr["IdEnrollment"];
                int semesterEnrollment = (int)dr["Semester"];
                int idStudyEnrollment = (int)dr["IdStudy"];
                string startDateEnrollment = dr["StartDate"].ToString();
                dr.Close();

                //idStudent unique
                com.CommandText = "select IndexNumber from Student where IndexNumber=@IndexStudent";
                com.Parameters.AddWithValue("IndexStudent", request.IndexNumber);
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    tran.Rollback();
                    return BadRequest("student z tym indexem juz istnieje");
                }
                dr.Close();

                com.CommandText =
                    "insert into Student(IndexNumber,FirstName,LastName,BirthDate,IdEnrollment) values (@IndexStudent,@FirstName,@LastName,Convert(varchar(40),@BirthDate,4) ,@idStudies)";
                com.Parameters.AddWithValue("FirstName", request.FirstName);
                com.Parameters.AddWithValue("LastName", request.LastName);
                com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                com.ExecuteNonQuery();

                tran.Commit();
                return CreatedAtAction("createStudent", new EnrollmentOdp { IdEnrollment = idEnrollment, IdStudy = idStudyEnrollment, Semester = semesterEnrollment, StartDate = startDateEnrollment });

            }
        }



      
    }
}