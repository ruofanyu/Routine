﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Routine.API.DtoParameters
{
    public class EmployeeDtoParameters
    {
        private const int MaxPageSize = 20;
        public string Gender { get; set; }
        public string Q { get; set; }
        public int PageNumber { get; set; } = 1;
        public int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderBy { get; set; } = "name";
    }
}
