using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EntityManager:Singleton<EntityManager>
    {
        private int index = 0;
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapid, Entity entity)
        {
            AllEntities.Add(entity);
            entity.EntityData.Id = ++this.index;

            List<Entity> entities = null;
            if (!MapEntities.TryGetValue(mapid, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapid] = entities;
            }
            entities.Add(entity);
        }
        public void RemoveEntity(int mapid, Entity entity)
        {
            this.AllEntities.Remove(entity);
            this.MapEntities[mapid].Remove(entity);
        }
    }
}
