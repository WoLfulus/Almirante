// -----------------------------------------------------------------------
// <copyright file="VelocityComponent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entites.Entities.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Entities.Components;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Velocity component class.
    /// </summary>
    public class VelocityComponent : Component
    {
        /// <summary>
        /// Velocity value.
        /// </summary>
        public Vector2 Value
        {
            get;
            set;
        }
    }
}
