﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class Attribute
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<AttributeValue> AttributeValues { get; set; }
    }
}