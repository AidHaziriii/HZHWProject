using Microsoft.AspNetCore.Mvc;
using HZHWProject.Models;
using Microsoft.Data.SqlClient;

namespace HZHWProject.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        void ConnectionString()
        {
            con.ConnectionString = "data source = localhost; database = HZHW; integrated security = SSPI;";
        }
        [HttpPost]
        public IActionResult Verify(Student student)
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Student where username ='"+student.username+"' and password= '"+student.password+"'";
            dr = com.ExecuteReader();   
            if(dr.Read())
            {
                con.Close();
                return View("Success");
            }
            else
            {
                con.Close();
                return View("Error");
            }
            
        }
    }
}
