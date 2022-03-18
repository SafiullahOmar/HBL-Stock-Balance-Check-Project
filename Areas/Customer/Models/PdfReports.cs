using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineStore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Customer.Models
{
    
    public class PdfReports
    {
        // string constr = "Server=DESKTOP-0KC8G0J;database=BerajStore;Integrated Security=SSPI; Persist Security =fasle";
        //string constr = ConfigurationExtensions.GetConnectionString(_configuration, "RazorPagesMovieContext");
        private readonly string constr;
        public PdfReports(IConfiguration configuration) {
            constr = configuration.GetConnectionString("DefaultConnection");
        }

        
        public DataTable GetSummaryReports(string proc) {


            var dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(constr)) {
                SqlCommand com = new SqlCommand(proc, connection);
                com.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt); 
                connection.Close();

            }

            return dt;
        }

        public DataTable GetBillReports(string Date,string Bill)
        {
            var dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand("spBillReport", connection);
                com.Parameters.AddWithValue("@Date", SqlDbType.Date).Value=Date;
                com.Parameters.AddWithValue("@Bill", SqlDbType.NVarChar).Value=Bill;
                com.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                connection.Close();

            }

            return dt;
        }
    }
}
