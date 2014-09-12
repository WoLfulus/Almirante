/// 
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 

namespace Almirante.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Almirante.Entities.Components;
    using Almirante.Entities.Systems;

    /// <summary>
    /// Entity management class.
    /// </summary>
    public sealed class EntityManager
    {
        /// <summary>
        /// Current entity id.
        /// </summary>
        private ulong id = 0;

        /// <summary>
        /// Stores if the entity manager has been initialized.
        /// </summary>
        internal static bool initialized = false;

        /// <summary>
        /// Empty list of entities.
        /// </summary>
        private readonly List<Entity> emptyList;

        /// <summary>
        /// Stores the unused entity ids.
        /// </summary>
        private Queue<Entity> addedEntities;

        /// <summary>
        /// Stores a list of entities.
        /// </summary>
        private Dictionary<ulong, Entity> entities;

        /// <summary>
        /// Stores a list of entities.
        /// </summary>
        private Dictionary<Type, Dictionary<ulong, Entity>> entitiesByType;

        /// <summary>
        /// The entities by group.
        /// </summary>
        private Dictionary<string, Dictionary<ulong, Entity>> entitiesByGroup;

        /// <summary>
        /// Stores a list of entities.
        /// </summary>
        private Dictionary<string, Entity> entitiesByTag;

        /// <summary>
        /// Stores a list of dead entities.
        /// </summary>
        private List<Entity> deadEntities;

        /// <summary>
        /// System management class.
        /// </summary>
        public SystemManager Systems
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityManager()
        {
            this.emptyList = new List<Entity>();
            this.addedEntities = new Queue<Entity>();
            this.entities = new Dictionary<ulong, Entity>();
            this.entitiesByType = new Dictionary<Type, Dictionary<ulong, Entity>>();
            this.entitiesByTag = new Dictionary<string, Entity>();
            this.entitiesByGroup = new Dictionary<string, Dictionary<ulong, Entity>>();
            this.deadEntities = new List<Entity>();
            this.Systems = new SystemManager(this);
        }

        /// <summary>
        /// Initializes the entity manager.
        /// </summary>
        internal static void Initialize()
        {
            if (initialized)
            {
                return;
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && type.IsAbstract)
                    {
                        continue;
                    }

                    if (typeof(Component).IsAssignableFrom(type))
                    {
                        ComponentHelper.GetInfo(type);
                    }
                    else if (typeof(EntitySystem).IsAssignableFrom(type))
                    {
                        SystemHelper.GetInfo(type);
                    }
                }
            }

            initialized = true;
        }

        #region Entity methods

        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity instance.</returns>
        internal Entity GetEntity(ulong id)
        {
            Entity entity = null;
            if (this.entities.TryGetValue(id, out entity))
            {
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Gets an entity by its tag name.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Entity instance.</returns>
        internal Entity GetEntity(string tag)
        {
            Entity entity = null;
            if (this.entitiesByTag.TryGetValue(tag, out entity))
            {
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Gets all entities that belongs to the specified group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>Enumeration of the entities.</returns>
        internal IEnumerable<KeyValuePair<ulong, Entity>> GetEntities(string group)
        {
            Dictionary<ulong, Entity> dictionary = null;
            if (this.entitiesByGroup.TryGetValue(group, out dictionary))
            {
                return dictionary;
            }

            return null;
        }

        #endregion Entity methods

        /// <summary>
        /// Updates the entity manager.
        /// </summary>
        /// <param name="time">The time elapsed time in <c>seconds</c>.</param>
        public void Update(double time)
        {
            while (this.addedEntities.Count > 0)
            {
                Entity entity = this.addedEntities.Dequeue();
                entity.Create();

                this.entities.Add(entity.Id, entity);

                Dictionary<ulong, Entity> dict = null;
                if (this.entitiesByType.TryGetValue(entity.GetType(), out dict))
                {
                    dict.Add(entity.Id, entity);
                }
                else
                {
                    dict = new Dictionary<ulong, Entity>();
                    dict.Add(entity.Id, entity);
                    this.entitiesByType.Add(entity.GetType(), dict);
                }

                if (entity.Group != null)
                {
                    Dictionary<ulong, Entity> dict2 = null;
                    if (this.entitiesByGroup.TryGetValue(entity.Group, out dict2))
                    {
                        dict2.Add(entity.Id, entity);
                    }
                    else
                    {
                        dict = new Dictionary<ulong, Entity>();
                        dict.Add(entity.Id, entity);
                        this.entitiesByGroup.Add(entity.Group, dict);
                    }
                }

                if (entity.Tag != null)
                {
                    if (this.entitiesByTag.ContainsKey(entity.Tag))
                    {
                        throw new Exception("Duplicated entity tag.");
                    }
                    else
                    {
                        this.entitiesByTag.Add(entity.Tag, entity);
                    }
                }

                this.Systems.RegisterEntity(entity);
            }

            this.Systems.Execute(time);

            foreach (var entity in this.deadEntities)
            {
                entity.Destroy();
                this.RemoveEntity(entity);
            }

            this.deadEntities.Clear();
        }

        /// <summary>
        /// Draws the entities.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void Draw()
        {
            foreach (var entity in this.entities)
            {
                if (!entity.Value.Dead)
                {
                    entity.Value.Draw();
                }
            }
        }

        /// <summary>
        /// Creates a free entity id.
        /// </summary>
        /// <returns>New entity id.</returns>
        private ulong CreateId()
        {
            lock (this)
            {
                return this.id++;
            }
        }

        /// <summary>
        /// Creates a determined entity info.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="tag">The tag.</param>
        /// <param name="group">The group.</param>
        /// <returns>
        /// New entity instance.
        /// </returns>
        public T Create<T>(string tag = null, string group = null)
            where T : Entity, new()
        {
            T entity = new T();
            entity.Id = this.CreateId();
            entity.Tag = tag;
            entity.Group = group;
            entity.Manager = this;

            ComponentHelper.CreateComponents(entity);

            this.addedEntities.Enqueue(entity);
            return entity;
        }

        /// <summary>
        /// Removes the specified entity from the manager
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal bool RemoveEntity(Entity entity)
        {
            lock (this)
            {
                ulong id = entity.Id;
                if (this.entities.Remove(id))
                {
                    return true;
                }

                Dictionary<ulong, Entity> dict;
                if (this.entitiesByType.TryGetValue(entity.GetType(), out dict))
                {
                    dict.Remove(entity.Id);
                }

                if (entity.Group != null)
                {
                    Dictionary<ulong, Entity> dict2;
                    if (this.entitiesByGroup.TryGetValue(entity.Group, out dict2))
                    {
                        dict2.Remove(entity.Id);
                    }
                }

                if (entity.Tag != null)
                {
                    this.entitiesByTag.Remove(entity.Tag);
                }
            }

            return false;
        }

        /// <summary>
        /// Kills an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        internal void KillEntity(Entity entity)
        {
            this.deadEntities.Add(entity);
        }

        /// <summary>
        /// Clears all entities from the entity manager.
        /// </summary>
        public void Clear()
        {
            foreach (var entity in this.entities)
            {
                entity.Value.Destroy();
            }

            this.entities.Clear();
            this.entitiesByType.Clear();
            this.entitiesByGroup.Clear();
            this.entitiesByTag.Clear();
            this.addedEntities.Clear();
            this.deadEntities.Clear();
            this.Systems.ClearEntities();

            this.id = 0;
        }
    }
}