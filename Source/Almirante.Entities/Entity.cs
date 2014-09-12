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
    using System.Numerics;
    using Almirante.Entities.Components;
    using Almirante.Entities.Components.Attributes;
    using Almirante.Entities.Types;

    /// <summary>
    /// Entity class.
    /// </summary>
    public abstract class Entity : IComparable<Entity>
    {
        /// <summary>
        /// Represents an invalid entity (empty).
        /// </summary>
        public const ulong None = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// Gets the entity's id.
        /// </summary>
        public ulong Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the entity info information.
        /// </summary>
        public TypeInfo Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Entity"/> is dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        internal bool Dead
        {
            get;
            set;
        }

        /// <summary>
        /// Component mapper.
        /// </summary>
        public BigInteger ComponentMask
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets entity manager instance that holds this entity.
        /// </summary>
        public EntityManager Manager
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the entity's position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Component]
        public PositionComponent Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group
        {
            get;
            internal set;
        }

        /// <summary>
        /// Stores the components of this entity.
        /// </summary>
        private Component[] components;

        /// <summary>
        /// Stores the components of this entity.
        /// </summary>
        private DrawableComponent[] drawableComponents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            EntityManager.Initialize();

            this.Type = TypeManager<Entity>.GetInfo(this.GetType()).Value;

            this.components = new Component[ComponentHelper.Count];
            this.drawableComponents = new DrawableComponent[ComponentHelper.Count];

            for (ulong i = 0; i < ComponentHelper.Count; i++)
            {
                this.components[i] = null;
                this.drawableComponents[i] = null;
            }

            this.Dead = false;
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="type">The type of the component.</param>
        /// <returns>
        /// The component instance.
        /// </returns>
        /// <exception cref="System.Exception">Component already exists.</exception>
        internal Component AddComponent(Type type)
        {
            var info = ComponentHelper.GetInfo(type);
            if (!info.HasValue)
            {
                throw new Exception("Internal error: component is not registered.");
            }

            ulong id = info.Value.Id;

            if (this.components[id] != null)
            {
                throw new Exception("Component already exists.");
            }

            var component = ComponentHelper.CreateInstance(type);
            component.Owner = this;

            this.components[id] = component;

            var drawable = component as DrawableComponent;
            if (drawable != null)
            {
                this.drawableComponents[id] = drawable;
            }

            this.ComponentMask |= info.Value.Mask;

            return component;
        }

        /// <summary>
        /// Checks if a component exists in the current entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>
        ///   <c>true</c> if this instance has the specified component; otherwise, <c>false</c>.
        /// </returns>
        public bool HasComponent<T>()
            where T : Component
        {
            return this.HasComponent(typeof(T));
        }

        /// <summary>
        /// Checks if a component exists in the current entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the current entity has the specified component; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasComponent(Type type)
        {
            if (this.GetComponent(type) == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a component from the entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>The component instance.</returns>
        public T GetComponent<T>()
            where T : Component
        {
            return this.GetComponent(typeof(T)) as T;
        }

        /// <summary>
        /// Gets a component from the entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The component instance.</returns>
        internal Component GetComponent(Type type)
        {
            var info = ComponentHelper.GetInfo(type);
            if (!info.HasValue)
            {
                throw new Exception("Internal error - trying to get a component that is not registered.");
            }
            return this.components[info.Value.Id];
        }

        /// <summary>
        /// Gets a list of components in this entity.
        /// </summary>
        /// <returns>
        /// Enumerator to a list of components.
        /// </returns>
        public Component[] GetComponents()
        {
            return this.components;
        }

        /// <summary>
        /// Marks the current entity for removal in the next update.
        /// </summary>
        public void Kill()
        {
            this.Dead = true;
            this.Manager.KillEntity(this);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        internal void Create()
        {
            foreach (var component in this.components)
            {
                if (component == null)
                {
                    continue;
                }
                component.Create();
            }

            this.OnCreate();
        }

        /// <summary>
        /// Entity has been created and added to the manager.
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        internal void Destroy()
        {
            foreach (var component in this.components)
            {
                if (component == null)
                {
                    continue;
                }
                component.Destroy();
            }

            for (ulong i = 0; i < ComponentHelper.Count; i++)
            {
                this.components[i] = null;
                this.drawableComponents[i] = null;
            }

            this.OnDestroy();
        }

        /// <summary>
        /// Called when the entity is getting destroyed by the manager.
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// Draws the entity.
        /// </summary>
        internal void Draw()
        {
            foreach (var component in this.drawableComponents)
            {
                if (component == null)
                {
                    continue;
                }
                component.Draw();
            }
        }

        /// <summary>
        /// Compares the current object with another object of the same info.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other" /> parameter.
        /// Zero
        /// This object is equal to <paramref name="other" />.
        /// Greater than zero
        /// This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(Entity other)
        {
            return (int)(this.Id - other.Id);
        }
    }
}