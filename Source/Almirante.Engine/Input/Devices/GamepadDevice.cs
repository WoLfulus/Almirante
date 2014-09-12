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

namespace Almirante.Engine.Input.Devices
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Gamepad
    /// </summary>
    public class GamepadDevice : IInputDevice
    {
        /// <summary>
        /// Stores the left thumbstick threshold.
        /// </summary>
        private float leftThumbThreshold;

        /// <summary>
        /// Stores the right thumbstick threshold.
        /// </summary>
        private float rightThumbThreshold;

        /// <summary>
        /// Stores the left trigger threshold.
        /// </summary>
        private float leftTriggerThreshold;

        /// <summary>
        /// Stores the right trigger threshold.
        /// </summary>
        private float rightTriggerThreshold;

        /// <summary>
        /// Stores the left vibration.
        /// </summary>
        private float vibrationLeft;

        /// <summary>
        /// Stores the right vibration.
        /// </summary>
        private float vibrationRight;

        /// <summary>
        /// Stores all keyboard device keys.
        /// </summary>
        private readonly Dictionary<Buttons, GamepadButton> buttons;

        /// <summary>
        /// Stores the current keyboard state.
        /// </summary>
        private GamePadState state;

        /// <summary>
        /// Stores the current keyboard state.
        /// </summary>
        private GamePadState lastState;

        /// <summary>
        /// The player index related to this gamepad <see cref="PlayerIndex"/>
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get a Vector2 for the position of the left thumbstick
        /// </summary>
        public Vector2 LeftThumbstick
        {
            get
            {
                return GamePad.GetState(this.PlayerIndex).ThumbSticks.Left;
            }
        }

        /// <summary>
        /// Get a Vector2 for the position of the right thumbstick
        /// </summary>
        public Vector2 RightThumbstick
        {
            get
            {
                return GamePad.GetState(this.PlayerIndex).ThumbSticks.Right;
            }
        }

        /// <summary>
        /// Get the value for the left trigger position
        /// </summary>
        public float LeftTrigger
        {
            get
            {
                return GamePad.GetState(this.PlayerIndex).Triggers.Left;
            }
        }

        /// <summary>
        /// Get the value for the right trigger position
        /// </summary>
        public float RightTrigger
        {
            get
            {
                return GamePad.GetState(this.PlayerIndex).Triggers.Left;
            }
        }

        /// <summary>
        /// Get or set the safe value for the left thumbstick, ranging from 0.0 to 1.0
        /// </summary>
        public float LeftThumbstickThreshold
        {
            get
            {
                return this.leftThumbThreshold;
            }
            set
            {
                this.leftThumbThreshold = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        /// <summary>
        /// Get or set the safe value for the right thumbstick, ranging from 0.0 to 1.0
        /// </summary>
        public float RightThumbstickThreshold
        {
            get
            {
                return this.rightThumbThreshold;
            }
            set
            {
                this.rightThumbThreshold = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        /// <summary>
        /// Get or set the safe value for the left trigger, ranging from 0.0 to 1.0
        /// </summary>
        public float LeftTriggerThreshold
        {
            get
            {
                return this.leftTriggerThreshold;
            }
            set
            {
                this.leftTriggerThreshold = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        /// <summary>
        /// Get or set the safe value for the right trigger, ranging from 0.0 to 1.0
        /// </summary>
        public float RightTriggerThreshold
        {
            get
            {
                return this.rightTriggerThreshold;
            }
            set
            {
                this.rightTriggerThreshold = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        /// <summary>
        /// Set the vibration value for the left motor, ranging from 0.0 to 1.0
        /// </summary>
        public float VibrationLeft
        {
            get
            {
                return this.vibrationLeft;
            }
            set
            {
                this.vibrationLeft = MathHelper.Clamp(value, 0f, 1f);
                GamePad.SetVibration(this.PlayerIndex, this.vibrationLeft, this.vibrationRight);
            }
        }

        /// <summary>
        /// Set the vibration value for the right motor, ranging from 0.0 to 1.0
        /// </summary>
        public float VibrationRight
        {
            get
            {
                return this.vibrationLeft;
            }
            set
            {
                this.vibrationRight = MathHelper.Clamp(value, 0f, 1f);
                GamePad.SetVibration(this.PlayerIndex, this.vibrationLeft, this.vibrationRight);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get;
            private set;
        }

        /// <summary>
        /// Gamepad event delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void GamepadEvent(GamepadDevice sender);

        /// <summary>
        /// Gamepad event delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="button">The button</param>
        public delegate void GamepadButtonEvent(GamepadDevice sender, GamepadButton button);

        /// <summary>
        /// Occurs when the device connects.
        /// </summary>
        public event GamepadEvent Connected;

        /// <summary>
        /// Occurs when the device disconnects.
        /// </summary>
        public event GamepadEvent Disconnected;

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event GamepadButtonEvent Pressed;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event GamepadButtonEvent Released;

        /// <summary>
        /// Creates a new instance of the <see cref="GamepadDevice"/> class.
        /// </summary>
        internal GamepadDevice(PlayerIndex playerIndex)
        {
            this.PlayerIndex = playerIndex;
            this.buttons = new Dictionary<Buttons, GamepadButton>();
            foreach (Buttons k in Enum.GetValues(typeof(Buttons)))
            {
                this.buttons.Add(k, new GamepadButton(k));
            }
            this.LeftThumbstickThreshold = 0.2f;
            this.RightThumbstickThreshold = 0.2f;
            this.LeftTriggerThreshold = 0.2f;
            this.RightTriggerThreshold = 0.2f;
            this.VibrationLeft = 0.0f;
            this.VibrationRight = 0.0f;
        }

        /// <summary>
        /// Gets the key state for the specified keyboard key.
        /// </summary>
        /// <param name="key">Requested key.</param>
        /// <returns>State of the requested key.</returns>
        public GamepadButton this[Buttons key]
        {
            get
            {
                return this.buttons[key];
            }
            internal set
            {
                this.buttons[key] = value;
            }
        }

        /// <summary>
        /// Update the keyboard keys state
        /// </summary>
        public void Update()
        {
            this.lastState = this.state;
            this.state = GamePad.GetState(this.PlayerIndex);

            if (this.lastState == null)
            {
                this.lastState = this.state;
            }

            this.IsConnected = this.state.IsConnected;

            if (this.lastState.IsConnected)
            {
                if (!this.state.IsConnected)
                {
                    if (this.Disconnected != null)
                    {
                        this.Disconnected(this);
                    }
                }
            }
            else
            {
                if (this.state.IsConnected)
                {
                    if (this.Connected != null)
                    {
                        this.Connected(this);
                    }
                }
            }

            #region buttons

            GamePadButtons buttons = this.state.Buttons;

            this.UpdateButton(Buttons.A, buttons.A == ButtonState.Pressed);
            this.UpdateButton(Buttons.B, buttons.B == ButtonState.Pressed);
            this.UpdateButton(Buttons.X, buttons.X == ButtonState.Pressed);
            this.UpdateButton(Buttons.Y, buttons.Y == ButtonState.Pressed);
            this.UpdateButton(Buttons.LeftShoulder, buttons.LeftShoulder == ButtonState.Pressed);
            this.UpdateButton(Buttons.RightShoulder, buttons.RightShoulder == ButtonState.Pressed);
            this.UpdateButton(Buttons.LeftStick, buttons.LeftStick == ButtonState.Pressed);
            this.UpdateButton(Buttons.RightStick, buttons.RightStick == ButtonState.Pressed);
            this.UpdateButton(Buttons.Start, buttons.Start == ButtonState.Pressed);
            this.UpdateButton(Buttons.Back, buttons.Back == ButtonState.Pressed);
            this.UpdateButton(Buttons.BigButton, buttons.BigButton == ButtonState.Pressed);

            #endregion buttons

            #region dpad

            GamePadDPad dpad = this.state.DPad;

            this.UpdateButton(Buttons.DPadUp, dpad.Up == ButtonState.Pressed);
            this.UpdateButton(Buttons.DPadDown, dpad.Down == ButtonState.Pressed);
            this.UpdateButton(Buttons.DPadLeft, dpad.Left == ButtonState.Pressed);
            this.UpdateButton(Buttons.DPadRight, dpad.Right == ButtonState.Pressed);

            #endregion dpad

            #region thumbsticks

            Vector2 left = this.state.ThumbSticks.Left;
            this.UpdateButton(Buttons.LeftThumbstickRight, left.X >= this.LeftThumbstickThreshold);
            this.UpdateButton(Buttons.LeftThumbstickLeft, left.X <= -this.LeftThumbstickThreshold);
            this.UpdateButton(Buttons.LeftThumbstickUp, left.Y >= this.LeftThumbstickThreshold);
            this.UpdateButton(Buttons.LeftThumbstickDown, left.Y <= -this.LeftThumbstickThreshold);
            Vector2 right = this.state.ThumbSticks.Right;
            this.UpdateButton(Buttons.RightThumbstickRight, right.X >= this.RightThumbstickThreshold);
            this.UpdateButton(Buttons.RightThumbstickLeft, right.X <= -this.RightThumbstickThreshold);
            this.UpdateButton(Buttons.RightThumbstickUp, right.Y >= this.RightThumbstickThreshold);
            this.UpdateButton(Buttons.RightThumbstickDown, right.Y <= -this.RightThumbstickThreshold);

            #endregion thumbsticks

            #region triggers

            this.UpdateButton(Buttons.LeftTrigger, this.state.Triggers.Left >= this.LeftTriggerThreshold);
            this.UpdateButton(Buttons.RightTrigger, this.state.Triggers.Right >= this.RightTriggerThreshold);

            #endregion triggers
        }

        /// <summary>
        /// Update the state of the given button
        /// </summary>
        /// <param name="button">The button which will be evaluated</param>
        /// <param name="pressed">true if the button is pressed</param>
        private void UpdateButton(Buttons button, bool pressed)
        {
            GamepadButton gamepadButton = this[button];
            gamepadButton.Update(pressed);
            if (gamepadButton.Pressed)
            {
                if (this.Pressed != null)
                {
                    this.Pressed(this, gamepadButton);
                }
            }
            else if (gamepadButton.Released)
            {
                if (this.Released != null)
                {
                    this.Released(this, gamepadButton);
                }
            }
        }
    }
}