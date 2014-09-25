namespace Tests.Inputs.Screens
{
    using System;
    using System.Text;
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Input;
    using Almirante.Engine.Input.Devices;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Input test scene.
    /// </summary>
    [Startup]
    public class InputScreen : Scene
    {
        /// <summary>
        ///
        /// </summary>
        private double time;

        /// <summary>
        ///
        /// </summary>
        private readonly StringBuilder strings = new StringBuilder();

        /// <summary>
        ///
        /// </summary>
        private Resource<Texture2D> dot;

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            var input = AlmiranteEngine.Input;

            GamepadDevice controller = input.GetController(PlayerIndex.One);
            controller.Pressed += new GamepadDevice.GamepadButtonEvent(ButtonPressed);
            controller.Released += new GamepadDevice.GamepadButtonEvent(ButtonReleased);

            KeyboardDevice keyboard = input.Keyboard;
            keyboard.Pressed += new KeyboardDevice.KeyboardEvent(KeyPressed);
            keyboard.Released += new KeyboardDevice.KeyboardEvent(KeyReleased);

            MouseDevice mouse = input.Mouse;
            mouse.Moved += new MouseDevice.MouseMovedEvent(MouseMoved);
            mouse.Pressed += new MouseDevice.MouseKeyEvent(MousePressed);
            mouse.Released += new MouseDevice.MouseKeyEvent(MouseReleased);

            this.dot = AlmiranteEngine.Resources.LoadSync<Texture2D>("Images\\Dot");
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
            this.time += AlmiranteEngine.Time.Frame;
            if (this.time >= 0.25f)
            {
                this.time = time - 0.25f;
                int index = this.strings.ToString().IndexOf(System.Environment.NewLine);
                if (index >= 0)
                {
                    this.strings.Remove(0, index + System.Environment.NewLine.Length);
                }
            }
        }

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            var input = AlmiranteEngine.Input;
            var resources = AlmiranteEngine.Resources;

            var background = resources.DefaultBackground;

            batch.Start(true);
            batch.Draw(background, Vector2.Zero - new Vector2(background.Width / 2, background.Height / 2), Color.White);
            batch.Draw(dot.Content, Vector2.Zero - new Vector2(dot.Content.Width / 2, dot.Content.Height / 2), Color.White);
            batch.End();

            batch.Start();
            batch.DrawFont(resources.DefaultFont, new Vector2(25, 25), FontAlignment.Left, Color.White, this.strings.ToString());
            batch.DrawFont(resources.DefaultFont, new Vector2(250, 25), FontAlignment.Left, Color.White, string.Format("Mouse wheel: {0}", input.Mouse.ScrollValue));
            batch.DrawFont(resources.DefaultFont, new Vector2(250, 50), FontAlignment.Left, Color.White, string.Format("Mouse position: {0}", input.Mouse.Position));
            batch.DrawFont(resources.DefaultFont, new Vector2(250, 75), FontAlignment.Left, Color.White, string.Format("Mouse world position: {0}", input.Mouse.WorldPosition));
            batch.End();
        }

        /// <summary>
        /// Mouses the released.
        /// </summary>
        /// <param name="key">The key.</param>
        private void MouseReleased(MouseKey key)
        {
            this.strings.AppendLine(String.Format("Mouse Released: {0}", key.Key));
        }

        /// <summary>
        /// Mouses the pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        private void MousePressed(MouseKey key)
        {
            this.strings.AppendLine(String.Format("Mouse Pressed: {0}", key.Key));
        }

        /// <summary>
        /// Mouses the moved.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="worldPosition">The world position.</param>
        private void MouseMoved(Vector2 position, Vector2 worldPosition)
        {
            // strings.AppendLine(String.Format("Mouse Moved: {0}", position));
        }

        /// <summary>
        /// Keys the released.
        /// </summary>
        /// <param name="key">The key.</param>
        private void KeyReleased(KeyboardKey key)
        {
            this.strings.AppendLine(String.Format("Keyboard Released: {0}", key.Key));
        }

        /// <summary>
        /// Keys the pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        private void KeyPressed(KeyboardKey key)
        {
            this.strings.AppendLine(String.Format("Keyboard Pressed: {0}", key.Key));
        }

        /// <summary>
        /// Buttons the released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="button">The button.</param>
        private void ButtonReleased(GamepadDevice sender, GamepadButton button)
        {
            this.strings.AppendLine(String.Format("Released: {0}", button.Button));
        }

        /// <summary>
        /// Buttons the pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="button">The button.</param>
        private void ButtonPressed(GamepadDevice sender, GamepadButton button)
        {
            this.strings.AppendLine(String.Format("Pressed: {0}", button.Button));
        }
    }
}