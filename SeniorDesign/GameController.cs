using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SeniorDesign
{
    public class GameController : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ChopperSprite chopper;
        //FIXME just using one missile for testing
        private MissileSprite[] missiles;
        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            chopper = new ChopperSprite();
            missiles = new MissileSprite[3]
            {
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
            if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            chopper.Update(gameTime);
            foreach(var missile in missiles) missile.Update(gameTime, chopper.Position);
            // TODO: Add your update logic here

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
            foreach (var missile in missiles) missile.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
