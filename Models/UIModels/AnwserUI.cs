﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class AnwserUI
    {
        public int Id { get; set; }
        public string AnwserText { get; set; }
        public int CompId { get; set; }
        public int AnwserModuleId { get; set; }

    }
}