using Microsoft.EntityFrameworkCore;
using Routine.API.Data;
using Routine.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Routine.API.DtoParameters;
using Routine.API.Helper;
using Routine.API.Models;

namespace Routine.API.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        public readonly RoutineDbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        //构造函数
        public CompanyRepository(RoutineDbContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService)); ;
        }

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="company"></param>
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }

            _context.Companies.Add(company);
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employee"></param>
        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        /// <summary>
        /// 判断公司是否存在
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.AnyAsync(predicate: x => x.Id == companyId);
        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="company"></param>
        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Remove(company);
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="employee"></param>
        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<PageList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            //if (string.IsNullOrWhiteSpace(parameters.CompanyName) && string.IsNullOrWhiteSpace(parameters.SearchTerm))
            //{
            //    return await _context.Companies.ToListAsync();
            //}
            var queryExpression = _context.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm) || x.Introduction.Contains(parameters.SearchTerm));
            }

            //queryExpression = queryExpression.Skip(parameters.PageSize * (parameters.PageIndex - 1))
            //    .Take(parameters.PageSize);
            //return await queryExpression.ToListAsync();
            return await PageList<Company>.CreateAsync(queryExpression, parameters.PageIndex, parameters.PageSize);

        }

        /// <summary>
        /// 通过主键获取具体一系列公司
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _context.Companies.Where(x => companyIds.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// 获取具体某一个公司
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(predicate: x => x.Id == companyId);
        }

        /// <summary>
        /// 获取公司下某一个员工
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employeeId == null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _context.Employees.Where(x => x.CompanyId == companyId && x.Id == employeeId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取某一个公司的所有员工
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="genderDisplayer"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplayer, string search)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (string.IsNullOrWhiteSpace(genderDisplayer) && string.IsNullOrWhiteSpace(search))
            {
                return await _context.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.EmployeeNo).ToListAsync();
            }
            var items = _context.Employees.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(genderDisplayer))
            {
                genderDisplayer = genderDisplayer.Trim();
                var gender = Enum.Parse<Gender>(genderDisplayer);
                items = items.Where(x => x.Gender == gender);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                items = items.Where(x => x.EmployeeNo.Contains(search) || x.FirstName.Contains(search) || x.LastName.Contains(search));
            }
            return await items.OrderBy(x => x.EmployeeNo).ToListAsync();
        }


        /// <summary>
        /// 获取某一个公司的所有员工
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="genderDisplayer"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeDtoParameters parameters)
        {
            if (companyId == null)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            var items = _context.Employees.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                parameters.Gender = parameters.Gender.Trim();
                var gender = Enum.Parse<Gender>(parameters.Gender);
                items = items.Where(x => x.Gender == gender);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                items = items.Where(x => x.EmployeeNo.Contains(parameters.Q) || x.FirstName.Contains(parameters.Q) || x.LastName.Contains(parameters.Q));
            }
            //if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            //{
            //    if (parameters.OrderBy.ToLower() == "name")
            //    {
            //        items = items.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            //    }
            //}
            var mappingDictionary = _propertyMappingService.GetPropertyMapping<EmployeeDto, Employee>();
            items = items.ApplySort(parameters.OrderBy, mappingDictionary);
            return await items.ToArrayAsync();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        /// <summary>
        /// 更新公司属性
        /// </summary>
        /// <param name="company"></param>
        public void UpdateCompany(Company company)
        {
            //_context.Entry(company).State = EntityState.Modified;
        }

        /// <summary>
        /// 更新员工属性
        /// </summary>
        /// <param name="employee"></param>
        public void UpdateEmployee(Employee employee)
        {
            //throw new NotImplementedException();
        }
    }
}
