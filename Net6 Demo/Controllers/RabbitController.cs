using Microsoft.AspNetCore.Mvc;
using Net6_Demo.Helpers;

namespace Net6_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly RabbitMQHelper _mq;
        public RabbitController(RabbitMQHelper mq)
        {
            _mq = mq;
        }

        [HttpGet]
        public dynamic Index()
        {
            return null;
        }
    }
}
