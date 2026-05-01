using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Ambient Loops")]
    public AudioClip springAmbient;
    public AudioClip summerAmbient;
    public AudioClip autumnAmbient;
    public AudioClip winterAmbient;

    [Header("Sound Effects")]
    public AudioClip sfxPlaceCell;
    public AudioClip sfxBonusCombo;
    public AudioClip sfxHoneyFill;
    public AudioClip sfxSeasonChange;
    public AudioClip sfxPollenCollect;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume   = 0.8f;
    public float crossfadeDuration = 2f;

    AudioSource _musicA;
    AudioSource _musicB;
    AudioSource _sfxSource;
    bool _aIsPlaying = true;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        _musicA    = gameObject.AddComponent<AudioSource>();
        _musicB    = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { _musicA, _musicB })
        {
            src.loop = true; src.playOnAwake = false; src.volume = 0f;
        }
        _sfxSource.loop = false; _sfxSource.playOnAwake = false;

        // Resources.Load<AudioClip> path = everything inside Assets/Resources/
        // with NO file extension. File at:
        //   Assets/Resources/Audio/bees buzzing ambient.wav
        // loads as:
        //   "Audio/bees buzzing ambient"
        TryLoad(ref springAmbient,    "Audio/bees buzzing ambient");
        TryLoad(ref summerAmbient,    "Audio/summer nature birds");
        TryLoad(ref autumnAmbient,    "Audio/autumn wind leaves");
        TryLoad(ref winterAmbient,    "Audio/winter wind howling");
        TryLoad(ref sfxPlaceCell,     "Audio/soft_click");
        TryLoad(ref sfxBonusCombo,    "Audio/chime ding");
        TryLoad(ref sfxHoneyFill,     "Audio/chime ding");
        TryLoad(ref sfxSeasonChange,  "Audio/chime ding");
        TryLoad(ref sfxPollenCollect, "Audio/soft_click");
    }

    void TryLoad(ref AudioClip clip, string path)
    {
        if (clip != null) return;

        clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            // Fallback: scan all clips in Resources/Audio and fuzzy-match by name
            string fragment = System.IO.Path.GetFileName(path).ToLowerInvariant();
            var all = Resources.LoadAll<AudioClip>("Audio");
            foreach (var c in all)
            {
                string n = c.name.ToLowerInvariant().Replace(".wav","").Replace(".mp3","");
                if (n == fragment || n.Contains(fragment))
                {
                    clip = c;
                    Debug.Log($"[AudioManager] Fallback matched '{c.name}' for '{path}'");
                    return;
                }
            }
            Debug.LogError($"[AudioManager] FAILED '{path}'. Assign in Inspector or check filename.");
        }
        else
        {
            Debug.Log($"[AudioManager] Loaded: {path}");
        }
    }

    void Start() => PlayMusic(springAmbient, instant: true);

    public void OnSeasonChanged(Season season)
    {
        AudioClip clip = season switch
        {
            Season.Spring => springAmbient,
            Season.Summer => summerAmbient,
            Season.Autumn => autumnAmbient,
            Season.Winter => winterAmbient,
            _             => springAmbient
        };
        PlayMusic(clip);
        PlaySFX(sfxSeasonChange);
    }

    void PlayMusic(AudioClip clip, bool instant = false)
    {
        if (clip == null) { Debug.LogWarning("[AudioManager] null clip in PlayMusic"); return; }
        StartCoroutine(CrossfadeTo(clip, instant ? 0f : crossfadeDuration));
    }

    IEnumerator CrossfadeTo(AudioClip newClip, float duration)
    {
        AudioSource incoming = _aIsPlaying ? _musicB : _musicA;
        AudioSource outgoing = _aIsPlaying ? _musicA : _musicB;

        incoming.clip = newClip; incoming.volume = 0f; incoming.Play();

        if (duration <= 0f)
        {
            outgoing.Stop(); incoming.volume = musicVolume;
            _aIsPlaying = !_aIsPlaying; yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; float t = Mathf.Clamp01(elapsed / duration);
            incoming.volume = Mathf.Lerp(0f, musicVolume, t);
            outgoing.volume = Mathf.Lerp(musicVolume, 0f, t);
            yield return null;
        }
        outgoing.Stop(); incoming.volume = musicVolume; _aIsPlaying = !_aIsPlaying;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || _sfxSource == null) return;
        _sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayPlaceCell()     => PlaySFX(sfxPlaceCell);
    public void PlayBonusCombo()    => PlaySFX(sfxBonusCombo);
    public void PlayHoneyFill()     => PlaySFX(sfxHoneyFill);
    public void PlayPollenCollect() => PlaySFX(sfxPollenCollect);
}