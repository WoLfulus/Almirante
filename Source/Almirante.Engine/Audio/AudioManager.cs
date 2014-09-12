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

namespace Almirante.Engine.Audio
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// Audio management class.
    /// </summary>
    public class AudioManager : IDisposable
    {
        /// <summary>
        /// Max concurrent sounds.
        /// </summary>
        public const int MaxSounds = 32;

        /// <summary>
        /// Song instances.
        /// </summary>
        private Dictionary<string, Song> songs = new Dictionary<string, Song>();

        /// <summary>
        /// Effect instances.
        /// </summary>
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        /// <summary>
        /// Current song.
        /// </summary>
        // private Song playingSong = null;

        /// <summary>
        /// Current sounds.
        /// </summary>
        private SoundEffectInstance[] playingSounds = new SoundEffectInstance[MaxSounds];

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioManager"/> class.
        /// </summary>
        internal AudioManager()
        {
        }

        #region IDisposable Implementation

        protected bool disposed/* = false*/;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Implementation
    }
}