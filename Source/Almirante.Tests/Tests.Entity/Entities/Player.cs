// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entity.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Entities;
    using Almirante.Entities.Components;
    using Almirante.Entities.Components.Attributes;
    using Tests.Entity.Entities.Components;

    /// <summary>
    /// Player entity class.
    /// </summary>
    internal class Player : Entity
    {
        /// <summary>
        /// Gets or sets the velocity component.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        [Component]
        public VelocityComponent Velocity { get; set; }

        /// <summary>
        /// Entity has been created and added to the manager.
        /// </summary>
        protected override void OnCreate()
        {
        }

        /// <summary>
        /// Called when the entity is getting destroyed by the manager.
        /// </summary>
        protected override void OnDestroy()
        {
        }
    }
}