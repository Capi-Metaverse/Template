using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    void Update()
    {
        _audioSource.volume = volume;

        //if (SceneManager.GetActiveScene().buildIndex > 1 && SongText == null)
        //{
        //    MusicController = GameObject.Find("Menus").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(9).gameObject;
        //    SongText = MusicController.transform.GetChild(2).GetComponent<TMP_Text>();
        //    SongText.text = CurrentSong;

        //    LeftArrow = MusicController.transform.GetChild(0).GetComponent<Button>();
        //    RightArrow = MusicController.transform.GetChild(1).GetComponent<Button>();

        //    LeftArrow.onClick.AddListener(() => ChangeSong(Random.Range(0, songs.Length)));
        //    RightArrow.onClick.AddListener(() => ChangeSong(Random.Range(0, songs.Length)));
        //}

        if (_audioSource.isPlaying)
            _trackTimer += 1 * Time.deltaTime;

        if ((!_audioSource.isPlaying || _trackTimer >= _audioSource.clip.length) && StopAudio == false)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
        ResetShuffle();
    }

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

    public void ActivateAudio()
    {
        _audioSource.Play();
        StopAudio = false;
    }
}