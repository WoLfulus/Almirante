using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Almirante.Network
{

    /// <summary>
    /// Packet manager.
    /// </summary>
    public class PacketManager
    {
        /// <summary>
        /// Locking object.
        /// </summary>
        private static object locker = new Object();

        /// <summary>
        /// Next component id.
        /// </summary>
        private static int next = 0;

        /// <summary>
        /// Component types holder.
        /// </summary>
        private static Dictionary<Type, PacketInfo> infos = new Dictionary<Type, PacketInfo>();

        /// <summary>
        /// Gets the current count of registered types.
        /// </summary>
        public static int Count
        {
            get
            {
                lock (PacketManager.locker)
                {
                    return next;
                }
            }
        }

        /// <summary>
        /// Blocks creation of instances.
        /// </summary>
        internal PacketManager()
        {
        }

        /// <summary>
        /// Gets a component info information.
        /// </summary>
        /// <param name="type">The requested type.</param>
        /// <returns>
        /// Type information of the component.
        /// </returns>
        public static PacketInfo GetInformation(Type type)
        {
            lock (PacketManager.locker)
            {
                PacketInfo info = null;
                if (PacketManager.infos.TryGetValue(type, out info))
                {
                    return info;
                }

                if (!typeof(Packet).IsAssignableFrom(type))
                {
                    throw new Exception("Type '" + type.FullName + "' doesn't inherit Packet class.");
                }

                var attributes = type.GetCustomAttributes(typeof(PacketAttribute), false);
                if (attributes.Length == 0)
                {
                    throw new Exception("Missing PacketAttribute attribute on class '" + type.FullName + "'.");
                }

                var packet = attributes[0] as PacketAttribute;
                var count = (from v in PacketManager.infos.Values
                             where v.Id == packet.Id 
                             select v).Count();
                if (count > 0)
                {
                    throw new Exception("Packet id #" + packet.Id + " already exists.");
                }

                info = new PacketInfo();
                info.Id = packet.Id;

                SortedList<int, PropertyInfo> props = new SortedList<int, PropertyInfo>();
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    attributes = property.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (attributes.Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        var field = attributes[0] as FieldAttribute;
                        props.Add(field.Id, property);
                    }
                }

                ///
                /// Constructor
                ///
                {
                    var construct = Expression.New(type);
                    info.Constructor = Expression.Lambda<Func<Packet>>(construct).Compile();
                }

                ///
                /// Writer
                ///
                {
                    var s = Expression.Parameter(typeof(BinaryWriter), "s");
                    var p = Expression.Parameter(typeof(Packet), "p");

                    var pc = Expression.Convert(p, type);

                    List<Expression> exps = new List<Expression>();
                    foreach (var prop in props)
                    {
                        var property = Expression.Property(pc, prop.Value.GetGetMethod());
                        var convert = Expression.Convert(property, prop.Value.PropertyType);
                        var call = Expression.Call(s, typeof(BinaryWriter).GetMethod("Write", new Type[1] { prop.Value.PropertyType }), convert);
                        exps.Add(call);
                    }

                    var b = Expression.Block(exps.ToList());

                    info.Writer = Expression.Lambda<Action<BinaryWriter, Packet>>(b, s, p).Compile();
                }

                ///
                /// Reader
                ///
                {
                    var s = Expression.Parameter(typeof(BinaryReader), "s");
                    var p = Expression.Parameter(typeof(Packet), "p");

                    var pc = Expression.Convert(p, type);

                    List<Expression> exps = new List<Expression>();
                    foreach (var prop in props)
                    {
                        var method = "";

                        var n = prop.Value.PropertyType.FullName;
                        if (n == typeof(Boolean).FullName)
                        {
                            method = "ReadBoolean";
                        } 
                        else if (n == typeof(Byte).FullName)
                        {
                            method = "ReadByte";
                        }
                        else if (n == typeof(Char).FullName)
                        {
                            method = "ReadChar";
                        }
                        else if (n == typeof(Decimal).FullName)
                        {
                            method = "ReadDecimal";
                        }
                        else if (n == typeof(Double).FullName)
                        {
                            method = "ReadDouble";
                        }
                        else if (n == typeof(Int16).FullName)
                        {
                            method = "ReadInt16";
                        }
                        else if (n == typeof(Int32).FullName)
                        {
                            method = "ReadInt32";
                        }
                        else if (n == typeof(Int64).FullName)
                        {
                            method = "ReadInt64";
                        }
                        else if (n == typeof(SByte).FullName)
                        {
                            method = "ReadSByte";
                        }
                        else if (n == typeof(Single).FullName)
                        {
                            method = "ReadSingle";
                        }
                        else if (n == typeof(String).FullName)
                        {
                            method = "ReadString";
                        }
                        else if (n == typeof(UInt16).FullName)
                        {
                            method = "ReadUInt16";
                        }
                        else if (n == typeof(UInt32).FullName)
                        {
                            method = "ReadUInt32";
                        }
                        else if (n == typeof(UInt64).FullName)
                        {
                            method = "ReadUInt64";
                        }
                        else
                        {
                            throw new Exception("Field type not supported: " + n);
                        }

                        var call = Expression.Call(s, method, new Type[0]);
                        var convert = Expression.Convert(call, prop.Value.PropertyType);
                        var set = Expression.Call(pc, prop.Value.GetSetMethod(), convert);

                        exps.Add(set);
                    }

                    var b = Expression.Block(exps.ToList());

                    info.Reader = Expression.Lambda<Action<BinaryReader, Packet>>(b, s, p).Compile();
                }
                
                PacketManager.infos.Add(type, info);
                return info;
            }
        }
    }
}
