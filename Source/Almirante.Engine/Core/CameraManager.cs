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

namespace Almirante.Engine
{
    using System;
    using Almirante.Engine.Core;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Camera class.
    /// </summary>
    public sealed class CameraManager
    {
        /// <summary>
        /// The two PI value.
        /// </summary>
        public const float TwoPi = 6.283185f;

        /// <summary>
        /// The transformation
        /// </summary>
        private Matrix transformation;

        /// <summary>
        /// Gets the transformation.
        /// </summary>
        /// <value>
        /// The transformation.
        /// </value>
        public Matrix Matrix
        {
            get
            {
                return this.transformation;
            }
        }

        /// <summary>
        /// The origin
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The matrix origin
        /// </summary>
        private Matrix matrixOrigin;

        /// <summary>
        /// The position
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The matrix position
        /// </summary>
        private Matrix matrixPosition;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                this.position = value;
                float x = -this.position.X;
                float y = -this.position.Y;
                this.matrixPosition = Matrix.CreateTranslation(x, y, 0);
                this.UpdateTransformation();
            }
        }

        /// <summary>
        /// The rotation
        /// </summary>
        private float rotation;

        /// <summary>
        /// The rotation radians
        /// </summary>
        private float rotationRadians;

        /// <summary>
        /// The matrix rotation
        /// </summary>
        private Matrix matrixRotation;

        /// <summary>
        /// Gets or sets the rotation in degrees.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
                this.rotationRadians = (value / 360.0f) * CameraManager.TwoPi;
                this.matrixRotation = Matrix.CreateRotationZ(this.rotationRadians);
                this.UpdateTransformation();
            }
        }

        /// <summary>
        /// The zoom
        /// </summary>
        private float zoom;

        /// <summary>
        /// The zoom matrix scale
        /// </summary>
        private Matrix matrixZoom;

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        public float Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                if (value < 0.01f)
                {
                    this.zoom = 0.01f;
                }
                else
                {
                    this.zoom = value;
                    this.matrixZoom = Matrix.CreateScale(this.zoom, this.zoom, 1);
                }
                this.UpdateTransformation();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        internal CameraManager()
        {
            this.Zoom = 1.0f;
            this.Rotation = 0;
            this.Position = Vector2.Zero;
            this.UpdateTransformation();
        }

        /// <summary>
        /// Updates the origin.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        internal void UpdateOrigin(int width, int height)
        {
            this.origin.X = width * 0.5f;
            this.origin.Y = height * 0.5f;
            this.matrixOrigin = Matrix.CreateTranslation(this.origin.X, this.origin.Y, 0);
            this.UpdateTransformation();
        }

        /// <summary>
        /// Initializes the camera.
        /// </summary>
        internal void Initialize()
        {
            var resolution = AlmiranteEngine.Settings.Resolution;
            this.origin = new Vector2(resolution.BaseWidth * 0.5f, resolution.BaseHeight * 0.5f);
            this.matrixOrigin = Matrix.CreateTranslation(this.origin.X, this.origin.Y, 0);
            this.UpdateTransformation();
        }

        /// <summary>
        /// Updates the camera's transformation matrix.
        /// </summary>
        private void UpdateTransformation()
        {
            this.transformation = Matrix.Identity * this.matrixPosition * this.matrixRotation * this.matrixZoom * this.matrixOrigin;
        }
    }
}