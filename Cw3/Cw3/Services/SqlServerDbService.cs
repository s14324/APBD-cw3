using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public class SqlServerDbService : IDbService
    {
        public IEnumerable<Student> GetStudents()
        {
            return null;
        }

        public Student GetStudent(string index)
        {
            SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s14324;Integrated Security=True");
            SqlCommand com = new SqlCommand("SELECT * from Student where IndexNumber like @Index");

            using (con)
            using (com)
            {
                com.Connection = con;

                com.Parameters.AddWithValue("@Index", index);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    return st;
                }
                return null;
            }
        }
    }
}
