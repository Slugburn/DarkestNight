using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Slugburn.DarkestNight.Web.Controllers
{
    [RoutePrefix("api/hero")]
    public class HeroController : ApiController
    {
        [Route]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[]
            {
                "Acolyte",
                "Druid",
                "Knight",
                "Priest",
                "Prince",
                "Rogue",
                "Scholar",
                "Seer",
                "Wizard"
            };
        }
    }
}
