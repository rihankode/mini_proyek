using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using System.Security.Cryptography.Xml;

namespace mini_proyek.Controllers
{
    [ApiController]
    public class KategoriController : ControllerBase
    {
        private KategoriInterfaces _kategoriInterfaces;

       public KategoriController(KategoriInterfaces kategoriInterfaces) {
            _kategoriInterfaces = kategoriInterfaces;
        }

        [Route("services/getData")]
        [HttpPost]
        public IActionResult GetData(Kategori request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _kategoriInterfaces.Get_kategori(request);//, headerDevice,headerVersion);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {

                    status = "0",
                    //message = "Get Data Failed",
                    message = e.Message.ToString(),
                });
            }
        }

        [Route("services/createData")]
        [HttpPost]
        public IActionResult CreateData(Kategori request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _kategoriInterfaces.Create_data_kategori(request);//, headerDevice,headerVersion);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {

                    status = "0",
                    //message = "Get Data Failed",
                    message = e.Message.ToString(),
                });
            }
        }

        [Route("services/updateData")]
        [HttpPost]
        public IActionResult updateData(Kategori request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _kategoriInterfaces.Update_Data_katgeori(request);//, headerDevice,headerVersion);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {

                    status = "0",
                    //message = "Get Data Failed",
                    message = e.Message.ToString(),
                });
            }
        }

        [Route("services/deleteData")]
        [HttpPost]
        public IActionResult deleteData(Kategori request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _kategoriInterfaces.Delete_data(request);//, headerDevice,headerVersion);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {

                    status = "0",
                    //message = "Get Data Failed",
                    message = e.Message.ToString(),
                });
            }
        }
    }
}
