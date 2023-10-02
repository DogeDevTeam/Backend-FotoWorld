using FotoWorldBackend.Models.UserModels;
using FotoWorldBackend.Services.UserS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FotoWorldBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [Route("offers")]
        [HttpGet]
        public IActionResult GetOffers() 
        { 
            var ret = _userService.GetOfferList();
            if(ret!=null)
            {
                return Ok(ret);
            }
            return NotFound();
        }

        [Route("get-offer-detailed/{id}")]
        [Authorize(Roles = "Operator, User")]
        [Consumes("multipart/form-data", "application/json")]
        [HttpGet]
        public IActionResult GetOfferDetailed([FromRoute] int id)
        {
            var ret = _userService.GetOfferDetailed(id);
            return Ok(ret);

        }



        [Route("follow-offer/{offerId}")]
        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [Authorize(Roles = "Operator, User")]
        public IActionResult FollowOffer([FromRoute] int offerId)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(o => o.Type == "id").Value;

            var res = _userService.FollowOffer(offerId, userId);

            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("unfollow-offer/{offerId}")]
        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [Authorize(Roles = "Operator, User")]
        public IActionResult UnfollowOffer([FromRoute]int offerId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(o => o.Type == "id").Value;

            var res = _userService.UnfollowOffer(offerId, userId);

            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }
        //A jakiś mix do zalewania Passata b5 fl na sandomierskim Orlenie? Znam twojego fana, potrzebuje playlisty do dolewania oleju.
        [Route("create-opinion/{offerId}")]
        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [Authorize(Roles = "Operator, User")]
        public IActionResult CreateOpinion([FromRoute] int offerId, [FromForm] CreateOperatorOpinionModel opinion)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(o => o.Type == "id").Value;

            var res = _userService.CreateOperatorOpinion(opinion,offerId, userId);

            if(res)
            {
                return Ok(opinion);
            }
            return BadRequest();

        }

        [Route("remove-opinion/{offerId}")]
        [HttpPost]
        [Consumes("multipart/form-data", "application/json")]
        [Authorize(Roles = "Operator, User")]
        public IActionResult RemoveOpinion([FromRoute] int offerId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(o => o.Type == "id").Value;

            var res = _userService.RemoveOperatorOpinion(offerId, userId);

            if (res)
            {
                return Ok();
            }
            return BadRequest();
        }


        [Route("get-image/{id}")]
        [HttpGet]
        public IActionResult GetImage(int id) { 
        
            return File(_userService.GetImageById(id), "image/*");
        }
    }
}
