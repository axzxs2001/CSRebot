﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRobot.GenerateEntityTools.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Field
    {
        /// <summary>
        /// 
        /// </summary>
        public string FieldName
        { get; set; }
        public string FieldDescribe { get; set; }

        public string DBType { get; set; }

        public long? FieldSize { get; set; }
    }
}
