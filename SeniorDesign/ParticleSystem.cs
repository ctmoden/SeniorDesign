using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SeniorDesign.ButtonStates;
using System.Collections.Generic;

namespace SeniorDesign
{
    public abstract class ParticleSystem
    {
        #region Static Fields
        /// <summary>
        /// Shared sprite batch for particle system(s)
        /// </summary>
        protected static SpriteBatch spriteBatch;
        /// <summary>
        /// Shared content manager for particle system(s) 
        /// </summary>
        protected static ContentManager contentManager;
        #endregion Static Fields

        #region Private Fields
        //array of particles
        private Particle[] particles;
        //keeps track of free particles for quicker reloading
        private Queue<int> freeParticles;
        //s
        private Texture2D texture;
        #endregion
        #region cpu caching notes!
        /// <summary>
        /// A Queue containing indices of unused particles in the Particles array
        /// keeps track of free particles in array
        /// slots in array point to somewhere else in memory
        /// actual particle data in individual cells AS A STRUCT(linear in mem)
        /// helps  with memory caching
        /// registers are 64 bit in modern omputers, hold a single val for competition
        /// RAM: where program vars and code live
        /// static | stack | heap
        /// stack: instance/class vars
        /// heap: dynamic memory, expands and contracts (garbage collection)
        /// stack and heap grow towards eachother 
        /// garbage collector cleans things up as program progresses
        /// struct values in array are actually stored in array in stack, otherwise using pointers to heap
        /// copy particle structs into registers and cache surrounding mem in L2 cache in memory
        /// quickly accessed, right on motherboard next to socket on CPU
        /// L1 cache stored on the CPU dye itself (smaller than L2 cache, very fast)
        /// sepearate L1 cache for every CPU
        /// copy from raam to put into L2 cache, and L1 cache will pull from the L2 cache to 
        /// crunch computations a lot quicker than sending byte by byte to CPU from RAM
        /// we grab the cell we want (with actual values... not just pointers to other mem locations which we would have to go back to RAM to get)
        /// cache miss results in going back to RAM from cache when you have to do something like get
        /// values from pointers.
        /// Would dynamic particel object be stored in the heap??
        /// </summary>
        #endregion

        public ParticleSystem(int maxParticles)
        {
            particles = new Particle[maxParticles];
            for(int i = 0; i < particles.Length; i++)
            {
                particles[i].Initialize();
                freeParticles.Enqueue(i);//enqueues all particles at first
            }
        }

    }
}
