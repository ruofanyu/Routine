using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.API.Entities;
using Routine.API.Models;

namespace Routine.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id",new PropertyMappingValue(new List<string>{"Id"} )},
            {"CompanyId",new PropertyMappingValue(new List<string>{"ComapntId"} )},
            {"EmployeeNo",new PropertyMappingValue(new List<string>{ "EmployeeNo" } )},
            {"Name",new PropertyMappingValue(new List<string>{ "FirstName", "LastName" } )},
            {"GenderDisplayer",new PropertyMappingValue(new List<string>{ "Gender" } )},
            {"Age",new PropertyMappingValue(new List<string>{ "DateOfbirth" } ,true)},
        };


        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }

            //下面两种时一样的写法
            throw new Exception($"无法找到唯一的映射关系{typeof(TSource)},{typeof(TDestination)}");
            //throw new Exception(string.Format("无法找到唯一的映射关系{0},{1}",typeof(TSource),typeof(TDestination)));
        }
    }
}
