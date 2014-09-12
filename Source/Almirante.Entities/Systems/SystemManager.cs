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

namespace Almirante.Entities.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// System management class.
    /// </summary>
    public sealed class SystemManager
    {
        /// <summary>
        /// Entity manager instance.
        /// </summary>
        private EntityManager entities;

        /// <summary>
        /// Stores a list of systems.
        /// </summary>
        private Dictionary<ulong, EntitySystem> systems;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemManager"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public SystemManager(EntityManager manager)
        {
            this.entities = manager;
            this.systems = new Dictionary<ulong, EntitySystem>();
        }

        /// <summary>
        /// Adds a new system to the manager.
        /// </summary>
        /// <typeparam name="T">System type.</typeparam>
        public void Add<T>()
            where T : EntitySystem, new()
        {
            this.Add(new T());
        }

        /// <summary>
        /// Adds a new system to the manager.
        /// </summary>
        /// <param name="system">System instance.</param>
        public void Add(EntitySystem system)
        {
            system.Systems = this;
            system.manager = this.entities;
            if (this.systems.ContainsKey(system.Type.Id))
            {
                throw new Exception("You cannot more than one system of the same type. This system already registered.");
            }
            this.systems.Add(system.Type.Id, system);
        }

        /// <summary>
        /// Gets a system from the manager.
        /// </summary>
        /// <typeparam name="T">System type.</typeparam>
        /// <returns></returns>
        public EntitySystem Get<T>()
            where T : EntitySystem
        {
            var info = SystemHelper.GetInfo(typeof(T));
            if (info.HasValue)
            {
                return this.systems[info.Value.Id];
            }

            return null;
        }

        /// <summary>
        /// Gets all systems from the manager.
        /// </summary>
        /// <returns>List with all systems.</returns>
        public EntitySystem[] GetAll()
        {
            return this.systems.Values.ToArray();
        }

        /// <summary>
        /// Remove a specific system from the manager.
        /// </summary>
        /// <typeparam name="T">System type.</typeparam>
        public void Remove<T>()
            where T : EntitySystem
        {
            var type = SystemHelper.GetInfo(typeof(T));
            if (type.HasValue)
            {
                this.Remove(type.Value.Id);
            }
        }

        /// <summary>
        /// Removes the system.
        /// </summary>
        /// <param name="id">The id.</param>
        internal void Remove(ulong id)
        {
            this.systems.Remove(id);
        }

        /// <summary>
        /// Adds an entity to the systems.
        /// </summary>
        /// <param name="entity">Entity instance,</param>
        internal void RegisterEntity(Entity entity)
        {
            foreach (var system in this.systems)
            {
                var processor = system.Value as EntityProcessor;
                if (processor != null)
                {
                    processor.RegisterEntity(entity);
                }
            }
        }

        /// <summary>
        /// Removes an entity from the systems.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        internal void RemoveEntity(Entity entity)
        {
            foreach (var system in this.systems)
            {
                var processor = system.Value as EntityProcessor;
                if (processor != null)
                {
                    processor.RemoveEntity(entity);
                }
            }
        }

        /// <summary>
        /// Clears all entities from the systems.
        /// </summary>
        internal void ClearEntities()
        {
            foreach (var system in this.systems)
            {
                var processor = system.Value as EntityProcessor;
                if (processor != null)
                {
                    processor.ClearEntities();
                }
            }
        }

        /// <summary>
        /// Executes the registered systems.
        /// </summary>
        internal void Execute(double time)
        {
            foreach (var system in this.systems)
            {
                system.Value.Execute(time);
            }
        }
    }
}