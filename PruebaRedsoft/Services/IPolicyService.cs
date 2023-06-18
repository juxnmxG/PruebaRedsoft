using PruebaRedsoft.Models;

namespace PruebaRedsoft.Services
{
    public interface IPolicyService
    {
        List<Policy> Get();
        Policy Get(string id);
        Policy Create(Policy policy);
        void Update(string id, Policy policy);
        Task<Policy> GetByPolicyNumberOrLicensePlate(int policyNumber, string plate);
        void Delete(string id);
    }
}
