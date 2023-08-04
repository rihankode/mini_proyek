using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using System.Security.Cryptography.Xml;

namespace mini_proyek.Controllers
{
    
    [ApiController]
    public class AreaController : ControllerBase
    {
        private AreaInterfaces _areaInterfaces;

        public AreaController(AreaInterfaces areaInterfaces)
        {
            _areaInterfaces = areaInterfaces;
        }

        [Microsoft.AspNetCore.Mvc.Route("services/getArea")]
        [HttpPost]
        public IActionResult GetData(Area request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _areaInterfaces.Get_data_Area(request);//, headerDevice,headerVersion);

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

        [Route("services/createArea")]
        [HttpPost]
        public IActionResult CreateData(Area request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _areaInterfaces.Create_data_area(request);//, headerDevice,headerVersion);

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

        [Route("services/updateArea")]
        [HttpPost]
        public IActionResult updateData(Area request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _areaInterfaces.Update_Data_area(request);//, headerDevice,headerVersion);

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

        [Route("services/deleteArea")]
        [HttpPost]
        public IActionResult deleteData(Area request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _areaInterfaces.Delete_area(request);//, headerDevice,headerVersion);

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

        [Route("services/getAreaById")]
        [HttpPost]
        public IActionResult getdataById(Area request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _areaInterfaces.getDataById(request);//, headerDevice,headerVersion);

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
