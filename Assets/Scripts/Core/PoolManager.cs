using System.Collections.Generic;
using UnityEngine;

/// <summary>Cấu hình tạo sẵn object cho pool.</summary>
[System.Serializable]
public class PoolPreload
{
    public GameObject prefab;
    public int count = 10;
}

/// <summary>Object Pool dùng chung. Mọi đạn, địch, thiên thạch, mìn, vật phẩm, hiệu ứng phải Spawn/Despawn qua đây.</summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Tooltip("Danh sách prefab tạo sẵn khi vào game")]
    [SerializeField] private List<PoolPreload> preload = new List<PoolPreload>();

    private readonly Dictionary<GameObject, Stack<PooledObject>> pools = new Dictionary<GameObject, Stack<PooledObject>>();
    private readonly Dictionary<GameObject, Transform> containers = new Dictionary<GameObject, Transform>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        foreach (PoolPreload p in preload)
        {
            if (p == null || p.prefab == null) continue;
            for (int i = 0; i < p.count; i++)
            {
                PooledObject po = CreateNew(p.prefab, Vector3.zero, Quaternion.identity);
                Despawn(po.gameObject);
            }
        }
    }

    /// <summary>Lấy object từ pool (hoặc tạo mới), đặt vị trí trước khi active.</summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("PoolManager.Spawn: prefab bị trống.", this);
            return null;
        }

        Stack<PooledObject> stack = GetStack(prefab);
        while (stack.Count > 0)
        {
            PooledObject po = stack.Pop();
            if (po == null) continue; // object đã bị Destroy ngoài ý muốn
            po.transform.SetPositionAndRotation(position, rotation);
            po.InPool = false;
            po.gameObject.SetActive(true);
            return po.gameObject;
        }
        return CreateNew(prefab, position, rotation).gameObject;
    }

    /// <summary>Trả object về pool. Object không thuộc pool thì Destroy. Gọi 2 lần vẫn an toàn.</summary>
    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        PooledObject po = obj.GetComponent<PooledObject>();
        if (po == null || po.SourcePrefab == null)
        {
            Destroy(obj);
            return;
        }
        if (po.InPool) return;

        po.InPool = true;
        obj.SetActive(false);
        obj.transform.SetParent(GetContainer(po.SourcePrefab), false);
        GetStack(po.SourcePrefab).Push(po);
    }

    private PooledObject CreateNew(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, position, rotation, GetContainer(prefab));
        PooledObject po = obj.GetComponent<PooledObject>();
        if (po == null) po = obj.AddComponent<PooledObject>();
        po.SourcePrefab = prefab;
        po.InPool = false;
        return po;
    }

    private Stack<PooledObject> GetStack(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out Stack<PooledObject> stack))
        {
            stack = new Stack<PooledObject>();
            pools[prefab] = stack;
        }
        return stack;
    }

    private Transform GetContainer(GameObject prefab)
    {
        if (!containers.TryGetValue(prefab, out Transform container) || container == null)
        {
            container = new GameObject("Pool_" + prefab.name).transform;
            container.SetParent(transform, false);
            containers[prefab] = container;
        }
        return container;
    }
}
