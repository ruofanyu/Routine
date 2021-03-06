﻿using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.API.DtoParameters;
using Routine.API.Entities;
using Routine.API.Helper;
using Routine.API.Models;
using Routine.API.Services;

namespace Routine.API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }




        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]
        //public async Task<IActionResult> GetCompanies()
        //既可以使用上面的这个方式，也可以使用下面的这个——下面的是使用ActionResult<T>、其中T是IEnumerable<CompanyDto>类型
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] CompanyDtoParameters parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            //生成前一页标签
            var previousPageLike = companies.HasPrevious
                ? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            //生成后一页标签
            var nextPageLinke = companies.HasNext
                ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var pagenationMetadata = new
            {
                totalCount = companies.TotalCount,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPages = companies.TotalPages,
                previousPageLike,
                nextPageLinke
            };

            Response.Headers.Add("X-Pagenation", JsonSerializer.Serialize(pagenationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            if (companies == null)
            {
                return NotFound();
            }

            //进行映射
            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            //var companyDto = new List<CompanyDto>();

            //foreach (var company in companies)
            //{
            //    companyDto.Add(new CompanyDto
            //    {
            //        Id = company.Id,
            //        Name = company.Name
            //    });
            //}
            //return new JsonResult(companies);
            return Ok(companyDto);
        }

        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        //可以把上面的("{companyId})写在下面这样"
        //[Route("{companyId}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            //return Ok(company);
            return Ok(_mapper.Map<CompanyDto>(company));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyAddDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();
            var ReturnDto = _mapper.Map<CompanyDto>(entity);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = ReturnDto.Id }, ReturnDto);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var companyEntity = await _companyRepository.GetCompanyAsync(companyId);
            if (companyEntity == null)
            {
                return NotFound();
            }

            await _companyRepository.GetEmployeesAsync(companyId, null, null);
            _companyRepository.DeleteCompany(companyEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("allow", "GET,POST,OPTIONS,PUT,PATCH");
            return Ok();
        }


        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
            }
        }
    }
}
