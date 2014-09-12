# Almirante Engine #

### Overview ###

This is a project I made back in 2012. It's a (2D) game engine made on top of Microsoft's XNA Framework - Basically a collection of features that (in my opinion) are essential when making a 2D game. I've tried to keep the code clean, and some of the features are not complete at all.

### Features ###

- Bootstrap
	- Configure before launch
- Scene Management
	- Simple transitions
	- "Stack based" (push, pop, switch)
- Camera Management
	- Position
	- Rotation
	- Zoom
- Resolution Management
	- Scaling support
- Time Management
	- Slow motion?
- Resource Management
	- Sync/async loading (multithreaded)
- Bitmap fonts
	- Text alignment (left, right, center)
- Event-based input
	- Gamepad
	- Keyboard
	- Mouse
- Tweening
	- Machine-state based
	- Event-based
	- Pauseable
	- Fluent API
		- Forward(time)
		- Backward(time)
		- Repeat(times)
		- Wait(time)
		- Action(callable)
	- Different objects
		- Value (float)
		- Color (Microsoft.Xna.Framework.Color)
		- Vector (Microsoft.Xna.Framework.Vector2)
- Entity Management 
	- Entities
		- Composed with properties and attributes
	- Components
	- Systems
		- Supports filters
- *WinForms support*

### Examples ###

Examples are presented in form of simple test projects. See ***Tests*** folder inside the solution.

### License ###

> The MIT License (MIT)
> 
> Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy
> of this software and associated documentation files (the "Software"), to deal
> in the Software without restriction, including without limitation the rights
> to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
> copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in
> all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
> IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
> FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
> AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
> LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
> OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
> THE SOFTWARE.
