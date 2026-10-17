using UnityEngine;

/// <summary>Đánh dấu object thuộc pool (PoolManager tự thêm).</summary>
public class PooledObject : MonoBehaviour
{
    public GameObject SourcePrefab;
    public bool InPool;
}
