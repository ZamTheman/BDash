﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BDash
{
    public class InputManager
    {
        private static InputManager instance;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();
                return instance;
            }
        }

        KeyboardState currentKeyState, previousKeyState;

        public void Update()
        {
            previousKeyState = currentKeyState;
            if (!ScreenManager.Instance.IsTransitioning)
                currentKeyState = Keyboard.GetState();
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}
