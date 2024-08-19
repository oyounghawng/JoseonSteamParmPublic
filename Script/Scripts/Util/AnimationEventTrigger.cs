using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    public LayerMask mask;
    public void Play3DSound(string path)
    {
        SoundManager.Instance?.Play(Define.Sound.Effect, path, transform, Managers.Sound.EffectVolume);
    }
    public void PlaySoundEffect(string path)
    {
        SoundManager.Instance?.Play(Define.Sound.Effect, path, Managers.Sound.EffectVolume);
    }

}