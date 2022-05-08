using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using System.Collections.Generic;
using System;

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
        //private Dragon[] testDragons;
        private List<Dragon> testDragons;
        private SpriteFont font;
        private Vector2 aimVector;
        private int killCount = 0;
        private double gameSeconds;
        private double gamePlayTime;
        private KeyboardState currentKeyboardState;
        private double currentBestTime;
        private int currentKillCount;
        private bool newBestTimeSet = false;
        private bool isFileUpdated = false;
        private bool isTimeRecorded = false;
        public GameController()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            chopper = new ChopperSprite();
            readFile();
            initializeChopperWeapons();
            initializeDragonsAndFlames();
            base.Initialize();
        }
        private void readFile()
        {
            FileReader.SetFileName("high_score.txt");
            FileReader.ReadFile(Content);
            currentBestTime = FileReader.GetBestTime();
            currentKillCount = FileReader.GetDragonsKilled();
        }
        private void initializeChopperWeapons()
        {
            missiles = new MissileSprite[10]//fly away pattern, use a list instead, bool for alive
            {//object pool
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position),
                new MissileSprite(chopper.Position)
            };
            bulletSystem = new BulletParticleSystem(chopper.Position);

        }
        private void initializeDragonsAndFlames()
        {
            testDragons = new List<Dragon>();
            for(int i = 0; i < 3; i++)
            {
                testDragons.Add(new Dragon(3, true));
            }
            for(int i = 0; i < 6; i++)
            {
                testDragons.Add(new Dragon(3, false));
            }
            FlameParticleSystem.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            loadChopperAndWeaponsContent();
            font = Content.Load<SpriteFont>("bangers");
            //dragon1.LoadContent(Content);
            loadDragonAndFlamesContent();              // TODO: use this.Content to load your game content here
            
        }
        private void loadChopperAndWeaponsContent()
        {
            chopper.LoadContent(Content);
            bulletSystem.LoadContent(Content);
            foreach (var missile in missiles) missile.LoadContent(Content);

        }      
        private void loadDragonAndFlamesContent()
        {
            foreach (var dragon in testDragons) dragon.LoadContent(Content);
            FlameParticleSystem.LoadContent(Content);
        }
        /// <summary>
        /// When all the dragons on screen die, wait a random amount of time and add 1-3 dragons on screen
        /// call from update
        /// </summary>
        private void generateDragons()
        {
            bool allDead = false;
            int killed = 0;
            //have to do this since dragons will die out of order
            for(int i = Dragon.killCount-1; i>=0; i--)
            {
                if (!testDragons[i].Alive) killed++;
            }
            if(killed == Dragon.killCount && killed > 0)
            {
                allDead = true;
            }
            //want to look at the next dragon in the list
            if(Dragon.killCount < testDragons.Count)
            {
                if (allDead && !testDragons[Dragon.killCount].OnScreen)
                {
                    //add 1-3 dragons to screen 
                    int dragonNum = HelperMethods.Next(1, 4);
                    int index = Dragon.killCount;
                    while (index < dragonNum + Dragon.killCount && index < testDragons.Count)
                    {
                        testDragons[index].SetOnScreen(true);
                        index++;
                    }
                }
            }
                          
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
            if (!(chopper.IsAlive) || Dragon.killCount == testDragons.Count);
            {
                if (KeyboardManager.IsPressed(Keys.R)) resetGame();
            }
            checkChopperCollisions();
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
                    Vector2 noise = new Vector2(HelperMethods.Next(0, 5), HelperMethods.Next(0, 5));
                    Vector2 tempChopper = chopper.Position + new Vector2(150, 60) + noise;//add noise to make less accurate
                    aimVector = dragon.Position - tempChopper;
                }
                
                FlameParticleSystem.Update(gameTime, spawnFlame, aimVector, dragon.Position, dragon.Alive);             
            }
            if ((Dragon.killCount == testDragons.Count || !chopper.IsAlive) && !isTimeRecorded)
            {
                isTimeRecorded = true;
                gamePlayTime = Math.Round(gameSeconds, 2);
                
            }
            if(Dragon.killCount == testDragons.Count && chopper.IsAlive)
            {
                if (currentBestTime > gamePlayTime && !isFileUpdated)
                {
                    newBestTimeSet = true;
                    FileReader.WriteHighScoreInfo(Content, gamePlayTime, testDragons.Count);
                    isFileUpdated = true;
                }
            }
                #region dragon flame testing
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
            generateDragons();
            checkDragonCollisions();
            base.Update(gameTime);
        }
        /// <summary>
        /// Reads high score in at start of gamePlay
        /// </summary>
        private void readHighScore()
        {

        }
        private void writeHighScore()
        {

        }
        private void resetGame()
        {
            gameSeconds = 0.0;
            gamePlayTime = 0.0;
            resetChopper();
            resetDragons();

        }
        private void resetDragons()
        {
            testDragons.Clear();
            initializeDragonsAndFlames();
            loadDragonAndFlamesContent();
        }
        private void resetChopper()
        {
            chopper.ResetChopper(true);
            initializeChopperWeapons();
            loadChopperAndWeaponsContent();
                       
        }
        /// <summary>
        /// Draws sprites
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if(chopper.IsAlive) gameSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            chopper.Draw(gameTime, _spriteBatch);
            foreach (var missile in missiles)
            {
                missile.Draw(gameTime, _spriteBatch);
            }
            bulletSystem.Draw(gameTime, _spriteBatch);
            //dragon1.Draw(gameTime, _spriteBatch);
            foreach (var dragon in testDragons) dragon.Draw(gameTime, _spriteBatch);
            Vector2 aimTemp = aimVector;
            FlameParticleSystem.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, $"Choppa HP: {chopper.HitPoints}", new Vector2(10, 10), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(font, $"Kill Count: {Dragon.killCount}", new Vector2(10, 20), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
            switch (chopper.IsAlive)
            {
                case true:
                    if(Dragon.killCount == testDragons.Count) _spriteBatch.DrawString(font, $"Total Time: {Math.Round(gamePlayTime, 2)}", new Vector2(10, 30), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
                    else _spriteBatch.DrawString(font, $"Elapsed Time: {Math.Round(gamePlayTime, 2)}", new Vector2(10, 30), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
                    break;
                case false:
                    _spriteBatch.DrawString(font, $"Total Time: {Math.Round(gamePlayTime, 2)}", new Vector2(10, 30), Color.Gold, 0f, new Vector2(), .25f, SpriteEffects.None, 0);
                    break;
            }
            if (!chopper.IsAlive) _spriteBatch.DrawString(font, $"You died! :/ Press 'R' to try again!", new Vector2(100, 200), Color.Gold, 0f, new Vector2(), 1f, SpriteEffects.None, 0);
            //if dragon killcount == testDragons.Count: display win message on screen
            if(Dragon.killCount == testDragons.Count) _spriteBatch.DrawString(font, $"You won, all dragons destroyed! Press 'R' to play again or 'esc'/'Q' to quit", new Vector2(75, Constants.GAME_HEIGHT/7), Color.Gold, 0f, new Vector2(), .5f, SpriteEffects.None, 0);
            if(newBestTimeSet) _spriteBatch.DrawString(font, $"New Record! You destroyed {testDragons.Count} dragons in {gamePlayTime} seconds", new Vector2(75, Constants.GAME_HEIGHT / 5),
                Color.Gold, 0f, new Vector2(), .5f, SpriteEffects.None, 0);
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
