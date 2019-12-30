using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public ItemManager(Character owner)
        {
            Owner = owner;
            foreach (var item in owner.Data.Items)
            {
                this.Items.Add(item.ItemID, new Item(item));
            }
        }

        public bool UseItem(int itemId, int count = 1)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                if (item.Count < count)
                    return false;
                item.Remove(count);
                Log.InfoFormat("UseItem  id = {0},count = {1}", itemId, count);
                return true;
            }
            return false;
        }


        public bool HasItem(int itemId)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                return item.Count > 0;
            }
            return false;
        }

        public Item GetItem(int itemId)
        {
            return this.Items[itemId];
        }

        public bool AddItem(int itemId, int count)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.TCharacterID = Owner.Data.ID;
                dbItem.ItemID = itemId;
                dbItem.Owner = Owner.Data;
                dbItem.ItemCount = count;
                Owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                this.Items.Add(itemId, item);
                Log.InfoFormat("AddItem  id = {0},count = {1}", itemId, count);
            }
            DBService.Instance.Save();
            return true;
        }

        public bool RemoveItem(int itemId, int count)
        {
            if (!this.Items.ContainsKey(itemId))
            {
                return false;
            }
            Item item = this.Items[itemId];
            if (item.Count < count)
                return false;
            Log.InfoFormat("RemoveItem  id = {0},count = {1}",itemId,count);
            item.Remove(count);
            DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in Items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.Count });
            }
        }

    }
}
