﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Services.Models
{
    public abstract class BaseRS<TModel>
    {
        public virtual TModel Result { get; set; }
    }
}
