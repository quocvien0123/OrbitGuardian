using UnityEngine;

/// <summary>Tiện ích hướng 2D. Quy ước: transform.up (+Y) là phía trước, góc 0° = hướng +Y.</summary>
public static class Direction2D
{
    /// <summary>Rotation để transform.up trùng với direction.</summary>
    public static Quaternion ToRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        return Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>Vector đơn vị theo góc (độ), 0° = +Y, tăng ngược chiều kim đồng hồ (khớp với ToRotation).</summary>
    public static Vector2 FromAngle(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
    }

    /// <summary>Xoay vector v một góc (độ), ngược chiều kim đồng hồ.</summary>
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
