using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Sound : FileStream
    {
        public Sound(string path) : base(path, FileMode.Open, FileAccess.Read) 
        {
            _soundPlayer = new SoundPlayer(this);
        }

        public void PlaySound()
        {
            _soundPlayer.Play();
        }

        public void PlaySoundLoop()
        {
            _soundPlayer.PlayLooping();
        }

        public void StopSound()
        {
            _soundPlayer.Stop();
        }
        private SoundPlayer _soundPlayer;
    }
}
