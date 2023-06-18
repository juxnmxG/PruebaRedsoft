using PruebaRedsoft.Models;
using MongoDB.Driver;

namespace PruebaRedsoft.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IMongoCollection<Policy> _policys;

        public PolicyService(IPolicyStoreDatabaseSettings settings, IMongoClient mongoClient) {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _policys = database.GetCollection<Policy>(settings.PolicysCollectionName);
        }

        public Policy Create(Policy policy)
        {
            if (policy.DateInit > DateTime.Now || policy.DateEnd < DateTime.Now)
            {
                throw new ApplicationException("La póliza no está en vigencia");
            }

            _policys.InsertOne(policy);

            return policy;
        }

        public void Delete(string id)
        {
            _policys.DeleteOne(policy => policy.Id == id);
        }

        public List<Policy> Get()
        {
            return _policys.Find(policy => true).ToList();
        }

        public Policy Get(string id)
        {
            return _policys.Find(policy => policy.Id == id).FirstOrDefault();
        }

        public void Update(string id, Policy policy)
        {
            _policys.ReplaceOne(policy => policy.Id == id, policy);
        }

        public async Task<Policy> GetByPolicyNumberOrLicensePlate(int policyNumber, string licensePlate)
        {
            FilterDefinition<Policy> filter = Builders<Policy>.Filter.Eq(p => p.NumberPolicy, policyNumber)
                | Builders<Policy>.Filter.Eq(p => p.AutomotivePolicy.Plate, licensePlate);

            return await _policys.Find(filter).FirstOrDefaultAsync();
        }

    }
}
