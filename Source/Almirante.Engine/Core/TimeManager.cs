/// 
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 

namespace Almirante.Engine.Core
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Game time management.
    /// </summary>
    public sealed class TimeManager
    {
        /// <summary>
        /// Stores the last time the FPS was updated.
        /// </summary>
        private double fpsUpdate;

        /// <summary>
        /// Stores the total ammount of time spent in updates.
        /// </summary>
        private double fpsAccumulator;

        /// <summary>
        /// Stores how many times the update functions was called in a second.
        /// </summary>
        private int fpsUpdates;

        /// <summary>
        /// Gets the time since the application startup.
        /// </summary>
        public double Total
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the time since the application startup.
        /// </summary>
        public double TotalScaled
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the elapsed frame time.
        /// </summary>
        public double Frame
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the elapsed frame time.
        /// </summary>
        public double FrameScaled
        {
            get;
            internal set;
        }

        /// <summary>
        /// Global time scale.
        /// </summary>
        public double Scale
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the fps
        /// </summary>
        public double Fps
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the fps
        /// </summary>
        public double IntervaledFps
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Gets the GameTime of the current frame
        /// </summary>
        public GameTime GameTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the game timer.
        /// </summary>
        public void Initialize()
        {
            this.Fps = 60;
            this.IntervaledFps = 60;
            this.GameTime = new GameTime();
            this.Scale = 1.0;
        }

        /// <summary>
        /// Updates the game time management.
        /// </summary>
        /// <param name="time">Elapsed time.</param>
        public void Update(GameTime time)
        {
            this.GameTime = time;

            this.Frame = time.ElapsedGameTime.TotalSeconds;
            this.Total += this.Frame;
            this.Fps = 1.0 / this.Frame;

            this.FrameScaled = this.Frame * this.Scale;
            this.TotalScaled += this.FrameScaled;

            this.fpsAccumulator += this.Frame;
            this.fpsUpdates++;

            if (this.Total - this.fpsUpdate >= 1.0)
            {
                this.fpsUpdate = this.Total;
                this.IntervaledFps = this.fpsAccumulator / this.fpsUpdates;
                this.fpsAccumulator = 0;
                this.fpsUpdates = 0;
            }
        }
    }
}