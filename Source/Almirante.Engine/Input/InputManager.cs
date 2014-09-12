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

namespace Almirante.Engine.Input
{
    using System;
    using System.Collections.Generic;
    using Almirante.Engine.Input.Devices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Input management class.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Stores the list of registered device list.
        /// </summary>
        private readonly List<IInputDevice> devices;

        /// <summary>
        /// Creates a new instance of the <see cref="InputManager" /> class.
        /// </summary>
        internal InputManager()
        {
            this.Gamepads = new Dictionary<PlayerIndex, GamepadDevice>();
            this.Gamepads[PlayerIndex.One] = new GamepadDevice(PlayerIndex.One);
            this.Gamepads[PlayerIndex.Two] = new GamepadDevice(PlayerIndex.Two);
            this.Gamepads[PlayerIndex.Three] = new GamepadDevice(PlayerIndex.Three);
            this.Gamepads[PlayerIndex.Four] = new GamepadDevice(PlayerIndex.Four);

            this.devices = new List<IInputDevice>();
            this.devices.Add(this.Keyboard = new KeyboardDevice());
            this.devices.Add(this.Mouse = new MouseDevice());
            this.devices.Add(this.Gamepads[PlayerIndex.One]);
            this.devices.Add(this.Gamepads[PlayerIndex.Two]);
            this.devices.Add(this.Gamepads[PlayerIndex.Three]);
            this.devices.Add(this.Gamepads[PlayerIndex.Four]);
        }

        /// <summary>
        /// Gets the keyboard device.
        /// </summary>
        public KeyboardDevice Keyboard
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the mouse device.
        /// </summary>
        public MouseDevice Mouse
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the gamepad devices.
        /// </summary>
        public Dictionary<PlayerIndex, GamepadDevice> Gamepads
        {
            get;
            internal set;
        }

        /// <summary>
        /// Get the specified GamepadDevice or create a new one if possible
        /// </summary>
        /// <param name="index">The index of the controller.</param>
        /// <returns>
        /// A GamepadDevice instance or null if the controller could not be created
        /// </returns>
        public GamepadDevice GetController(PlayerIndex index)
        {
            GamepadDevice gamepad = null;
            foreach (IInputDevice device in this.devices)
            {
                gamepad = device as GamepadDevice;
                if (gamepad != null)
                {
                    if (gamepad.PlayerIndex == index)
                    {
                        return gamepad;
                    }
                }
            }
            if (GamePad.GetState(index).IsConnected)
            {
                gamepad = new GamepadDevice(index);
                this.devices.Add(gamepad);
            }
            return gamepad;
        }

        /// <summary>
        /// Update all available devices
        /// </summary>
        public void Update()
        {
            foreach (var device in this.devices)
            {
                device.Update();
            }
        }

        /// <summary>
        /// Update all available devices
        /// </summary>
        public void Update(IntPtr window)
        {
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = window;
            foreach (var device in this.devices)
            {
                device.Update();
            }
        }
    }
}