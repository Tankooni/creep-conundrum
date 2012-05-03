using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Engine
{
    public enum Musics
    {
        One,
        Two,
        Three,
        Four
    }

    public enum SFX
    {
        
    }

    static class SoundManager
    {
        static List<SoundEffect> music = new List<SoundEffect>();
        static List<SoundEffect> sfx = new List<SoundEffect>();
        public static SoundEffectInstance currentMusic, previousMusic;
        private static Thread fadeInThread, fadeOutThread, setListThread;
        public static float Volume = .7f;
        private static bool loopd = false;
        private static double currentSongDuration, startTime;
        public static GameTime gameTime;
        
        static Musics[] setList;

        public static void InitMain(ContentManager CM)
        {
            music.Add(CM.Load<SoundEffect>(@"Music\Spazzmatica Polka"));
            music.Add(CM.Load<SoundEffect>(@"Music\Faceoff"));
        }

        public static void Init(ContentManager CM)
        {
            music.Clear();
            music.Add(CM.Load<SoundEffect>(@"Music\Corncob"));
            music.Add(CM.Load<SoundEffect>(@"Music\Faster Does It"));
            music.Add(CM.Load<SoundEffect>(@"Music\Groove Grove"));
            music.Add(CM.Load<SoundEffect>(@"Music\CATGROOVE"));
            
            //sfx.Add(CM.Load<SoundEffect>(@"Sounds\SFX\Hit"));
        }

        public static void PlaySound(SFX s, float volume)
        {
            SoundEffectInstance SoundInstance = sfx[(int)s].CreateInstance();
            SoundInstance.Volume = volume;
            if (SoundInstance != null)
                SoundInstance.Play();
        }

        public static void PlaySetList(Musics[] sL)
        {
            setList = sL;
            PlaySong(sL[0]);
            setListThread = new Thread(listUpdate);
            setListThread.IsBackground = true;
            setListThread.Start();
        }

        private static void listUpdate()
        {
            uint currentTrack = 0;
            while(true)
            {
                if ((gameTime.TotalGameTime.TotalMilliseconds - startTime) > (currentSongDuration - 2000))
                {
                    //Console.WriteLine("Triggered");
                    if (currentTrack < setList.Length-1)
                        currentTrack++;
                    else
                        currentTrack = 0;

                    PlaySong(setList[currentTrack]);
                }
            }
        }

        public static void PlayLoop(Musics m)
        {
            loopd = true;
            PlaySong(m);
            loopd = false;
        }

        public static void PlaySong(Musics m)
        {
            if (fadeOutThread != null)
                if (fadeOutThread.IsAlive && previousMusic != null)
                    previousMusic.Stop();
            previousMusic = currentMusic;
            currentMusic = music[(int)m].CreateInstance();
            if (currentMusic != null)
            {
                currentSongDuration = music[(int)m].Duration.TotalMilliseconds;
                try
                {
                    startTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                catch { }
                currentMusic.IsLooped = loopd;
                currentMusic.Volume = 0;
                currentMusic.Play();
                //if (fadeInThread != null && fadeInThread.IsAlive)
                //    fadeInThread.Abort();
                    fadeInThread = new Thread(fadeIn);
                fadeInThread.Name = "fade in thread";
                fadeInThread.IsBackground = true;
                fadeInThread.Start();

                fadeOutThread = new Thread(fadeOut);
                fadeOutThread.Name = "fade out thread";
                fadeOutThread.IsBackground = true;
                fadeOutThread.Start();
            }
        }

        private static void fadeIn()
        {
            //music.Volume = 0;
            while (currentMusic.Volume < Volume - 0.05f)
            {
                currentMusic.Volume += 0.05f;
                Thread.Sleep(60);
            }
            currentMusic.Volume = Volume;
        }
        private static void fadeOut()
        {
            try
            {
                while (previousMusic.Volume > 0 + 0.05f)
                {
                    previousMusic.Volume -= 0.05f;
                    Thread.Sleep(60);
                }
                previousMusic.Stop();
            }
            catch
            {
                fadeOutThread.Abort();
            }
        }
    }
}
