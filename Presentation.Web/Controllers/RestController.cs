using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DTO = Application.DTO;
using Application.Services;
using Enums = Domain.Infrastructure;
using Infrastructure.Logging;
using System.Web.Script.Serialization;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// RestController
    /// </summary>
    [RoutePrefix("api/sgde")]
    public class RestController : ApiController
    {
        /// <summary>
        /// ping
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult ping()
        {
            return Ok(true);
        }
    }
}
