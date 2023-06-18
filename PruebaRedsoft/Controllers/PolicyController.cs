using Microsoft.AspNetCore.Mvc;
using PruebaRedsoft.Models;
using PruebaRedsoft.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaRedsoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService policyService;

        public PolicyController(IPolicyService policyService) {
            this.policyService = policyService;
        }
        // GET: api/<PolicyController>
        [HttpGet]
        public ActionResult<List<Policy>> Get()
        {
            return policyService.Get();
        }

        // GET api/<PolicyController>/5
        [HttpGet("{id}")]
        public ActionResult<Policy> Get(string id)
        {
            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id}");
            }
            return existingPolicy;
        }

        // POST api/<PolicyController>
        [HttpPost]
        public ActionResult<Policy> Post([FromBody] Policy policy)
        {
            policyService.Create(policy);

            return CreatedAtAction(nameof(Get), new { id = policy.Id }, policy);
        }

        // PUT api/<PolicyController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Policy policy)
        {
            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id}");
            }

            policyService.Update(id, policy);

            return NoContent();
        }

        // DELETE api/<PolicyController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var existingPolicy = policyService.Get(id);

            if (existingPolicy == null)
            {
                return NotFound($"Poliza con el id = {id}");
            }

            policyService.Delete(id);

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<Policy>> GetPolicy(int policyNumber, string licensePlate)
        {
            var policy = await policyService.GetByPolicyNumberOrLicensePlate(policyNumber, licensePlate);

            if (policy == null)
            {
                return NotFound();
            }

            return policy;
        }

    }
}
