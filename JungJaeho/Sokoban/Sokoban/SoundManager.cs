using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class SoundManager : Singleton<SoundManager>
    {
        public SoundManager() 
        { 
            _sounds = new Dictionary<string, Sound>();
        }

        public void AddSound(string tag, string path)
        {
            _sounds.Add(tag, new Sound(path));
        }

        public void PlaySound(string tag)
        {
            Sound sound;
            if(_sounds.TryGetValue(tag, out sound))
            {
                sound.PlaySound();
            }
        }
        private Dictionary<string, Sound> _sounds;
    }
}
