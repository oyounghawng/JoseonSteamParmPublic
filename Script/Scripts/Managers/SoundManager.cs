using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private GameObject _soundRoot = null;

    private float bgmVolume = 1.0f;
    public float BgmVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = value;
            _audioSources[(int)Define.Sound.Bgm].volume = bgmVolume;
        }
    }

    private float effectVolume = 1.0f;
    public float EffectVolume { get => effectVolume; set => effectVolume = value; }
    public override void Init()
    {
        if (_soundRoot == null)
        {
            _soundRoot = GameObject.Find("@SoundRoot");
            if (_soundRoot == null)
            {
                _soundRoot = new GameObject { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(_soundRoot);

                string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    _audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }

                _audioSources[(int)Define.Sound.Bgm].loop = true;
            }
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
            audioSource.Stop();
        _audioClips.Clear();
    }

    public void SetPitch(Define.Sound type, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)type];
        if (audioSource == null)
            return;

        audioSource.pitch = pitch;
    }

    public void Play(Define.Sound type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Play();
    }

    public bool Play(Define.Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        AudioSource audioSource = _audioSources[(int)type];
        if (path.Contains("Sounds/") == false)
            path = string.Format("Sounds/{0}", path);

        audioSource.volume = volume;

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
                return false;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;

            // Option
            if (Managers.Game.BGMOn)
                audioSource.Play();

            return true;
        }
        else if (type == Define.Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
                return false;

            audioSource.spatialBlend = 0;

            audioSource.pitch = pitch;

            // Option
            if (Managers.Game.EffectSoundOn)
                audioSource.PlayOneShot(audioClip);

            return true;
        }

        return false;
    }
    public bool Play(Define.Sound type, string path, Transform target, float volume = 1.0f, float pitch = 1.0f)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        AudioSource audioSource = _audioSources[(int)type];
        if (path.Contains("Sounds/") == false)
            path = string.Format("Sounds/{0}", path);

        audioSource.volume = volume;

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
                return false;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;

            // Option
            if (Managers.Game.BGMOn)
                audioSource.Play();

            return true;
        }
        else if (type == Define.Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
                return false;

            audioSource.pitch = pitch;
            audioSource.spatialize = true;
            audioSource.minDistance = 0.5f;
            audioSource.maxDistance = 2f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.transform.position = target.position;
            // Option
            if (Managers.Game.EffectSoundOn)
                audioSource.PlayOneShot(audioClip);

            return true;
        }

        return false;
    }
    public void Stop(Define.Sound type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }

    public float GetAudioClipLength(string path)
    {
        AudioClip audioClip = GetAudioClip(path);
        if (audioClip == null)
            return 0.0f;
        return audioClip.length;
    }

    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(path, out audioClip))
            return audioClip;

        audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add(path, audioClip);
        return audioClip;
    }
}
