using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SeniorDesign
{
    public class Dragon
    {
        private Texture2D dragonTexture;

        private double animationTimer;

        private const short ANIMATION_ROW = 2;

        private Vector2 position;

        //FIXME need a public position?

        private int hitCount;

        public bool Alive;

        public Dragon()
        {
            hitCount = 0;
            position = new Vector2(500, 500);

        }
        public void LoadContent(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
