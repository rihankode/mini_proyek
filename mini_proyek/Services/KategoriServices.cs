using Azure.Core;
using Microsoft.Data.SqlClient;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using Newtonsoft.Json;
using System.Data;

namespace mini_proyek.Services
{
    public class KategoriServices : KategoriInterfaces
    {

        private DbContex _context;

        private IConfiguration _configuration;
        public KategoriServices(DbContex context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Dictionary<string, object> Create_data_kategori(Kategori request)
        {

            var resenkrip = new Dictionary<string, object>();
            var resError = new Dictionary<string, object>();

            try
            {
                for (int i = 0; i < request.data?.Count; i++)
                {
                    //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                    dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.data[i], Formatting.Indented));


                 

                    string data = objJson["kategoriName"].ToString();

                    if ( data == "")
                    {
                        resError["error"] = "data tidak boleh kosong !!";
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Tambah Data Kategori Gagal");
                        resenkrip["data"] = resError;
                        return resenkrip;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                        {


                            SqlCommand cmd1 = new SqlCommand(" select top 1 1 from md_kategori_area where kategori_name = @kategori " +
                               "", con);
                            con.Open();
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Parameters.AddWithValue("@kategori", data);
                            SqlDataAdapter adpt = new SqlDataAdapter(cmd1);
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            con.Close();

                            if (dt.Rows.Count == 1)
                            {
                                resError["error"] = "data tidak boleh sama !!";
                                resenkrip.Add("status", "0");
                                resenkrip.Add("message", "Tambah Data Kategori Gagal");
                                resenkrip["data"] = resError;
                                return resenkrip;
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand(" INSERT INTO md_kategori_area (kategori_name,kategori_area_sts) VALUES (@kategori,1) " +
                              "", con);
                                con.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@kategori", data);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }

                          
                          
                        }

                    }

                   

                }


                resenkrip.Add("status", "1");
                resenkrip.Add("message", "Tambah Data Kategori Success");
                //resenkrip["data"] = dataResult;
            }
            catch (Exception e)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Tambah Data Kategori Gagal");
            }

         

            return resenkrip;
        }
        public Dictionary<string, object> Delete_data(Kategori request)
        {


         

                var resenkrip = new Dictionary<string, object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                {
                    List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                    SqlCommand cmd = new SqlCommand("DELETE FROM md_kategori_area WHERE kat_id = @id ", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", request.id);
                    cmd.ExecuteNonQuery();
                    //cmd.Parameters.AddWithValue("@regno", request.regno);
                    //cmd.Parameters.AddWithValue("@type", request.type);
                    con.Close();
                }
                resenkrip.Add("status", "1");
                resenkrip.Add("message", "Hapus Data Kategori Success");
            }
            catch (Exception e)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Hapus Data Kategori Gagal");
            }
            
            return resenkrip;
        }
        public Dictionary<string, object> Get_kategori(Kategori request)
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
                    filter = "kategori_name";
                }

                qfilter += " AND " + filter + " = '" + objJson["value"] + "' ";
            }

            var dump = qfilter;


            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {
                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                string querys = String.Format("SELECT kat_id AS id,kategori_name name,kategori_area_sts as status " +
                    "FROM md_kategori_area WITH(NOLOCK) where 1=1 {0}  ORDER BY kat_id OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY ", qfilter, request.index, request.perpage);
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
        public Dictionary<string, object> Update_Data_katgeori(Kategori request)
        {
            var resenkrip = new Dictionary<string, object>();
            var resError = new Dictionary<string, object>();
            try
            {
                for (int i = 0; i < request.data?.Count; i++)
                {
                    //dynamic objJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.field[i].ToJson());
                    dynamic objJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(request.data[i], Formatting.Indented));

                    string data = objJson["kategoriName"].ToString();

                    if (data == "")
                    {
                        resError["error"] = "data tidak boleh kosong !!";
                        resenkrip.Add("status", "0");
                        resenkrip.Add("message", "Update Data Kategori Gagal");
                        resenkrip["data"] = resError;
                        return resenkrip;
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
                        {

                            SqlCommand cmd1 = new SqlCommand(" select top 1 1 from md_kategori_area where kategori_name = @kategori " +
                             "", con);
                            con.Open();
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Parameters.AddWithValue("@kategori", data);
                            SqlDataAdapter adpt = new SqlDataAdapter(cmd1);
                            DataTable dt = new DataTable();
                            adpt.Fill(dt);
                            con.Close();

                            if (dt.Rows.Count == 1)
                            {
                                resError["error"] = "data tidak boleh sama !!";
                                resenkrip.Add("status", "0");
                                resenkrip.Add("message", "Tambah Data Kategori Gagal");
                                resenkrip["data"] = resError;
                                return resenkrip;
                            }
                            else
                            {

                                List<Dictionary<string, object>> dataResult = new List<Dictionary<string, object>>();
                                SqlCommand cmd = new SqlCommand("UPDATE md_kategori_area SET kategori_name = @kategori where kat_id=@id ", con);
                                con.Open();
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@kategori", data);
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
                resenkrip.Add("message", "Update Data Kategori Sukses");
            }
            catch (Exception e)
            {
                resenkrip.Add("status", "0");
                resenkrip.Add("message", "Update Data Kategori Gagal");
            }
           
            return resenkrip;
        }

    }
}
