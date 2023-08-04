using Microsoft.Data.SqlClient;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using Newtonsoft.Json;
using System.Data;

namespace mini_proyek.Services
{
    public class HistoryServices : HistoryInterface
    {
        private DbContex _context;
        private IConfiguration _configuration;

        public HistoryServices(DbContex context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        public Dictionary<string, object> Get_data_Hostory(History request)
        {
            var res = new Dictionary<string, object>();

            string filter = "";
            string qfilter = "";

            for (int i = 0; i < request.filter?.Count; i++)
            {
                //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.filter[i], Formatting.Indented));

                if (objJson["fieldName"] == "kategoriName")
                {
                    filter = "c.kategori_name";
                }
                else if (objJson["fieldName"] == "number")
                {
                    filter = "b.kategori_name";
                }
                else if (objJson["fieldName"] == "description")
                {
                    filter = "a.hist_description";
                }

                qfilter += " AND " + filter + " LIKE '%" + objJson["value"] + "%' ";
            }

            var dump = qfilter;


            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {
                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                string querys = String.Format("SELECT b.area_number number, C.kategori_name kategoriName, A.user_car_id userId,A.hist_sts, CASE WHEN A.logout IS NULL THEN FORMAT(A.login, 'dd MMMM yyyy HH:mm','id-ID') ELSE FORMAT(A.logout,'dd MMMM yyyy HH:mm') END timeline" +
                    " ,A.hist_description description FROM mg_park_history A WITH(NOLOCK)" +
                    " JOIN mg_parking_area b on A.hist_area_id=b.area_id JOIN md_kategori_area c on c.kat_id=b.area_kategori_id\r\n " +
                    " where 1=1 {0}  ORDER BY b.area_number OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY ", qfilter, request.index, request.perpage);
                SqlCommand cmd = new SqlCommand(querys, con);
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@INDEX", request.index);
                cmd.Parameters.AddWithValue("@PERPAGE", request.perpage);
                cmd.ExecuteNonQuery();
                //cmd.Parameters.AddWithValue("@regno", request.regno);
                //cmd.Parameters.AddWithValue("@type", request.type);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var resx = new Dictionary<string, object>();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            resx[dt.Columns[j].ToString()] = dt.Rows[i][j].ToString();
                        }
                        dataResult.Add(resx);
                    }
                    res.Add("status", "1");
                    res.Add("message", "Get Data Kategori Success");
                    res["data"] = dataResult;


                    //#region Log Location

                    //Log_User_Location(request.userid, request.lon, request.lat, request.addr, "Get Data All");

                    //#endregion
                }
                else
                {
                    res.Add("status", "1");
                    res.Add("message", "Get Data Kategori Not Found");
                }

            }

            return res;
        }

        public Dictionary<string, object> Get_data_Hostor_by_idy(History request)
        {

            var res = new Dictionary<string, object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {
                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                string querys = "SELECT b.area_number number, C.kategori_name kategoriName, A.user_car_id userId,A.hist_sts, CASE WHEN A.logout IS NULL THEN FORMAT(A.login, 'dd MMMM yyyy HH:mm','id-ID') ELSE FORMAT(A.logout,'dd MMMM yyyy HH:mm') END timeline" +
                    " ,A.hist_description description FROM mg_park_history A " +
                    " JOIN mg_parking_area b on A.hist_area_id=b.area_id JOIN md_kategori_area c on c.kat_id=b.area_kategori_id\r\n " +
                    " where A.user_car_id=@userid ";
                SqlCommand cmd = new SqlCommand(querys, con);
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@userid", request.userid);
                cmd.ExecuteNonQuery();
                //cmd.Parameters.AddWithValue("@regno", request.regno);
                //cmd.Parameters.AddWithValue("@type", request.type);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var resx = new Dictionary<string, object>();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            resx[dt.Columns[j].ToString()] = dt.Rows[i][j].ToString();
                        }
                        dataResult.Add(resx);
                    }
                    res.Add("status", "1");
                    res.Add("message", "Get Data History Success");
                    res["data"] = dataResult;


                    //#region Log Location

                    //Log_User_Location(request.userid, request.lon, request.lat, request.addr, "Get Data All");

                    //#endregion
                }
                else
                {
                    res.Add("status", "1");
                    res.Add("message", "Get Data History Not Found");
                }
            }
             return res;
        }
    }
}
