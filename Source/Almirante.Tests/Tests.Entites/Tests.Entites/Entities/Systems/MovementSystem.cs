// -----------------------------------------------------------------------
// <copyright file="MovementSystem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entites.Entities.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Engine.Core;
    using Almirante.Entities;
    using Almirante.Entities.Components;
    using Almirante.Entities.Filters;
    using Almirante.Entities.Systems;
    using Almirante.Entities.Systems.Multithreaded;
    using Tests.Entites.Entities.Components;

    /// <summary>
    /// Movement system.
    /// </summary>
    public class MovementSystem : ParallelEntityProcessor
    {
        /// <summary>
        /// Creates a movement system.
        /// </summary>
        public MovementSystem()
            : base(Filter.Create().Has(typeof(VelocityComponent)))
        {
        }

        /// <summary>
        /// Updates all the positions by applying velocity.
        /// </summary>
        /// <param name="entity"></param>
        protected override void Process(Entity entity)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var vel = entity.GetComponent<VelocityComponent>();
            var dif = vel.Value * (float)AlmiranteEngine.Time.Frame;
            pos.Set(dif.X + pos.X, dif.Y + pos.Y);
        }
    }
}