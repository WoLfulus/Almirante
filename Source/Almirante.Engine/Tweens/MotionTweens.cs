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

namespace Almirante.Engine.Tweens
{
    using System;

    /// <summary>
    /// Tween method class.
    /// </summary>
    public static class MotionTweens
    {
        /// <summary>
        /// Easing function delegate.
        /// </summary>
        /// <param name="time">Time</param>
        /// <param name="start">Begin</param>
        /// <param name="change">Changes</param>
        /// <param name="duration">Duration</param>
        /// <returns>Modified value</returns>
        public delegate float TweenFunction(float time, float start, float change, float duration);

        /// <summary>
        /// BackIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BackIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
        }

        /// <summary>
        /// BackOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BackOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
        }

        /// <summary>
        /// BackInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BackInOut(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1)
            {
                return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            }
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /// <summary>
        /// BounceOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BounceOut(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75))
            {
                return c * (7.5625f * t * t) + b;
            }
            else if (t < (2 / 2.75))
            {
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            }
            else
            {
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
            }
        }

        /// <summary>
        /// BounceIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BounceIn(float t, float b, float c, float d)
        {
            return c - BounceOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        /// BounceInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float BounceInOut(float t, float b, float c, float d)
        {
            if (t < d / 2)
            {
                return BounceIn(t * 2, 0, c, d) * 0.5f + b;
            }
            else
            {
                return BounceOut(t * 2 - d, 0, c, d) * .5f + c * 0.5f + b;
            }
        }

        /// <summary>
        /// Linear tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float Linear(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        /// <summary>
        /// CircularIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CircularIn(float t, float b, float c, float d)
        {
            return -c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /// <summary>
        /// CircularOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CircularOut(float t, float b, float c, float d)
        {
            return c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /// <summary>
        /// CircularInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CircularInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
            }
            return c / 2 * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /// <summary>
        /// CubicIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CubicIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        /// CubicOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CubicOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /// <summary>
        /// CubicInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float CubicInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return c / 2 * t * t * t + b;
            }
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /// <summary>
        /// ElasticIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ElasticIn(float t, float b, float c, float d)
        {
            if (t == 0)
            {
                return b;
            }
            if ((t /= d) == 1)
            {
                return b + c;
            }
            float p = d * .3f;
            float s = p / 4;
            return -(float)(c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }

        /// <summary>
        /// ElasticOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ElasticOut(float t, float b, float c, float d)
        {
            if (t == 0)
            {
                return b;
            }
            if ((t /= d) == 1)
            {
                return b + c;
            }
            float p = d * .3f;
            float s = p / 4;
            return (float)(c * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b);
        }

        /// <summary>
        /// ElasticInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ElasticInOut(float t, float b, float c, float d)
        {
            if (t == 0)
            {
                return b;
            }
            if ((t /= d / 2) == 2)
            {
                return b + c;
            }
            float p = d * (.3f * 1.5f);
            float a = c;
            float s = p / 4;
            if (t < 1)
            {
                return -.5f * (float)(a * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
            }
            return (float)(a * Math.Pow(2, -10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b);
        }

        /// <summary>
        /// ExponentialIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ExponentialIn(float t, float b, float c, float d)
        {
            return (t == 0) ? b : c * (float)Math.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        /// ExponentialOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ExponentialOut(float t, float b, float c, float d)
        {
            return (t == d) ? b + c : c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <summary>
        /// ExponentialInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float ExponentialInOut(float t, float b, float c, float d)
        {
            if (t == 0)
            {
                return b;
            }
            if (t == d)
            {
                return b + c;
            }
            if ((t /= d / 2) < 1)
            {
                return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
            }
            return c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b;
        }

        /// <summary>
        /// QuadraticIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuadraticIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// QuadraticOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuadraticOut(float t, float b, float c, float d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        /// QuadraticInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuadraticInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return c / 2 * t * t + b;
            }
            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /// <summary>
        /// QuarticIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuarticIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        /// QuarticOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuarticOut(float t, float b, float c, float d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /// <summary>
        /// QuarticInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuarticInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return c / 2 * t * t * t * t + b;
            }
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /// <summary>
        /// QuinticIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuinticIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        /// QuinticOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuinticOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /// <summary>
        /// QuinticInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float QuinticInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return c / 2 * t * t * t * t * t + b;
            }
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /// <summary>
        /// SinusoidalIn tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float SinusoidalIn(float t, float b, float c, float d)
        {
            return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }

        /// <summary>
        /// SinusoidalOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float SinusoidalOut(float t, float b, float c, float d)
        {
            return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
        }

        /// <summary>
        /// SinusoidalInOut tweener.
        /// </summary>
        /// <param name="t">Time</param>
        /// <param name="b">Begin</param>
        /// <param name="c">Change</param>
        /// <param name="d">Duration</param>
        /// <returns>New position</returns>
        public static float SinusoidalInOut(float t, float b, float c, float d)
        {
            return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
        }
    }
}