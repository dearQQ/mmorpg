using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }
    class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        Dictionary<int, IEntityNotify> notifies = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            this.notifies[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntiy(NEntity nentity)
        {
            this.entities.Remove(nentity.Id);
            if (notifies.ContainsKey(nentity.Id))
            {
                notifies[nentity.Id].OnEntityRemoved();
                notifies.Remove(nentity.Id);
            }
        }

        public void OnEntitySync(NEntitySync entitySync)
        {
            Entity entity = null;
            this.entities.TryGetValue(entitySync.Id, out entity);
            if (entity != null)
            {
                if (entitySync.Entity != null)
                {
                    entity.EntityData = entitySync.Entity;
                }
                if (notifies.ContainsKey(entitySync.Id))
                {
                    notifies[entitySync.Id].OnEntityChanged(entity);
                    notifies[entitySync.Id].OnEntityEvent(entitySync.Event);
                }
            }
        }

    }
}
