﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRebot.GenerateEntityTools.Entity
{
    public class DataBase
    {

        public string DataBaseName { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();

    }
}
