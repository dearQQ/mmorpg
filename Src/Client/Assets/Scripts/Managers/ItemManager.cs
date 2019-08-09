using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using SkillBridge.Message;
using Common.Data;
namespace Managers
{
    public class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, ItemDefine> itemDefine = DataManager.Instance.Items;
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);
            }
        }
         

    }
}
