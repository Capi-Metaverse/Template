using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Manager
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _instance;
        public static MusicManager Instance
        {
            get
            {
                return _instance;
            }
        }
        private AudioSource _audioSource;
        private GameObject MusicController;
        private TMP_Text SongText;
        private Button LeftArrow;
        private Button RightArrow;
        public bool StopAudio = false;
        public Object[] songs;
        public string CurrentSong;
        public float volume = 0.04f;
        [SerializeField] private float _trackTimer;
        [SerializeField] private float _songsPlayed;
        [SerializeField] private bool[] _beenPlayed;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
                Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        /// <summary>
        /// Start the music
        /// </summary>
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            songs = Resources.LoadAll("Audios/Music", typeof(AudioClip));

            _beenPlayed = new bool[songs.Length];

            if (!_audioSource.isPlaying)
            {
                ChangeSong(Random.Range(0, songs.Length));
            }
        }

        // Update is called once per frame
        /// <summary>
        /// Control the state of the musics, when a song finish start other
        /// </summary>
        void Update()
        {
            _audioSource.volume = volume;
            if (_audioSource.isPlaying)
                _trackTimer += 1 * Time.deltaTime;

            if ((!_audioSource.isPlaying || _trackTimer >= _audioSource.clip.length) && StopAudio == false)
            {
                ChangeSong(Random.Range(0, songs.Length));
            }
            ResetShuffle();
        }
        /// <summary>
        /// Change to other song
        /// </summary>
        /// <param name="songPicked"></param>
        public void ChangeSong(int songPicked)
        {
            if (!_beenPlayed[songPicked])
            {
                _trackTimer = 0;
                _songsPlayed++;
                _beenPlayed[songPicked] = true;
                _audioSource.clip = (AudioClip)songs[songPicked];
                _audioSource.Play();
                CurrentSong = songs[songPicked].name;

                //if (SongText != null)
                //    SongText.text = CurrentSong;
            }
            else
            {
                _audioSource.Stop();
            }
        }
        /// <summary>
        /// Reset the list, when all the song of the list have been played
        /// </summary>
        private void ResetShuffle()
        {
            if (_songsPlayed == songs.Length)
            {
                _songsPlayed = 0;
                for (int i = 0; i < songs.Length; i++)
                {
                    if (i == songs.Length)
                        break;
                    else
                        _beenPlayed[i] = false;
                }
            }
        }
        /// <summary>
        /// Stop and Activate music, when you are in the app
        /// </summary>
        public void ChangeAudioState()
        {
            if (_audioSource.isPlaying == true)
            {
                _audioSource.Stop();
                StopAudio = true;
            }
            else
            {
                _audioSource.Play();
                StopAudio = false;
            }
        }
        /// <summary>
        /// Activate music
        /// </summary>
        public void ActivateAudio()
        {
            _audioSource.Play();
            StopAudio = false;
        }
    }
}