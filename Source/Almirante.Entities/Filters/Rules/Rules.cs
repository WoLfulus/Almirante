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

namespace Almirante.Entities.Filters.Rules
{
    using System;
    using System.Numerics;
    using Almirante.Entities.Components;

    #region IS rules

    /// <summary>
    /// Matches the info for the entity.
    /// </summary>
    public class IsRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private Type type;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="type">Type</param>
        /// <exception cref="System.Exception">Invalid entity type on filter ( + type.Name + )</exception>
        public IsRule(Type type)
        {
            this.type = type;

            if (!typeof(Entity).IsAssignableFrom(type))
            {
                throw new Exception("Invalid entity type on filter (" + type.Name + ")");
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return this.type.IsAssignableFrom(entity.GetType());
        }
    }

    /// <summary>
    /// Matches the info for the entity.
    /// </summary>
    public class IsAnyRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private Type[] types;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <exception cref="System.Exception">Invalid entity type on filter ( + type.Name + )</exception>
        public IsAnyRule(Type[] types)
        {
            this.types = types;

            foreach (var type in this.types)
            {
                if (!typeof(Entity).IsAssignableFrom(type))
                {
                    throw new Exception("Invalid entity type on filter (" + type.Name + ")");
                }
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            foreach (var type in this.types)
            {
                if (type.IsAssignableFrom(entity.GetType()))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Matches the info for the entity.
    /// </summary>
    public class IsNotRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private Type[] types;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <exception cref="System.Exception">Invalid entity type on filter ( + type.Name + )</exception>
        public IsNotRule(Type[] types)
        {
            this.types = types;

            foreach (var type in this.types)
            {
                if (!typeof(Entity).IsAssignableFrom(type))
                {
                    throw new Exception("Invalid entity type on filter (" + type.Name + ")");
                }
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            foreach (var type in this.types)
            {
                if (type.IsAssignableFrom(entity.GetType()))
                {
                    return false;
                }
            }
            return true;
        }
    }

    #endregion IS rules

    #region HAS rules

    /// <summary>
    /// Matches all components for the entity.
    /// </summary>
    public class HasRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private BigInteger mask;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="types">Type list</param>
        /// <exception cref="System.Exception">Invalid component type in filter.</exception>
        public HasRule(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var info = ComponentHelper.GetInfo(types[i]);
                if (!info.HasValue)
                {
                    throw new Exception("Invalid component type in filter.");
                }

                this.mask |= info.Value.Mask;
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return (entity.ComponentMask & this.mask) == this.mask;
        }
    }

    /// <summary>
    /// Matches at least one info for the entity.
    /// </summary>
    public class HasAnyRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private BigInteger mask;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="types">Type list</param>
        /// <exception cref="System.Exception">Invalid component type in filter.</exception>
        public HasAnyRule(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var info = ComponentHelper.GetInfo(types[i]);
                if (!info.HasValue)
                {
                    throw new Exception("Invalid component type in filter.");
                }

                this.mask |= info.Value.Mask;
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return (entity.ComponentMask & this.mask) != 0;
        }
    }

    /// <summary>
    /// Matches at least one info for the entity.
    /// </summary>
    public class HasNotRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private BigInteger mask;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="types">Type list</param>
        /// <exception cref="System.Exception">Invalid component type in filter.</exception>
        public HasNotRule(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var info = ComponentHelper.GetInfo(types[i]);
                if (!info.HasValue)
                {
                    throw new Exception("Invalid component type in filter.");
                }

                this.mask |= info.Value.Mask;
            }
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return (entity.ComponentMask & this.mask) == 0;
        }
    }

    #endregion HAS rules

    #region TAG rules

    /// <summary>
    /// Matches all the specified tag.
    /// </summary>
    public class TaggedRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private string tag;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <exception cref="System.Exception">Invalid component type in filter.</exception>
        public TaggedRule(string tag)
        {
            this.tag = tag;
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return (entity.Tag == this.tag);
        }
    }

    #endregion TAG rules

    #region GROUP rules

    /// <summary>
    /// Matches the specified group.
    /// </summary>
    public class GroupRule : Rule
    {
        /// <summary>
        /// Type to check against.
        /// </summary>
        private string group;

        /// <summary>
        /// Constructs a new rule instance.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <exception cref="System.Exception">Invalid component type in filter.</exception>
        public GroupRule(string group)
        {
            this.group = group;
        }

        /// <summary>
        /// Applies the rule to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>
        /// True if not break.
        /// </returns>
        public bool Apply(Entity entity)
        {
            return (entity.Group == this.group);
        }
    }

    #endregion GROUP rules
}