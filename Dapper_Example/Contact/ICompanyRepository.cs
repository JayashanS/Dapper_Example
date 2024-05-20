using Dapper_Example.DTO;

namespace Dapper_Example.Contact
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Entities.Company>> GetCompanies();
        public Task<IEnumerable<Entities.Company>> GetCompany(int id);
        public Task<Entities.Company> CreateCompany(CompanyCreation company);
        public Task<Entities.Company> GetCompanyByEmployeeId(int id);

    }
}
