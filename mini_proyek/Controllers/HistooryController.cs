using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mini_proyek.Interfaces;
using mini_proyek.Models;

namespace mini_proyek.Controllers
{
  
    [ApiController]
    public class HistooryController : ControllerBase
    {
        private HistoryInterface _historyInterface;

        public HistooryController(HistoryInterface areaInterfaces)
        {
            _historyInterface = areaInterfaces;
        }

        [Route("services/getHistory")]
        [HttpPost]
        public IActionResult GetData(History request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _historyInterface.Get_data_Hostory(request);//, headerDevice,headerVersion);

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

        [Route("services/getHistoryByUser")]
        [HttpPost]
        public IActionResult GetDataByUser(History request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _historyInterface.Get_data_Hostor_by_idy(request);//, headerDevice,headerVersion);

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
