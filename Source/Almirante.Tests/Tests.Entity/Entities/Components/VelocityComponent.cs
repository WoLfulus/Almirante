// -----------------------------------------------------------------------
// <copyright file="VelocityComponent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entity.Entities.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Entities.Components;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Velocity component
    /// </summary>
    class VelocityComponent : Component
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Vector2 Value
        {
            get;
            set;
        }
    }
}
