using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;

namespace SeniorDesign
{
    public class BulletParticleSystem : ParticleSystem
    {
        private Vector2 chopperPos;
        public BulletParticleSystem(Vector2 chopperPos) : base(100)
        {
            this.chopperPos = chopperPos;
        }

        protected 
    }
}
