// -----------------------------------------------------------------------
// <copyright file="FontScreen.cs" company="Almirante Games">
// Copyright © Almirante Games 2013
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Fonts.Screens
{
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [Startup]
    public class FontScreen : Scene
    {
        /// <summary>
        ///
        /// </summary>
        private BitmapFont font;

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            font = AlmiranteEngine.Resources.DefaultFont;
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Screen logic update function.
        /// </summary>
        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            if (font != null)
            {
                batch.Begin();
                batch.DrawFont(font, Vector2.Zero, "The quick brown fox jumps over the lazy dog");
                font.Color = Color.Blue;
                batch.DrawFont(font, new Vector2(0, font.FontHeight + 10), "À noite, vovô Kowalsky vê o ímã cair no pé do pinguim queixoso e vovó põe açúcar\n no chá de tâmaras do jabuti feliz");
                font.Color = Color.OrangeRed;
                batch.DrawFont(font, new Vector2(0, font.FontHeight * 3 + 10), "The\nquick\nbrown\nfox\njumps\nover\nthe\nlazy\ndog");
                font.Color = Color.White;
                batch.DrawFont(font, new Vector2(300, font.FontHeight * 3 + 10), FontAlignment.Right, "The\nquick\nbrown\nfox\njumps\nover\nthe\nlazy\ndog");
                batch.DrawFont(font, new Vector2(600, font.FontHeight * 3 + 10), FontAlignment.Center, "The\nquick\nbrown\nfox\njumps\nover\nthe\nlazy\ndog");
                batch.End();
            }
        }
    }
}