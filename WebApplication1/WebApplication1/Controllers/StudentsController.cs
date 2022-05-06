using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie3.Models;
//using Cwiczenie3.Serivices;
using Cwiczenie3.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Cwiczenie3.Services;

namespace Cwiczenie3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private IDbService _dbService;

        public StudentsController(IDbService service)
        {
            _dbService = service;
        }

        //ja zaczełam jeszcze raz z wykładu bo się pogubiłam (nie działało mi dobrze)

        [HttpGet("{id}")]
        public IActionResult GetStudentId(int id)
        {
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19562;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = $"select * from Student where Student.IndexNumber={id}";

                con.Open();
                var dr = com.ExecuteReader();

                dr.Read();
                string idd = dr["IdEnrollment"].ToString();
                dr.Close();
                com.CommandText = $"select * from Enrollment where Enrollment.IdEnrollment={idd}";
                dr = com.ExecuteReader();
                var st = new List<string>();

                dr.Read();
                return Ok(dr["IdEnrollment"].ToString());
                dr.Close();

            }
        }


        //2. QueryString
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {

            //return Ok(_dbService.GetStudents

            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19562;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "select * from Student";


                con.Open();

                var dr = com.ExecuteReader();
                var st = new List<string>();

                while (dr.Read())
                {
                    if (dr["LastName"] == DBNull.Value)
                    {

                    }
                    Console.WriteLine(dr["LastName"].ToString());
                    st.Add(dr["LastName"].ToString());

                }
                return Ok(st);
            }
        }

        [HttpGet("{id}/semester")]
        public IActionResult GetSemester(int id)
        {
            String st = "";
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19562;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select Semester from enrollment join student on enrollment.Idenrollment=student.Idenrollment where student.IndexNumber=@id";
                com.Parameters.AddWithValue("id", "s" + id.ToString());
                con.Open();
                var dr = com.ExecuteReader();

                while (dr.Read())
                {
                    st += st + dr["Semester"] + "\r\n";
                }
            }

            return Ok(st);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentWpis(String id)
        {
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19562;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student where Student.IndexNumber=@id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                dr.Read();
                string ajdi = dr["IdEnrollment"].ToString();

                return Ok(dr[0].ToString() + "," +dr[1] + "," +dr[2]);

            }

        }


        //[FromRoute], [FromBody], [FromQuery]
        //1. URL segment
        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]int id) //action method
        {
            if(id <= _dbService.GetStudents().Count())
            {
                return Ok(_dbService.GetStudents().Where(student =>
                student.IdStudent == id));
            }
            else
                return NotFound("Student was not found");
        }

        //3. Body - cialo zadan
        [HttpPost]
        public IActionResult CreateStudent([FromBody]Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            //...
            return Ok(student); //JSON 
        }

        /*[HttpPut("{id}")]
        public IActionResult PutStudent([FromRoute]int id , [FromBody]Student student)
        {
            student.IdStudent = id;
            return Ok(student + "Aktualizacja zakonczona");
        }*/

        [HttpPut("{id}")]
       public IActionResult PutStudent([FromRoute]int id)
       {
          
           return Ok("Aktualizacja zakonczona");
       }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent([FromRoute]int id)
        {
            return Ok("Usuwanie ukonczone");
        }


        [HttpPost]
        public IActionResult EnrollmentsController(Student s)
        {
            IStudentsDbService service = new ServerDbService();
            return null;

        }

        

    }
}

