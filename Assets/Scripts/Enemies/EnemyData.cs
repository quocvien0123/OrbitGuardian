using UnityEngine;

/// <summary>Thông số của một loại kẻ địch (ScriptableObject). Tạo asset trong Assets/Data/.</summary>
[CreateAssetMenu(menuName = "SpaceShooter/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Chung")]
    public int maxHealth = 1;
    public float moveSpeed = 4f;
    public int scoreValue = 10;
    [Range(0f, 1f)] public float dropChance = 0.05f;
    public MovementPattern pattern;
    [Tooltip("Độ lệch ngẫu nhiên (±độ) khi chọn hướng ban đầu")]
    public float aimSpread = 10f;

    [Header("Shooter")]
    [Tooltip("Khoảng cách giữ với người chơi khi bay vòng")]
    public float preferredDistance = 6f;
    [Tooltip("Tốc độ bay vòng (độ/giây)")]
    public float orbitSpeed = 40f;
    [Tooltip("Thời gian ở lại bay vòng trước khi bay đi (giây)")]
    public float stayDuration = 10f;
    public float fireInterval = 1.4f;
    public GameObject bulletPrefab;

    [Header("Kamikaze")]
    [Tooltip("Thời gian trôi chậm trước khi lao vào (giây)")]
    public float dashDelay = 0.8f;
    public float dashSpeed = 7f;
    [Tooltip("Tốc độ xoay tối đa khi bám đuổi (độ/giây)")]
    public float turnSpeed = 120f;
}
