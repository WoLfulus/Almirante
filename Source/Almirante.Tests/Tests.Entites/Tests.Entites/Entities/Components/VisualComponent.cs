// -----------------------------------------------------------------------
// <copyright file="SpriteComponent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Entites.Entities.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Resources;
    using Almirante.Entities.Components;
    using Almirante.Entities.Components.Attributes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VisualComponent : DrawableComponent
    {
        /// <summary>
        /// Position
        /// </summary>
        [ComponentReference]
        private PositionComponent position = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw()
        {
            var batch = AlmiranteEngine.Batch;
            var font = AlmiranteEngine.Resources.DefaultFont;
            batch.DrawFont(font, new Vector2(this.position.X, this.position.Y), BitmapFontAlignment.Center, Color.DarkRed, "[" + this.Owner.Id + "] " + (int)this.position.X + ", " + (int)this.position.Y);
        }
    }
}