using Dapper;
using Dapper_Example.Contact;
using Dapper_Example.Context;
using Dapper_Example.DTO;
using Dapper_Example.Entities;

namespace Dapper_Example.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DapperContext _context;
        public CompanyRepository(DapperContext context) => _context = context;

        public async Task<Company> CreateCompany(CompanyCreation company)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, System.Data.DbType.String);
            parameters.Add("Address", company.Address, System.Data.DbType.String);
            parameters.Add("Country", company.Country, System.Data.DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country,
                };
                return createdCompany;
            }

        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "SELECT * FROM Companies";
            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }

        }

        public async Task<IEnumerable<Company>> GetCompany(int id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @id";

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryAsync<Company>(query, new { id });
                return company.ToList();
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            var procedureName = "SelectCompanyByEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                    (procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return company;
            }
        }
    }
}
