// -----------------------------------------------------------------------
// <copyright file="ParallelMovementProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entity.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Entities.Filters;
    using Almirante.Entities.Systems.Multithreaded;
    using Tests.Entity.Entities.Components;

    /// <summary>
    /// Parallel movement processor
    /// </summary>
    internal class ParallelMovementProcessor : ParallelEntityProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelMovementProcessor"/> class.
        /// </summary>
        public ParallelMovementProcessor()
            : base(Filter.Create().Has(typeof(VelocityComponent)))
        {
        }

        /// <summary>
        /// Processes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void Process(Almirante.Entities.Entity e)
        {
            var position = e.Position;
            var velocity = e.GetComponent<VelocityComponent>();
            var diff = velocity.Value * 0.0016f;
            position.X = diff.X;
            position.Y = diff.Y;
        }
    }
}