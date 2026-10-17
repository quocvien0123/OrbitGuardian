using UnityEngine;

/// <summary>Một dòng trong bảng sinh: prefab, trọng số ngẫu nhiên và thời điểm mở khóa.</summary>
[System.Serializable]
public class SpawnEntry
{
    [Tooltip("Tên hiển thị trong Inspector (chỉ để dễ nhìn)")]
    public string label;
    [Tooltip("Prefab được sinh ra")]
    public GameObject prefab;
    [Tooltip("Trọng số: càng lớn càng dễ được chọn")]
    public float weight = 1f;
    [Tooltip("Số giây kể từ đầu lượt chơi để entry này bắt đầu xuất hiện")]
    public float unlockTime = 0f;
}
