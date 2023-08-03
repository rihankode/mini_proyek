using Microsoft.Data.SqlClient;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using Newtonsoft.Json;
using System.Data;

namespace mini_proyek.Services
{
    public class SlotsServices : SlotInterface
    {
        private DbContex _context;

        private IConfiguration _configuration;
        public SlotsServices(DbContex context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Dictionary<string, object> get_availibility(Slots request)
        {
            var resenkrip = new Dictionary<string, object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                {


                    SqlCommand cmd3 = new SqlCommand("Sp_slot_avalability", con);
                    con.Open();
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@identity", request.userIdentity);
                    SqlDataAdapter adpt1 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    adpt1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                        resenkrip.Add("status", dt1.Rows[0]["sts"]);
                        resenkrip.Add("message", dt1.Rows[0]["msg"]);
                    }
                    else
                    {
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Check In Gagal");
                    }


                }

            }
            catch (Exception q)
            {

                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Check In Gagal");
            }

          
            return resenkrip;
        }

        public Dictionary<string, object> get_checkout(Slots request)
        {
            var resenkrip = new Dictionary<string, object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                {


                    SqlCommand cmd3 = new SqlCommand("Sp_slot_checkout", con);
                    con.Open();
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@identity", request.userIdentity);
                    SqlDataAdapter adpt1 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    adpt1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                        resenkrip.Add("status", dt1.Rows[0]["sts"]);
                        resenkrip.Add("message", dt1.Rows[0]["msg"]);
                    }
                    else
                    {
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Check Out Gagal");
                    }


                }

            }
            catch (Exception q)
            {

                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Check Out Gagal");
            }


            return resenkrip;
        }

        public Dictionary<string, object> get_slot(Slots request)
        {
            var res = new Dictionary<string, object>();

            string filter = "";
            string qfilter = "";

            for (int i = 0; i < request.filter?.Count; i++)
            {
                //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.filter[i], Formatting.Indented));

                if (objJson["fieldName"] == "kode")
                {
                    filter = "a.slot_kode";
                }
                else if (objJson["fieldName"] == "number")
                {
                    filter = "b.area_number";
                }
                else if (objJson["fieldName"] == "name")
                {
                    filter = "b.kategori_name";
                }
               

                qfilter += " AND " + filter + " = '" + objJson["value"] + "' ";
            }

            var dump = qfilter;


            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {
                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                string querys = String.Format("select a.slot_kode kode, b.area_number number,c.kategori_name name, a.slot_user_id userId,format(d.user_car_login_created, 'dd MMMM yyyy hh:MM', 'id-ID') as timeCheckin ," +
                    " case when a.slot_sts = '1' then 'kososng' when a.slot_sts='2' 'digunakan' else 'non-aktif' end as 'status' " +
                    "from mg_parking_slot a WITH (NOLOCK)       join mg_parking_area b on a.area_id = b.area_id" +
                    " join md_kategori_area c on c.kat_id = b.area_kategori_id left join mg_parking_user_car d on d.user_car_id=a.slot_user_id " +
                    "WHERE  1=1 {0}  ORDER BY c.kategori_seq, b.area_number  OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY ",  qfilter, request.index, request.perpage);
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
                    res.Add("message", "Get Data Slot Success");
                    res["data"] = dataResult;


                    //#region Log Location

                    //Log_User_Location(request.userid, request.lon, request.lat, request.addr, "Get Data All");

                    //#endregion
                }
                else
                {
                    res.Add("status", "1");
                    res.Add("message", "Get Data Slot Not Found");
                }

            }

            return res;
        }
    }
}
