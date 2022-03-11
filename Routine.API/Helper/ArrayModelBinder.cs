using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Routine.API.Helper
{
    /// <summary>
    /// 该类是用于批量获取公司的主键ID(Guid)，并放入IEnumberable集合中
    /// </summary>
    public class ArrayModelBinder : IModelBinder    //继承于IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                //如果绑定的类型不是IEnumerable，则失败
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //成功则获取该主键
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                //如果为空，则表示搜索成功，但值为空，返回空值
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            //如果不为空，则获取该数据类型中的第一个类型（即主键GUID）（因为我们传过来的仅仅只有一个，guid集合）
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            //创建转换器
            var converter = TypeDescriptor.GetConverter(elementType);

            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim())).ToArray();

            //创建实例对象
            var typeValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typeValues, 0);

            //绑定事件
            bindingContext.Model = typeValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
