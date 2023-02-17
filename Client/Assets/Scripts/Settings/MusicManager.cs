using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Object[] songs;
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
        songs = Resources.LoadAll("Audios/Music",typeof(AudioClip));

        _beenPlayed = new bool[songs.Length];

        if (!_audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0,songs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.volume = volume;

        if (_audioSource.isPlaying)
            _trackTimer += 1 * Time.deltaTime;

        if (!_audioSource.isPlaying || Input.GetButtonDown("Jump") || _trackTimer >= _audioSource.clip.length)
        {
            ChangeSong(Random.Range(0,songs.Length));
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
        }
        else{
            _audioSource.Stop();
        }
    }

    private void ResetShuffle()
    {
        if (_songsPlayed == songs.Length)
        {
            _songsPlayed = 0;
            for (int i=0;i<songs.Length;i++)
            {
                if (i == songs.Length)
                    break;
                else
                    _beenPlayed[i] = false;
            }
        }
    }
}
