using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public Item(NItemInfo item)
        {
            Id = item.Id;
            Count = item.Count;
        }
    }
}
