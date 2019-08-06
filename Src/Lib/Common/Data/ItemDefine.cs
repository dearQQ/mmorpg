using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class ItemDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set;}
        public int UseCD { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public string Function { get; set; }
        public string Description { get; set; }
    }
}
