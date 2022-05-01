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
        private BulletParticleSystem bulletSystem;
        private Dragon dragon1;
        private Dragon dragon2;
        private Dragon[] testDragons;
        private SpriteFont font;
        private Vector2 aimVector;
        

        private KeyboardState currentKeyboardState;
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
            testDragons = new Dragon[]
            {
                new Dragon(3, new Vector2(600, 100)),
                new Dragon(3, new Vector2(600,400))
            };
            //FIXME initial loding for testing
            FlameParticleSystem.Initialize();
            foreach (var dragon in testDragons) FlameParticleSystem.AddNewDragonPos(dragon.Position);
            bulletSystem = new BulletParticleSystem(chopper.Position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            chopper.LoadContent(Content);
            bulletSystem.LoadContent(Content);
            font = Content.Load<SpriteFont>("bangers");

            //dragon1.LoadContent(Content);
            foreach (var missile in missiles) missile.LoadContent(Content);
            foreach (var dragon in testDragons) dragon.LoadContent(Content);
            // TODO: use this.Content to load your game content here
            FlameParticleSystem.LoadContent(Content);
        }
        /// <summary>
        /// Updates sprites
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = KeyboardManager.GetState();
            //FIXME move release var to control of missile sprite?
            bool release = false;//determines if current missile is released yet.
            if (KeyboardManager.IsPressed(Keys.Q) || KeyboardManager.IsPressed(Keys.Escape))
                Exit();
            chopper.Update(gameTime);
            checkChopperCollisions();
            //dragon1.Update(gameTime);
            //update all missiles each update to make sure their start position is set to most recent chopper position
            foreach (var missile in missiles)
            {
                
                missile.Update(chopper.Position);//FIXME move to chopper class?
                if (missile.Fired && missile.IsAlive) missile.FireControl();//FIXME move to missile class?
            }
            bool spawnFlame = false;
            foreach (var dragon in testDragons)
            {
                dragon.Update(gameTime, out spawnFlame);
                if (spawnFlame)
                {
                    //send targeting and origin pos info to flame system
                    //calculate aimVector
                    aimVector = dragon.Position - chopper.Position;
                    //FlameParticleSystem.Update(gameTime, spawnFlame, aimVector, dragon.Position);

                }
                FlameParticleSystem.Update(gameTime, spawnFlame, aimVector, dragon.Position, dragon.Alive);

                //FIXME finish to test all dragons later, just test with one now
                //FlameParticleSystem.
            }
            #region dragon flame testing

            //FlameParticleSystem.UpdateDragonPos(testDragons[0].Position, 0, testDragons[0].Alive);//FIXME not sure how this will work if dragon is dead
            //FlameParticleSystem.Update(gameTime, testDragons[0].Position, testDragons[0].Alive);            
            #endregion
            #region Monogame Example
            if (KeyboardManager.HasBeenPressed(Keys.Space))
            {
                int i = 0;
                while (!release && i < missiles.Length)
                {
                    //finds next available missile for firing
                    if (!missiles[i].Fired)
                    {
                        release = true;
                        missiles[i].Update(release, chopper.Position);
                    }
                    else i++;
                }
            }
            #endregion Monogame Example
            #region chopper machine gun
            /*
             while the m key is pressed down
            update the bullet particle system
             */
            if (KeyboardManager.IsPressed(Keys.M))
            {
                bulletSystem.IsFiring = true;
                
            }
            else
            {
                bulletSystem.IsFiring = false;
            }
            bulletSystem.Update(gameTime, chopper.Position);
            #endregion chopper machine gun
            checkDragonCollisions();
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
            bulletSystem.Draw(gameTime, _spriteBatch);
            //dragon1.Draw(gameTime, _spriteBatch);
            foreach (var dragon in testDragons) dragon.Draw(gameTime, _spriteBatch);
            FlameParticleSystem.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, $"Choppa HP: {chopper.HitPoints}", new Vector2(10, 10), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// checks collisions between chopper and dragon flames
        /// </summary>
        private void checkChopperCollisions()
        {
            int hitCount = 0;
            hitCount += FlameParticleSystem.CollisionChecker(chopper.Bounds);
            chopper.DetractHitPoints(hitCount, MunitionType.Flame);    
        }
        /// <summary>
        /// Checks whether bullets or missiles have collided with all currently alive dragons
        /// </summary>
        private void checkDragonCollisions()
        {//FIXME only working for one dragon on screen...
            int bulletCollisionCount = 0;
            int missileCollisionCount = 0;
            foreach (var dragon in testDragons)
            {
                if (!dragon.Alive) continue;
                bulletCollisionCount = bulletSystem.CollissionChecker(dragon.Bounds, dragon.Alive);
                foreach(var missile in missiles)
                {
                    missileCollisionCount += missile.CollisionChecker(dragon.Bounds);
                }
                dragon.DetractHitPoints(bulletCollisionCount, MunitionType.Bullet);
                dragon.DetractHitPoints(missileCollisionCount, MunitionType.Missile);
                missileCollisionCount = 0;
                bulletCollisionCount = 0;
            }
            
        }
    }
}
