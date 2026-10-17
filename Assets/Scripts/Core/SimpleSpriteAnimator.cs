using UnityEngine;

/// <summary>Animation sprite đơn giản (thay Animator): chạy lần lượt các frame.</summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 12f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool randomStartFrame = false;
    [Tooltip("Hết animation (không loop) thì trả về pool")]
    [SerializeField] private bool despawnOnComplete = false;

    private SpriteRenderer sr;
    private float timer;
    private int currentFrame;
    private bool finished;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        finished = false;
        currentFrame = 0;
        if (frames == null || frames.Length == 0) return;

        if (randomStartFrame) currentFrame = Random.Range(0, frames.Length);
        timer = framesPerSecond > 0f ? currentFrame / framesPerSecond : 0f;
        sr.sprite = frames[currentFrame];
    }

    private void Update()
    {
        if (finished || frames == null || frames.Length == 0 || framesPerSecond <= 0f) return;

        timer += Time.deltaTime;
        int index = Mathf.FloorToInt(timer * framesPerSecond);

        if (index >= frames.Length)
        {
            if (loop)
            {
                index %= frames.Length;
                timer %= frames.Length / framesPerSecond;
            }
            else
            {
                finished = true;
                sr.sprite = frames[frames.Length - 1];
                if (despawnOnComplete) ReturnToPool();
                return;
            }
        }

        if (index != currentFrame)
        {
            currentFrame = index;
            sr.sprite = frames[currentFrame];
        }
    }

    private void ReturnToPool()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Despawn(gameObject);
        else Destroy(gameObject);
    }
}
