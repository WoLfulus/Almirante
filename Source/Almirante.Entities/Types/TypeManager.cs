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

namespace Almirante.Entities.Types
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    /// <summary>
    /// Component info class.
    /// </summary>
    /// <typeparam name="T">Type to manage.</typeparam>
    public class TypeManager<T>
    {
        /// <summary>
        /// Locking object.
        /// </summary>
        private static object locker = new Object();

        /// <summary>
        /// Next component info bit.
        /// </summary>
        private static BigInteger nextBit = 1;

        /// <summary>
        /// Next component id.
        /// </summary>
        private static ulong nextId = 0;

        /// <summary>
        /// Component types holder.
        /// </summary>
        private static Dictionary<Type, TypeInfo> componentTypes = new Dictionary<Type, TypeInfo>();

        /// <summary>
        /// Gets the current count of registered types.
        /// </summary>
        public static ulong Count
        {
            get
            {
                lock (TypeManager<T>.locker)
                {
                    return nextId;
                }
            }
        }

        /// <summary>
        /// Blocks creation of instances.
        /// </summary>
        internal TypeManager()
        {
        }

        /// <summary>
        /// Gets a component info information.
        /// </summary>
        /// <param name="type">The requested type.</param>
        /// <returns>
        /// Type information of the component.
        /// </returns>
        public static TypeInfo? GetInfo(Type type)
        {
            lock (TypeManager<T>.locker)
            {
                if (!typeof(T).IsAssignableFrom(type))
                {
                    return null; // Invalid info information
                }

                TypeInfo info;
                if (TypeManager<T>.componentTypes.TryGetValue(type, out info))
                {
                    return info;
                }

                info = new TypeInfo();
                info.Id = TypeManager<T>.nextId++;
                info.Mask = TypeManager<T>.nextBit;

                TypeManager<T>.nextBit <<= 1;
                TypeManager<T>.componentTypes.Add(type, info);

                return info;
            }
        }
    }
}