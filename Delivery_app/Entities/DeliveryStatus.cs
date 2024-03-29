﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public enum DeliveryStatus : byte
    {
        [Display(Name = "Assigned")]
        Assigned,
        [Display(Name = "OnDeliver")]
        OnDeliver,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Cancelled")]
        Cancelled
    }
}
