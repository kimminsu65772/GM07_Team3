using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public sealed class SoundData
{
    [Header("식별 정보")]
    [SerializeField] private SoundId soundId = SoundId.None;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip clip;

    [Header("재생 설정")]
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    [SerializeField] private Vector2 pitchRange = new Vector2(1f, 1f);

    [SerializeField, Range(0f, 1f)] private float spatialBlend = 1f;

    [SerializeField] private bool loop;

    [Header("3D Sound")]
    [SerializeField, Min(0f)] private float minDistance = 1f;

    [SerializeField, Min(0f)] private float maxDistance = 30f;

    [Header("재생 제한")]
    [SerializeField, Min(1)] private int maxSimultaneous = 3;

    [SerializeField, Min(0f)] private float cooldown;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup mixerGroup;

    public SoundId SoundId => soundId;

    public AudioClip Clip => clip;

    public float Volume => volume;

    public Vector2 PitchRange => pitchRange;

    public float SpatialBlend => spatialBlend;

    public bool Loop => loop;

    public float MinDistance => minDistance;

    public float MaxDistance => Mathf.Max(minDistance, maxDistance);

    public int MaxSimultaneous => maxSimultaneous;

    public float Cooldown => cooldown;
    public AudioMixerGroup MixerGroup => mixerGroup;
}