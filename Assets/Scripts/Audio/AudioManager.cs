using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    [SerializeField]
    private Dictionary<string, AudioSource> sources = new();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;
            source.outputAudioMixerGroup = s.mixerGroup ?? mixerGroup;
            sources.Add(s.name, source);
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        Debug.Log($"[Audiomanager] Playing sound {soundName}");
        if (sources.TryGetValue(soundName, out AudioSource source))
        {
            source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            source.Play();
        }


    }

    public void PlayAtPosition(string soundName, Vector3 position)
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        GameObject soundObject = new GameObject("Sound_" + soundName);
        soundObject.transform.position = position;

        AudioSource source = soundObject.AddComponent<AudioSource>();

        source.clip = s.clip;
        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.outputAudioMixerGroup = s.mixerGroup ?? mixerGroup;

        source.Play();
        if (!s.loop)
            Destroy(soundObject, s.clip.length / source.pitch);
        else
            sources.Add(soundName + position, source);


    }

    public void Stop(string soundName, Vector3? position = null)
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        Debug.Log($"[Audiomanager] Stopping sound {soundName}");
        if (position.HasValue)
        {
            string key = soundName + position.Value;
            if (sources.TryGetValue(key, out AudioSource source))
            {
                source.Stop();
                Debug.Log($"stopping {soundName} at {position}");
                sources.Remove(key);
                Destroy(source.gameObject);
            }
        }
        else
        {
            if (sources.TryGetValue(soundName, out AudioSource source))
            {
                source.Stop();
            }
        }

    }
}
