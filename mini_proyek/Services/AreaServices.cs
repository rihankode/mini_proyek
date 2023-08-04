using Microsoft.Data.SqlClient;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using Newtonsoft.Json;
using System.Data;

namespace mini_proyek.Services
{
    public class AreaServices : AreaInterfaces
    {
        private DbContex _context;
        private IConfiguration _configuration;
        public AreaServices(DbContex context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Dictionary<string, object> Create_data_area(Area request)
        {

            var resenkrip = new Dictionary<string, object>();
            var res = new Dictionary<string, object>();
            var resError = new Dictionary<string, object>();

            try
            {
                for (int i = 0; i < request.data?.Count; i++)
                {
                    //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                    dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.data[i], Formatting.Indented));
                    string kategoriId = objJson["kategoriId"].ToString();
                    string areaName = objJson["areaNumber"].ToString();

                    if (kategoriId == "" || areaName == "")
                    {
                        resError["error"] = "data tidak boleh kosong !!";
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Tambah Data Area Gagal");
                        resenkrip["data"] = resError;
                        return resenkrip;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                        {

                            SqlCommand cmd1 = new SqlCommand(" select top 1 1 from mg_parking_area where area_kategori_id = @kategori and area_number = @number " +
                               "", con);
                            con.Open();
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Parameters.AddWithValue("@kategori", kategoriId);
                            cmd1.Parameters.AddWithValue("@number", areaName);
                            SqlDataAdapter adpt = new SqlDataAdapter(cmd1);
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            con.Close();

                            if (dt.Rows.Count == 1)
                            {
                                resError["error"] = "data tidak boleh sama !!";
                                resenkrip.Add("status", "0");
                                resenkrip.Add("message", "Tambah Data Area Gagal");
                                resenkrip["data"] = resError;
                                return resenkrip;
                            }
                            else
                            {

                                

                                SqlCommand cmd3 = new SqlCommand("Sp_Create_Data_Area", con);
                                con.Open();
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@kategoriId", kategoriId);
                                cmd3.Parameters.AddWithValue("@areaName", areaName);
                                SqlDataAdapter adpt1 = new SqlDataAdapter(cmd3);
                                DataTable dt1 = new DataTable();
                                adpt1.Fill(dt1);
                                con.Close();

                               

                            }

                        }

                    }

                }


                resenkrip.Add("status", "1");
                resenkrip.Add("message", "Tambah Data Ares Success");
                //resenkrip["data"] = dataResult;
            }
            catch (Exception e)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Tambah Data Area Gagal");
            }



            return resenkrip;
        }
        public Dictionary<string, object> Delete_area(Area request)
        {

            var resenkrip = new Dictionary<string, object>();

            string msg = "";
            string sts = "";

            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                {
                    SqlCommand cmd2 = new SqlCommand("SELECT top 1 1 FROM mg_parking_slot where slot_user_id IS NULL  ", con);
                    con.Open();
                    cmd2.CommandType = CommandType.Text;
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd2);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        msg = " Tempat Sedang digunakan tidak bisa dihapus ";
                        sts = "0";
                    }
                    else
                    {
                        SqlCommand cmd1 = new SqlCommand("DELETE FROM mg_parking_slot WHERE area_id = @id ", con);
                        con.Open();
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@id", request.id);
                        cmd1.ExecuteNonQuery();
                        //cmd.Parameters.AddWithValue("@regno", request.regno);
                        //cmd.Parameters.AddWithValue("@type", request.type);
                        con.Close();

                        SqlCommand cmd = new SqlCommand("DELETE FROM mg_parking_area WHERE area_id = @id ", con);
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", request.id);
                        cmd.ExecuteNonQuery();
                        //cmd.Parameters.AddWithValue("@regno", request.regno);
                        //cmd.Parameters.AddWithValue("@type", request.type);
                        con.Close();

                        msg = "Tempat Berhasil dihapus ";
                        sts = "1";
                    }


