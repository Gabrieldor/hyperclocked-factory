using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance { get; private set; }
    public static event System.Action OnTick;

    [SerializeField] private float tickInterval = 1f;
    private float _elapsed;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= tickInterval)
        {
            _elapsed -= tickInterval;
            OnTick?.Invoke();
        }
    }
}
