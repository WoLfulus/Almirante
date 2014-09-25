namespace Tests.Gui.Screens
{
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Input;
    using Almirante.Engine.Interface;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Tests.Gui.Screens.Controls;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    [Startup]
    public class MainScreen : Scene
    {
        /// <summary>
        /// Stores the example button.
        /// </summary>
        private Button button1;

        /// <summary>
        /// Stores the example button.
        /// </summary>
        private Button button2;

        /// <summary>
        /// Stores the example panel.
        /// </summary>
        private Panel panel;

        /// <summary>
        /// Stores the example text scroller.
        /// </summary>
        private TextScroller scroller;

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            this.Interface.MouseClick += new System.EventHandler<MouseEventArgs>(Interface_MouseClick);

            this.button1 = new Button()
            {
                Text = "Button 1",
                Position = new Vector2(20, 20),
                Size = new Vector2(140, 50)
            };

            this.button1.MouseClick += (s, e) =>
            {
                this.scroller.Write("Button1 Clicked");
            };

            this.button1.MouseDoubleClick += (s, e) =>
            {
                this.scroller.Write("Button1 Double Clicked");
            };

            this.button1.MouseDown += (s, e) =>
            {
                this.scroller.Write("Button1 Mouse Down");
            };

            this.button1.MouseUp += (s, e) =>
            {
                this.scroller.Write("Button1 Mouse Up");
            };

            this.button1.MouseEnter += (s, e) =>
            {
                this.scroller.Write("Button1 Mouse Enter");
            };

            this.button1.MouseLeave += (s, e) =>
            {
                this.scroller.Write("Button1 Mouse Leave");
            };

            this.button2 = new Button()
            {
                Text = "Button 2",
                Position = new Vector2(20, 80),
                Size = new Vector2(140, 50)
            };

            this.button2.MouseClick += (s, e) =>
            {
                this.scroller.Write("Button2 Clicked");
            };

            this.button2.MouseDoubleClick += (s, e) =>
            {
                this.scroller.Write("Button2 Double Clicked");
            };

            this.button2.MouseDown += (s, e) =>
            {
                this.scroller.Write("Button2 Mouse Down");
            };

            this.button2.MouseUp += (s, e) =>
            {
                this.scroller.Write("Button2 Mouse Up");
            };

            this.button2.MouseEnter += (s, e) =>
            {
                this.scroller.Write("Button2 Mouse Enter");
            };

            this.button2.MouseLeave += (s, e) =>
            {
                this.scroller.Write("Button2 Mouse Leave");
            };

            this.panel = new Panel()
            {
                Position = new Vector2(100, 100),
                Size = new Vector2(500, 300)
            };

            this.scroller = new TextScroller()
            {
                Position = new Vector2(650, 100),
                Size = new Vector2(600, 400)
            };

            this.scroller.Write("Hello Example World!!! 1!!! ! !! !!!1!!1!");
            this.scroller.Write("Hello Example World!!! 1!!! !1!!1!");
            this.scroller.Write("Hello Example World! !! !!!1!!1!");
            this.scroller.Write("Hello Example World!!! 1!!1!");
            this.scroller.Write("Hello Example World!!! 1!");
            this.scroller.Write("Hello Example World!");

            this.panel.Controls.Add(this.button1);
            this.panel.Controls.Add(this.button2);
            this.Interface.Controls.Add(this.scroller);
            this.Interface.Controls.Add(this.panel);
        }

        private void Interface_MouseClick(object sender, MouseEventArgs e)
        {
            this.scroller.Write("Root click");
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
            var input = AlmiranteEngine.Input;
            var resources = AlmiranteEngine.Resources;

            batch.Start();
            batch.Draw(resources.DefaultBackground, new Vector2(0, 0), Color.White);
            batch.DrawFont(resources.DefaultFont, new Vector2(50, 50), FontAlignment.Left, Color.DarkMagenta, string.Format("Mouse: {0}, {1}", input.Mouse.Position.X, input.Mouse.Position.Y));
            batch.End();
        }
    }
}