                    resenkrip.Add("status", sts);
                    resenkrip.Add("message", msg);
                }


            }


              
            catch (Exception e )
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Hapus Data Ares Gagal");
            }

           

            return resenkrip;
        }

        public Dictionary<string, object> getDataById(Area request)
        {
            var resenkrip = new Dictionary<string, object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                {
                    List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                    SqlCommand cmd = new SqlCommand("SELECT a.area_id AS id, b.kategori_name AS areaName, a.area_number as areaNumber " +
                        "FROM mg_parking_area a  join md_kategori_area b on a.area_kategori_id=b.kat_id where a.area_id=@id", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", request.id);

                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);
                    con.Close();
                    //cmd.Parameters.AddWithValue("@regno", request.regno);
                    //cmd.Parameters.AddWithValue("@type", request.type);
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
                        resenkrip.Add("status", "1");
                        resenkrip.Add("message", "Get Data Area Success");
                        resenkrip["data"] = dataResult;


                        //#region Log Location

                        //Log_User_Location(request.userid, request.lon, request.lat, request.addr, "Get Data All");

                        //#endregion
                    }
                    else
                    {
                        resenkrip.Add("status", "1");
                        resenkrip.Add("message", "Get Data Area Not Found");
                    }
                }

            }
            catch (Exception e)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Hapus Data Area Gagal");
            }

            return resenkrip;
        }

        public Dictionary<string, object> Get_data_Area(Area request)
        {

            var res = new Dictionary<string, object>();

            string filter = "";
            string qfilter = "";

            for (int i = 0; i < request.filter?.Count; i++)
            {
                //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.filter[i], Formatting.Indented));

                if (objJson["fieldName"] == "areaName")
                {
                    filter = "a.area_kategori_id";
                }
                else if (objJson["fieldName"] == "areaNumber")
                {
                    filter = "a.area_number";
                }

                qfilter += " AND " + filter + " = '" + objJson["value"] + "' ";
            }

            var dump = qfilter;


            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {
                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                string querys = String.Format("SELECT a.area_id AS id, b.kategori_name AS areaName, a.area_number as areaNumber " +
                    "FROM mg_parking_area a WITH (NOLOCK) join md_kategori_area b on a.area_kategori_id=b.kat_id where 1=1 {0}  ORDER BY a.area_id OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY ", 
                    qfilter, request.index, request.perpage);
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
                    res.Add("message", "Get Data Area Success");
                    res["data"] = dataResult;


                    //#region Log Location

                    //Log_User_Location(request.userid, request.lon, request.lat, request.addr, "Get Data All");

                    //#endregion
                }
                else
                {
                    res.Add("status", "1");
                    res.Add("message", "Get Data Area Not Found");
                }

            }

            return res;
        }
        public Dictionary<string, object> Update_Data_area(Area request)
        {
            var resenkrip = new Dictionary<string, object>();
            var resError = new Dictionary<string, object>();


            try
            {
                for (int i = 0; i < request.data?.Count; i++)
                {
                    //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                    dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.data[i], Formatting.Indented));


                    string kategoriId = objJson["kategoriId"].ToString();
                    string areaName = objJson["areaNumber"].ToString();


                    if (kategoriId == "" || areaName == "")
                    {
                        resError["error"] = "data tidak boleh kosong !!";
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Update Data Area Gagal");
                        resenkrip["data"] = resError;
                        return resenkrip;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                        {

                            SqlCommand cmd1 = new SqlCommand(" select top 1 1 from mg_parking_area where area_kategori_id = @kategori and area_number = @number " +
                              "", con);
                            con.Open();
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Parameters.AddWithValue("@kategori", kategoriId);
                            cmd1.Parameters.AddWithValue("@number", areaName);
                            SqlDataAdapter adpt = new SqlDataAdapter(cmd1);
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            con.Close();

                            if (dt.Rows.Count == 1)
                            {
                                resError["error"] = "data tidak boleh sama !!";
                                resenkrip.Add("status", "0");
                                resenkrip.Add("message", "Update Data Area Gagal");
                                resenkrip["data"] = resError;
                                return resenkrip;
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand("UPDATE mg_parking_area SET area_kategori_id = @kategori, area_number=@number where area_id=@id ", con);
                                con.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@kategori", kategoriId);
                                cmd.Parameters.AddWithValue("@number", areaName);
                                cmd.Parameters.AddWithValue("@id", request.id);
                                cmd.ExecuteNonQuery();
                                //cmd.Parameters.AddWithValue("@regno", request.regno);
                                //cmd.Parameters.AddWithValue("@type", request.type);
                                con.Close();
                            }

                              
                        }
                    }

                      

                }
                resenkrip.Add("status", "1");
                resenkrip.Add("message", "Update Data Ares Success");
            }
            catch (Exception q)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Update Data Ares Gagal");
                
            }

          
            return resenkrip;
        }

    }
}
