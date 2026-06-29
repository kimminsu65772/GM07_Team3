using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public sealed class SoundManager : Singleton<SoundManager>
{
    [Header("Sound Database")]
    [SerializeField] private SoundDatabaseSO soundDatabase;

    [Header("BGM AudioSource")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX AudioSource 설정")]
    [SerializeField, Min(1)] private int initialSfxSourceCount = 10;

    [SerializeField, Min(1)] private int maxSfxSourceCount = 32;

    [Header("Audio Mixer")] 
    [SerializeField] private AudioMixer masterAudioMixer;

    private readonly Dictionary<SoundId, SoundData> soundDictionary = new();

    private readonly Dictionary<SoundId, float> lastPlayTimes = new();

    private readonly List<SfxVoice> sfxVoices = new();

    private SoundId currentBgmId = SoundId.None;

    private const string MasterVolumeParameter = "MasterVolume";
    private const string SfxVolumeParameter = "SfxVolume";
    private const string BgmVolumeParameter = "BgmVolume";

    private const string MasterVolumeSaveKey = "Audio.MasterVolume";
    private const string SfxVolumeSaveKey = "Audio.SfxVolume";
    private const string BgmVolumeSaveKey = "Audio.BgmVolume";

    private const float MinimumDecibel = -80f;

    public float MasterVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;
    public float BgmVolume { get; private set; } = 1f;

    private sealed class SfxVoice
    {
        public AudioSource Source;

        public SoundId CurrentSoundId;
    }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }

        InitializeSoundDictionary();
        InitializeBgmSource();
        InitializeSfxVoices();

        LoadAudio();
    }
    private void Start()
    {
        ApplyAllAudio();
    }

    #region Audio Mixer 볼륨

    public void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp01(value);

        SetMixerVolume(MasterVolumeParameter, MasterVolume);

        PlayerPrefs.SetFloat(MasterVolumeSaveKey, MasterVolume);
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Clamp01(value);

        SetMixerVolume(SfxVolumeParameter, SfxVolume);

        PlayerPrefs.SetFloat(SfxVolumeSaveKey, SfxVolume);
    }

    public void SetBgmVolume(float value)
    {
        BgmVolume = Mathf.Clamp01(value);

        SetMixerVolume(BgmVolumeParameter, BgmVolume);

        PlayerPrefs.SetFloat(BgmVolumeSaveKey, BgmVolume);
    }

    public void SaveAudio()
    {
        PlayerPrefs.Save();
    }

    private void LoadAudio()
    {
        MasterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumeSaveKey, 1f));

        SfxVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(SfxVolumeSaveKey, 1f));

        BgmVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(BgmVolumeSaveKey, 1f));
    }

    private void ApplyAllAudio()
    {
        SetMixerVolume(MasterVolumeParameter, MasterVolume);

        SetMixerVolume(SfxVolumeParameter, SfxVolume);

        SetMixerVolume(BgmVolumeParameter, BgmVolume);
    }

    private void SetMixerVolume(string parameterName, float linearVolume)
    {
        if (masterAudioMixer == null)
        {
            Debug.LogError($"{name}: MasterAudioMixer가 연결되지 않았습니다.", this);

            return;
        }

        float decibel = Decibel(linearVolume);

        bool success = masterAudioMixer.SetFloat(parameterName, decibel);

        if (!success)
        {
            Debug.LogWarning($"Audio Mixer Parameter를 찾지 못했습니다: " + $"{parameterName}", this);
        }
    }

    private static float Decibel(float Volume)
    {
        if (Volume <= 0.0001f)
        {
            return MinimumDecibel;
        }

        return Mathf.Log10(Volume) * 20f;
    }

    private void OnApplicationQuit()
    {
        SaveAudio();
    }

    #endregion




    private void OnValidate()
    {
        if (maxSfxSourceCount < initialSfxSourceCount)
        {
            maxSfxSourceCount = initialSfxSourceCount;
        }
    }



    #region 초기화

    private void InitializeSoundDictionary()
    {
        soundDictionary.Clear();

        if (soundDatabase == null)
        {
            Debug.LogError($"{name}: SoundDatabase가 연결되지 않았습니다.", this);

            return;
        }

        foreach (SoundData soundData in soundDatabase.Sounds)
        {
            if (soundData == null)
            {
                continue;
            }

            SoundId soundId = soundData.SoundId;

            if (soundId == SoundId.None)
            {
                Debug.LogWarning($"{name}: SoundId가 None인 데이터가 있습니다.", this);

                continue;
            }

            if (soundDictionary.ContainsKey(soundId))
            {
                Debug.LogWarning($"{name}: {soundId}가 중복 등록되었습니다.", this);

                continue;
            }

            soundDictionary.Add(soundId, soundData);
        }
    }

    private void InitializeBgmSource()
    {
        if (bgmSource == null)
        {
            GameObject bgmObject = new GameObject("BGM_Source");

            bgmObject.transform.SetParent(transform, false);

            bgmSource = bgmObject.AddComponent<AudioSource>();
        }
        bgmSource.playOnAwake = false;

        bgmSource.spatialBlend = 0f;
    }

    private void InitializeSfxVoices()
    {
        for (int i = 0; i < initialSfxSourceCount; i++)
        {
            CreateSfxVoice();
        }
    }

    private SfxVoice CreateSfxVoice()
    {
        GameObject sourceObject = new GameObject($"SFX_Source_{sfxVoices.Count:00}");

        sourceObject.transform.SetParent(transform, false);

        AudioSource audioSource = sourceObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        SfxVoice voice = new SfxVoice
        {
            Source = audioSource,
            CurrentSoundId = SoundId.None
        };

        sfxVoices.Add(voice);

        return voice;
    }

    #endregion

    #region 외부 재생 API
    public void Play2D( SoundId soundId)
    {
        PlaySfx(soundId, transform.position, true);
    }
    public void PlayAt(SoundId soundId, Vector3 position)
    {
        PlaySfx(soundId, position, false);
    }
    public void PlayBgm(SoundId soundId)
    {
        if (!TryGetSoundData(soundId, out SoundData soundData))
        {
            return;
        }
        if (currentBgmId == soundId && bgmSource.isPlaying)
        {
            return;
        }

        if (soundData.Clip == null)
        {
            Debug.LogWarning($"{soundId}: AudioClip이 연결되지 않았습니다.", this);

            return;
        }

        bgmSource.Stop();

        ApplySoundData(bgmSource, soundData, soundData.Loop);

        bgmSource.spatialBlend = 0f;

        bgmSource.Play();

        currentBgmId = soundId;
    }

    public void StopBgm()
    {
        if (bgmSource == null)
        {
            return;
        }

        bgmSource.Stop();
        bgmSource.clip = null;

        currentBgmId = SoundId.None;
    }

    public void StopAllSfx()
    {
        foreach (SfxVoice voice in sfxVoices)
        {
            ResetVoice(voice);
        }
    }

    #endregion

    #region SFX 재생

    private void PlaySfx(SoundId soundId, Vector3 position, bool force2D)
    {
        if (!TryGetSoundData(soundId, out SoundData soundData))
        {
            return;
        }
        if (!CanPlay(soundData))
        {
            return;
        }
        if (soundData.Clip == null)
        {
            Debug.LogWarning($"{soundId}: AudioClip이 연결되지 않았습니다.", this);

            return;
        }
        SfxVoice voice = GetAvailableVoice();

        if (voice == null)
        {
            return;
        }

        voice.CurrentSoundId = soundId;

        AudioSource source = voice.Source;

        source.transform.position = position;

        ApplySoundData(source, soundData, false);

        if (force2D)
        {
            source.spatialBlend = 0f;
        }

        source.Play();

        lastPlayTimes[soundId] = Time.unscaledTime;
    }

    private bool TryGetSoundData(SoundId soundId,out SoundData soundData)
    {
        if (soundDictionary.TryGetValue(soundId, out soundData))
        {
            return true;
        }

        Debug.LogWarning($"{soundId}: SoundDatabase에 등록되지 않았습니다.", this);

        return false;
    }
    private bool CanPlay(SoundData soundData)
    {
        int simultaneousCount = 0;

        foreach (SfxVoice voice in sfxVoices)
        {
            if (!voice.Source.isPlaying)
            {
                continue;
            }

            if (voice.CurrentSoundId == soundData.SoundId)
            {
                simultaneousCount++;
            }
        }

        if (simultaneousCount >= soundData.MaxSimultaneous)
        {
            return false;
        }
        if (soundData.Cooldown <= 0f)
        {
            return true;
        }

        if (!lastPlayTimes.TryGetValue(soundData.SoundId, out float lastPlayTime))
        {
            return true;
        }

        float elapsedTime = Time.unscaledTime -lastPlayTime;

        return elapsedTime >= soundData.Cooldown;
    }

    private SfxVoice GetAvailableVoice()
    {
        foreach (SfxVoice voice in sfxVoices)
        {
            if (!voice.Source.isPlaying)
            {
                ResetVoice(voice);

                return voice;
            }
        }

        if (sfxVoices.Count < maxSfxSourceCount)
        {
            return CreateSfxVoice();
        }
        return null;
    }

    #endregion

    #region AudioSource 설정

    private void ApplySoundData(AudioSource source, SoundData soundData, bool loop)
    {
        source.Stop();

        source.clip = soundData.Clip;

        source.volume = soundData.Volume;

        source.pitch = GetPitch(soundData);

        source.spatialBlend = soundData.SpatialBlend;

        source.minDistance = soundData.MinDistance;

        source.maxDistance = soundData.MaxDistance;

        source.outputAudioMixerGroup = soundData.MixerGroup;

        source.loop = loop;
    }
    private float GetPitch(SoundData soundData)
    {
        Vector2 pitchRange = soundData.PitchRange;

        float minPitch = Mathf.Min(pitchRange.x, pitchRange.y);

        float maxPitch = Mathf.Max(pitchRange.x, pitchRange.y);

        return Random.Range(minPitch, maxPitch);
    }

    #endregion

    #region Voice 초기화

    private void ResetVoice(SfxVoice voice)
    {
        voice.Source.Stop();

        voice.Source.clip = null;
        voice.Source.loop = false;
        voice.Source.pitch = 1f;
        voice.Source.spatialBlend = 0f;

        voice.Source.transform.localPosition = Vector3.zero;

        voice.CurrentSoundId = SoundId.None;
    }

    #endregion
}