using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;

namespace SeniorDesign
{
    public class GameController : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ChopperSprite chopper;
        //FIXME just using one missile for testing
        private MissileSprite[] missiles;//FIXME swap as missiles hit or miss, keep live missiles at the front 

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            chopper = new ChopperSprite();
            missiles = new MissileSprite[3]//fly away pattern, use a list instead, bool for alive
            {//object pool
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position)
            };
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            chopper.LoadContent(Content);
            foreach (var missile in missiles) missile.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// Updates sprites
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.GetState();
            bool release = false;//determines if current missile is released yet.
            if (KeyboardManager.IsPressed(Keys.Q) || KeyboardManager.IsPressed(Keys.Escape))
                Exit();
            chopper.Update(gameTime);
            //update all missiles each update to make sure their start position is set to most recent chopper position
            foreach (var missile in missiles)
            {
                
                missile.Update(chopper.Position);
                if (missile.Fired) missile.FireControl();
            }
            #region Monogame Example
            if (KeyboardManager.HasBeenPressed(Keys.Space))
            {
                int i = 0;
                while (!release && i < missiles.Length)
                {
                    //finds next available missile for firing
                    if (!missiles[i].Fired)
                    {
                        missiles[i].Update(true, chopper.Position);
                        release = true;
                    }
                    else i++;
                }
            }            
            #endregion Monogame Example

            
            base.Update(gameTime);
        }
        /// <summary>
        /// Draws sprites
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            chopper.Draw(gameTime, _spriteBatch);
            foreach (var missile in missiles)
            {
                missile.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
