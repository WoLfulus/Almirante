using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Almirante.Entities;
using Almirante.Entities.Components;
using Almirante.Entities.Components.Attributes;
using Almirante.Entities.Filters;
using Almirante.Entities.Systems;
using Almirante.Entities.Systems.Multithreaded;
using Microsoft.Xna.Framework;
using Tests.Entity.Entities;
using Tests.Entity.Systems;

namespace Tests.Entity
{
    /// <summary>
    /// Entry point
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Runs the system.
        /// </summary>
        private static void RunSystem<T>()
            where T : EntitySystem, new()
        {
            Stopwatch sw = new Stopwatch();

            EntityManager manager = new EntityManager();
            manager.Systems.Add<T>();

            Console.WriteLine("Running {0} system...", typeof(T).Name);
            Console.Write("Initializing entities... ");

            sw.Start();
            Random random = new Random((int)DateTime.Now.Ticks);

            int entityCount = 1000000;
            for (int j = 0; j < entityCount; j++)
            {
                var entity = manager.Create<Player>();
                entity.Position.Set(400, 400);
                entity.Velocity.Value = new Vector2() { X = random.Next(-10, 10), Y = random.Next(-10, 10) };
            }
            sw.Stop();

            Console.WriteLine("{0} ms / {1} ms", sw.Elapsed.TotalMilliseconds.ToString("0.00"), (sw.Elapsed.TotalMilliseconds / entityCount).ToString("0.00"));

            Console.Write("Running updates... ");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            int updateCount = 10;
            for (int i = 0; i < updateCount; i++)
            {
                manager.Update(timer.Elapsed.TotalSeconds);
            }

            timer.Stop();

            Console.WriteLine("{0} ms / {1} ms", sw.Elapsed.TotalMilliseconds.ToString("0.00"), (timer.Elapsed.TotalMilliseconds / updateCount).ToString("0.00"));
            Console.WriteLine("");
        }

        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        private static void Main(string[] args)
        {
            Program.RunSystem<MovementProcessor>();
            Program.RunSystem<ParallelMovementProcessor>();
            Console.ReadKey();
        }
    }
}