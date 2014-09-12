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

namespace Almirante.Entities.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Almirante.Entities.Components.Attributes;
    using Almirante.Entities.Types;

    /// <summary>
    /// Component info class.
    /// </summary>
    internal class ComponentHelper : TypeManager<Component>
    {
        /// <summary>
        /// Component properties by type.
        /// </summary>
        internal static Dictionary<Type, List<ComponentReference>> componentRefs = new Dictionary<Type, List<ComponentReference>>();

        /// <summary>
        /// Component properties by type.
        /// </summary>
        internal static Dictionary<Type, Func<Component>> componentCtors = new Dictionary<Type, Func<Component>>();

        /// <summary>
        /// Component properties by type.
        /// </summary>
        internal static Dictionary<Type, List<ComponentSetter>> componentSetters = new Dictionary<Type, List<ComponentSetter>>();

        /*

        /// <summary>
        /// Component pool.
        /// </summary>
        // internal static List<Queue<Component>> componentPool = new List<Queue<Component>>();
        */

        /// <summary>
        /// Gets the "compiled" property setter for storing.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        internal static Action<object, object> GetPropertySetter(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite || propertyInfo.GetSetMethod() == null)
            {
                return null;
            }

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            var conversion = Expression.ConvertChecked(instance, propertyInfo.DeclaringType);
            var setter = propertyInfo.GetSetMethod();
            var param = Expression.Convert(value, propertyInfo.PropertyType);
            var call = Expression.Call(conversion, setter, param);

            return Expression.Lambda<Action<object, object>>(call, instance, value).Compile();
        }

        /// <summary>
        /// Gets the "compiled" property setter for storing.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        internal static Action<object, object> GetFieldSetter(FieldInfo fieldInfo)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            var conversion = Expression.ConvertChecked(instance, fieldInfo.DeclaringType);
            var param = Expression.Convert(value, fieldInfo.FieldType);

            var field = Expression.Field(conversion, fieldInfo);
            var cast = Expression.Convert(value, fieldInfo.FieldType);
            var equal = Expression.Assign(field, cast);

            return Expression.Lambda<Action<object, object>>(equal, instance, value).Compile();
        }

        /// <summary>
        /// Gets the "compiled" property setter for storing.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static Func<Component> GetConstructor(Type type)
        {
            var create = Expression.New(type.GetConstructor(new Type[0] { }));
            var convert = Expression.ConvertChecked(create, typeof(Component));
            return Expression.Lambda<Func<Component>>(convert).Compile();
        }

        /// <summary>
        /// Creates an instance of a component.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Component CreateInstance(Type type)
        {
            Func<Component> ctor = null;

            if (!componentCtors.TryGetValue(type, out ctor))
            {
                ctor = GetConstructor(type);
                componentCtors[type] = ctor;
            }

            return ctor();
        }

        /// <summary>
        /// Creates the component instances for the specified entity
        /// </summary>
        /// <param name="entity"></param>
        internal static void CreateComponents(Entity entity)
        {
            List<ComponentSetter> components = null;

            var entityType = entity.GetType();
            if (!componentSetters.TryGetValue(entityType, out components))
            {
                components = new List<ComponentSetter>();

                var properties = entityType.GetProperties();
                foreach (var property in properties)
                {
                    if (!typeof(Component).IsAssignableFrom(property.PropertyType))
                    {
                        continue;
                    }

                    if (Attribute.IsDefined(property, typeof(ComponentAttribute), true))
                    {
                        components.Add(new ComponentSetter()
                        {
                            Method = GetPropertySetter(property),
                            ComponentType = property.PropertyType
                        });
                    }
                }

                componentSetters[entityType] = components;
            }

            foreach (var component in components)
            {
                component.Method(entity, entity.AddComponent(component.ComponentType));
            }

            foreach (var component in components)
            {
                List<ComponentReference> refs = null;

                if (!componentRefs.TryGetValue(component.ComponentType, out refs))
                {
                    refs = new List<ComponentReference>();

                    FieldInfo[] fields = component.ComponentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (Attribute.IsDefined(field, typeof(ComponentReferenceAttribute), true))
                        {
                            if (!typeof(Component).IsAssignableFrom(field.FieldType))
                            {
                                continue;
                            }

                            refs.Add(new ComponentReference()
                            {
                                ReferencedType = field.FieldType,
                                Setter = GetFieldSetter(field)
                            });
                        }
                    }

                    componentRefs[component.ComponentType] = refs;
                }

                if (refs.Count > 0)
                {
                    var componentInstance = entity.GetComponent(component.ComponentType);
                    foreach (var r in refs)
                    {
                        Component instance = entity.GetComponent(r.ReferencedType);
                        if (instance == null)
                        {
                            throw new Exception("Entity '" + entityType.Name + "'" +
                                " has a component '" + component.ComponentType.Name + "' that depends on '" + r.ReferencedType.Name + "'" +
                                ", but it doesn't exists.");
                        }
                        else
                        {
                            r.Setter(componentInstance, instance);
                        }
                    }
                }
            }
        }
    }
}