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

namespace Almirante.Entities.Filters
{
    using System;
    using System.Collections.Generic;
    using Almirante.Entities.Filters.Rules;

    /// <summary>
    /// Filter class.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// List of rules to this filter.
        /// </summary>
        private List<Rule> rules;

        /// <summary>
        /// Filter constructor.
        /// </summary>
        protected Filter()
        {
            this.rules = new List<Rule>();
        }

        /// <summary>
        /// Applies the filter to an entity.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>True if all rules passes.</returns>
        public bool Apply(Entity entity)
        {
            foreach (var rule in this.rules)
            {
                if (!rule.Apply(entity))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Filters entities of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>This instance.</returns>
        public Filter Is(Type type)
        {
            this.rules.Add(new IsRule(type));
            return this;
        }

        /// <summary>
        /// Filters entities that is not the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>This instance.</returns>
        public Filter IsNot(params Type[] type)
        {
            this.rules.Add(new IsNotRule(type));
            return this;
        }

        /// <summary>
        /// Filters entities that has any of the specified components.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>This instance.</returns>
        public Filter IsAny(params Type[] type)
        {
            this.rules.Add(new IsAnyRule(type));
            return this;
        }

        /// <summary>
        /// Filters entities that has the specified components.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>This instance.</returns>
        public Filter Has(params Type[] types)
        {
            this.rules.Add(new HasRule(types));
            return this;
        }

        /// <summary>
        /// Filters entities that has any of the specified components.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>This instance.</returns>
        public Filter HasAny(params Type[] types)
        {
            this.rules.Add(new HasAnyRule(types));
            return this;
        }

        /// <summary>
        /// Filters entities that doesn't have the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>This instance.</returns>
        public Filter HasNot(params Type[] types)
        {
            this.rules.Add(new HasNotRule(types));
            return this;
        }

        /// <summary>
        /// Filters entities that has been tagged with the specified value.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>This instance.</returns>
        public Filter TaggedAs(string tag)
        {
            this.rules.Add(new TaggedRule(tag));
            return this;
        }

        /// <summary>
        /// Filters entities that has been grouped with the specified value.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        /// This instance.
        /// </returns>
        public Filter PartOfGroup(string group)
        {
            this.rules.Add(new GroupRule(group));
            return this;
        }

        #region Static

        /// <summary>
        /// Creates a filter instance.
        /// </summary>
        /// <returns>New filter instance.</returns>
        public static Filter Create()
        {
            return new Filter();
        }

        #endregion Static
    }
}