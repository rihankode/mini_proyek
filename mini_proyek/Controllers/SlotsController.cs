using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mini_proyek.Interfaces;
using mini_proyek.Models;

namespace mini_proyek.Controllers
{
 
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private SlotInterface _slotinterface;

        public SlotsController(SlotInterface slotInterface)
        {
            _slotinterface = slotInterface;
        }

        [Route("services/checkin")]
        [HttpPost]
        public IActionResult Checkin(Slots request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _slotinterface.get_availibility(request);//, headerDevice,headerVersion);

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

        [Route("services/getSlot")]
        [HttpPost]
        public IActionResult getSlot(Slots request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _slotinterface.get_slot(request);//, headerDevice,headerVersion);

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

        [Route("services/checkout")]
        [HttpPost]
        public IActionResult Checkout(Slots request)
        {
            try
            {
                //string headerDevice = Request.Headers["Device"].ToString();
                //string headerVersion = Request.Headers["Version"].ToString();
                var result = _slotinterface.get_checkout(request);//, headerDevice,headerVersion);

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
