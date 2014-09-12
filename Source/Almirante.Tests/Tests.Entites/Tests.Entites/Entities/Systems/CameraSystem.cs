// -----------------------------------------------------------------------
// <copyright file="CameraSystem.cs" company="">
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
    using Almirante.Entities.Filters;
    using Almirante.Entities.Systems;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Movement system.
    /// </summary>
    public class CameraSystem : EntitySystem
    {
        /// <summary>
        /// Creates a movement system.
        /// </summary>
        public CameraSystem()
        {
        }

        /// <summary>
        /// Called when the system executes.
        /// </summary>
        /// <param name="time"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void OnExecute(double time)
        {
            Vector2 movement = Vector2.Zero;

            if (AlmiranteEngine.Input.Keyboard[Keys.Up].Down)
            {
                movement.Y = -100.0f;
            }
            else if (AlmiranteEngine.Input.Keyboard[Keys.Down].Down)
            {
                movement.Y = 100.0f;
            }

            if (AlmiranteEngine.Input.Keyboard[Keys.Left].Down)
            {
                movement.X = -100.0f;
            }
            else if (AlmiranteEngine.Input.Keyboard[Keys.Right].Down)
            {
                movement.X = 100.0f;
            }

            AlmiranteEngine.Camera.Position += movement * (float)AlmiranteEngine.Time.Frame;
        }
    }
}