using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audiosource;
    public AudioClip[] songs;
    public float volume;

    // Start is called before the first frame update
    void Start()
    {
        _audiosource = GetComponent<AudioSource>();

        if (!_audiosource.isPlaying)
        {
            Debug.Log("DebugLog: No music playing");
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _audiosource.volume = volume;

        if(!_audiosource.isPlaying)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
    }
    public void ChangeSong(int songPicked)
    {
        _audiosource.clip = songs[songPicked];
        _audiosource.Play();
    }
}
