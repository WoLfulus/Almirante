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
    using System.Collections.Generic;
    using System.Diagnostics;
    using Almirante.Entities.Filters;

    /// <summary>
    /// Entity processor class.
    /// </summary>
    public abstract class EntityProcessor : EntitySystem
    {
        /// <summary>
        /// Stores all active entities.
        /// </summary>
        protected Dictionary<ulong, Entity> entities;

        /// <summary>
        /// Instance of the entity filtering class.
        /// </summary>
        protected Filter filter = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityProcessor"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public EntityProcessor(Filter filter)
            : base()
        {
            this.entities = new Dictionary<ulong, Entity>();
            this.filter = filter;
        }

        /// <summary>
        /// Gets the entities that has been filtered by this processor.
        /// </summary>
        /// <returns>Entity instances.</returns>
        protected IEnumerable<KeyValuePair<ulong, Entity>> GetEntities()
        {
            return this.entities;
        }

        /// <summary>
        /// Applies the system filter to an entity and adds to the system if succeed.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not broken.
        /// </returns>
        internal bool RegisterEntity(Entity entity)
        {
            if (this.filter.Apply(entity))
            {
                this.entities[entity.Id] = entity;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an entity from this system.
        /// </summary>
        /// <param name="entity">Entity's instance.</param>
        /// <returns>
        /// True if not broken.
        /// </returns>
        internal bool RemoveEntity(Entity entity)
        {
            return this.entities.Remove(entity.Id);
        }

        /// <summary>
        /// Clears all entities from the system.
        /// </summary>
        internal void ClearEntities()
        {
            this.entities.Clear();
        }

        /// <summary>
        /// Executes the current system.
        /// </summary>
        protected override void OnExecute(double time)
        {
            foreach (var entity in this.entities.Values)
            {
                if (!entity.Dead)
                {
                    this.Process(entity);
                }
            }
        }

        /// <summary>
        /// Processes an entity that matches the system requeriments.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected abstract void Process(Entity entity);
    }
}