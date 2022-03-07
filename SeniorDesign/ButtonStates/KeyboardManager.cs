using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SeniorDesign.ButtonStates
{
    /// <summary>
    /// This class comes from a 22 June 2019 Monogame Community Article
    /// "One-Shot key press" by Luca_Carminati
    /// https://community.monogame.net/t/one-shot-key-press/11669
    /// </summary>
    public class KeyboardManager
    {
        private static KeyboardState currentState;
        private static KeyboardState previousState;

        public static KeyboardState GetState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }

        
    }
}
