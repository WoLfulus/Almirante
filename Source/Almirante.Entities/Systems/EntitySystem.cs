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
    using Almirante.Entities.Types;

    /// <summary>
    /// Entity system class.
    /// </summary>
    public abstract class EntitySystem
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TypeInfo Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// System manager instance.
        /// </summary>
        public SystemManager Systems
        {
            get;
            internal set;
        }

        /// <summary>
        /// System manager instance.
        /// </summary>
        internal EntityManager manager;

        /// <summary>
        /// Constructor of the system class.
        /// </summary>
        public EntitySystem()
        {
            this.Type = SystemHelper.GetInfo(this.GetType()).Value;
        }

        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity instance.</returns>
        protected Entity GetEntityById(ulong id)
        {
            return this.manager.GetEntity(id);
        }

        /// <summary>
        /// Gets an entity by its tag name.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Entity instance.</returns>
        protected Entity GetEntityByTag(string tag)
        {
            return this.manager.GetEntity(tag);
        }

        /// <summary>
        /// Gets all entities that belongs to the specified group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>Enumeration of the entities.</returns>
        protected IEnumerable<KeyValuePair<ulong, Entity>> GetEntitiesByGroup(string group)
        {
            return this.manager.GetEntities(group);
        }

        /// <summary>
        /// Executes the current system.
        /// </summary>
        internal void Execute(double time)
        {
            if (this.CanExecute(time) == false)
            {
                return;
            }

            this.OnExecute(time);
        }

        /// <summary>
        /// Called when the system executes.
        /// </summary>
        protected abstract void OnExecute(double time);

        /// <summary>
        /// Checks if the current system can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanExecute(double time)
        {
            return true;
        }
    }
}