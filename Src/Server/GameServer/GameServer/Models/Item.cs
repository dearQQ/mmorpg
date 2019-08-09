using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Item
    {
        TCharacterItem dbItem;
        public int ItemID;
        public int Count;
        public Item(TCharacterItem item)
        {
            dbItem = item;
            ItemID = item.ItemID;
            Count = item.ItemCount;
        }

        public void Add(int count)
        {
            this.Count += count;
            this.dbItem.ItemCount = this.Count;
        }

        public void Remove(int count)
        {
            this.Count -= count;
            this.dbItem.ItemCount = this.Count;
        }
    }
}
