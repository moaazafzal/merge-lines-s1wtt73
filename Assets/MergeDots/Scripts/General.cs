using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeDots.Scripts
{
    public class General : MonoBehaviour
    {
        private static General generalInstance;
    
        [Range(1,100)] public int allLevels = 10;
        public int level = 1;
        public string passedLevels = ",1,";
    
        public bool sound = true;
        public bool music = true;
        public GameObject btnSound;
        public GameObject btnMusic;
    
        public Sprite soundOn;
        public Sprite soundOff;
        public Sprite musicOn;
        public Sprite musicOff;

        public bool musicIsPlaying;
        public AudioClip audioMusic;
        public AudioClip audioLevelComplete;
        private AudioSource[] audioSources;


        // keeping General object in all scenes
        private void Awake()
        {
            DontDestroyOnLoad (this);
         
            if (generalInstance == null) {
                generalInstance = this;
            } else {
                DestroyObject(gameObject);
            }
        }

        private void Start()
        {
            GetPlayerPrefs();
            audioSources = GetComponents<AudioSource>();
        }


        private void Update()
        {
            // setting sprite and click listener of sound button
            if (btnSound == null)
            {
                btnSound = GameObject.Find("BtnSound");
                Button btnSoundButton = btnSound.GetComponent<Button>();
                btnSoundButton.onClick.AddListener(BtnSoundClick);

                SetSoundMusicSprites();
            }
        
            // setting sprite and click listener of music button
            if (btnMusic == null)
            {
                btnMusic = GameObject.Find("BtnMusic");
                Button btnMusicButton = btnMusic.GetComponent<Button>();
                btnMusicButton.onClick.AddListener(BtnMusicClick);
            
                SetSoundMusicSprites();
            }

            MusicPlayStop();
        }

        // playing or stopping music based on value of music variable
        void MusicPlayStop()
        {
            audioSources[0].clip = audioMusic;
            if (music && !audioSources[0].isPlaying)
            {
                audioSources[0].volume = 1f;
                audioSources[0].PlayOneShot(audioMusic);
            }
        
            else if (!music && audioSources[0].isPlaying)
            {
                audioSources[0].Stop();
            }

        }

        // playing given sound if sound is on
        public void PlaySound(string vSound)
        {
            if (!sound) return;
            if (vSound != "LevelComplete") return;
            audioSources[1].volume = 0.2f;
            audioSources[1].PlayOneShot(audioLevelComplete);

        }
    
        // getting PlayerPrefs (PassedLevels , Sound , Music)
        private void GetPlayerPrefs()
        {
            passedLevels = PlayerPrefs.GetString("PassedLevels");
            if (string.IsNullOrEmpty(passedLevels))
            {
                passedLevels = ",1,";
                PlayerPrefs.SetString("PassedLevels" , passedLevels);
            }
        
            if (PlayerPrefs.HasKey("Sound"))
            {
                sound = Convert.ToBoolean(PlayerPrefs.GetInt("Sound"));
            }
        
            if (PlayerPrefs.HasKey("Music"))
            {
                music = Convert.ToBoolean(PlayerPrefs.GetInt("Music"));
            }
        }
    
        void BtnSoundClick()
        {
            sound = !sound;
            PlayerPrefs.SetInt("Sound",Convert.ToInt32(sound));
            SetSoundMusicSprites();
        }
    
        void BtnMusicClick()
        {
            music = !music;
            PlayerPrefs.SetInt("Music",Convert.ToInt32(music));
            SetSoundMusicSprites();
        }

        // setting sprite of music & sound buttons
        void SetSoundMusicSprites()
        { 
            GetPlayerPrefs();

            if (btnSound != null)
            {
                if (sound)
                {
                    btnSound.GetComponent<Image>().sprite = soundOn;
                }
                else
                {
                    btnSound.GetComponent<Image>().sprite = soundOff;
                }
            }

            if (btnMusic != null)
            {
                if (music)
                {
                    btnMusic.GetComponent<Image>().sprite = musicOn;
                }
                else
                {
                    btnMusic.GetComponent<Image>().sprite = musicOff;
                }
            }
        }
       
    }
}
