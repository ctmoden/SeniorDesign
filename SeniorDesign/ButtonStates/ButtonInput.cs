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
    /// The framework for this class is built upon the stack overflow 
    /// article "Better way to determine if a button is clicked"
    /// with solution by Anitaro Mocrosoft
    /// https://stackoverflow.com/questions/32448449/better-way-to-determine-if-a-button-is-clicked
    /// 
    /// </summary>
    public class ButtonInput
    {
        private static ButtonState buttonState;
        private static ButtonState lastButtonState;

        public static ButtonState ButtonState
        {
            get { return buttonState; }
            set { buttonState = value; }
        }

        
    }
}
