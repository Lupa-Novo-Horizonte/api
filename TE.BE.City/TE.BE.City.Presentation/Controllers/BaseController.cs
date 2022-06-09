using Microsoft.AspNetCore.Mvc;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;

namespace TE.BE.City.Presentation.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ActionResult Response(bool success, object result)
        {
            if (success)
            {
                return Ok(
                        new
                        {
                            success = success,
                            data = result
                        });
            }
            else
            {
                return BadRequest(
                        new
                        {
                            success = success,
                            errors = result
                        });
            }
        }
    }
}
