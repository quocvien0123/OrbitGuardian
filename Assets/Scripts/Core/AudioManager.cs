using UnityEngine;

/// <summary>Phát nhạc nền và hiệu ứng âm thanh. Clip để trống vẫn an toàn.</summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip musicClip;

    private void Awake()
    {
        Instance = this;
        if (sfxSource == null)
            Debug.LogWarning("AudioManager: chưa gán Sfx Source, sẽ không có âm thanh hiệu ứng.", this);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        if (musicClip == null) return;
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager: có Music Clip nhưng chưa gán Music Source.", this);
            return;
        }
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>Phát một âm thanh hiệu ứng. An toàn khi clip hoặc AudioManager không có.</summary>
    public static void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null || Instance == null || Instance.sfxSource == null) return;
        Instance.sfxSource.PlayOneShot(clip, volume);
    }
}
