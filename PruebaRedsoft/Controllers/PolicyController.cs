using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaRedsoft.Models;
using PruebaRedsoft.Services;
using System.Security.Claims;

namespace PruebaRedsoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService policyService;
        private readonly IJwtService jwtService;

        public PolicyController(IPolicyService policyService, IJwtService jwtService) {
            this.policyService = policyService;
            this.jwtService = jwtService;
        }

        [HttpGet]
        public ActionResult<List<Policy>> Get()
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success)
            {
                return NotFound("Error en la autenticacion");
            }

            return policyService.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<Policy> Get(string id)
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success)
            {
                return NotFound("Error en la autenticacion");
            }

            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id}");
            }
            return existingPolicy;
        }

        [HttpPost]
        public ActionResult<Policy> Post([FromBody] Policy policy)
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success)
            {
                return NotFound("Error en la autenticacion");
            }

            policyService.Create(policy);

            return CreatedAtAction(nameof(Get), new { id = policy.Id }, policy);
        }

        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Policy policy)
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success)
            {
                return NotFound("Error en la autenticacion");
            }

            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id}");
            }

            policyService.Update(id, policy);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success) {
                return NotFound("Error en la autenticacion");
            }

            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id} no existe");
            }

            policyService.Delete(id);

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<Policy>> GetPolicy(int policyNumber, string licensePlate)
        {
            ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic resultToken = jwtService.ValidateToken(indentity);

            if (!resultToken.success)
            {
                return NotFound("Error en la autenticacion");
            }

            var policy = await policyService.GetByPolicyNumberOrLicensePlate(policyNumber, licensePlate);

            if (policy == null)
            {
                return NotFound();
            }

            return policy;
        }

    }
}
