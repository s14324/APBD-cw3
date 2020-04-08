using System.Collections.Generic;
using System.Data.SqlClient;
using Cw3.Models;
using Cw3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase //cw4 - outdated ----------------------------------
    {
        private IDbService _dbService;

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s14324;Integrated Security=True";


        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents([FromServices] IDbService dbService)
        {
            List<Student> list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM Student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    list.Add(st);
                }
            }
            return Ok(list);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT Student.IndexNumber, Enrollment.Semester FROM Student INNER JOIN Enrollment ON Enrollment.IdEnrollment = Student.IdEnrollment";

                com.Parameters.AddWithValue("index", indexNumber);
                con.Open();

                var dr = com.ExecuteReader();

                if (dr.Read())
                {
                    return Ok("Student index: " + dr["IndexNumber"].ToString() + " Wpis na semestr: " + dr["Semester"].ToString());
                }
            }
            return NotFound();
        }
    }
}