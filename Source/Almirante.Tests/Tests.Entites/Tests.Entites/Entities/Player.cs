// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entites.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Entities;
    using Almirante.Entities.Components;
    using Almirante.Entities.Components.Attributes;
    using Tests.Entites.Entities.Components;

    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : Entity
    {
        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        [Component]
        public VelocityComponent Velocity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visual.
        /// </summary>
        /// <value>
        /// The visual.
        /// </value>
        [Component]
        public VisualComponent Visual
        {
            get;
            set;
        }

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