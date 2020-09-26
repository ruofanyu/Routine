using Microsoft.AspNetCore.Mvc;
using Routine.API.Services;
using System;
using System.Threading.Tasks;

namespace Routine.API.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet("api/companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepository.GetCompaniesAsync();
            return new JsonResult(companies);
        }

        [HttpGet("api/companies/{companyId}")]
        public async Task<IActionResult> GetCompanies(Guid companyId)
        {
            var companies = await _companyRepository.GetCompanyAsync(companyId);
            return Ok(companies);
        }
    }
}
