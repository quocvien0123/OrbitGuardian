using UnityEngine;
using UnityEngine.UI;

/// <summary>Hiển thị số nguyên bằng các ô Image sprite (hỗ trợ số 0 đứng đầu, kẹp giá trị theo số ô).</summary>
public class SpriteNumberDisplay : MonoBehaviour
{
    [Tooltip("Các ô Image hiển thị chữ số từ trái sang phải")]
    [SerializeField] private Image[] digitSlots;

    [Tooltip("Danh sách 10 sprite chữ số từ 0 đến 9 (index = chữ số)")]
    [SerializeField] private Sprite[] digitSprites;

    private bool warnedMissingConfig;

    /// <summary>Cập nhật giá trị hiển thị. Số âm -> 0, vượt quá giới hạn -> kẹp tối đa.</summary>
    public void SetValue(int value)
    {
        if (digitSlots == null || digitSlots.Length == 0 || digitSprites == null || digitSprites.Length < 10)
        {
            if (!warnedMissingConfig)
            {
                Debug.LogWarning("SpriteNumberDisplay: thiếu cấu hình digitSlots hoặc digitSprites (cần đúng 10 sprite 0..9).", this);
                warnedMissingConfig = true;
            }
            return;
        }

        int slotCount = digitSlots.Length;
        int clampedVal = Mathf.Max(0, value);

        // Giới hạn tối đa theo số ô: 10^slotCount - 1 (ví dụ 3 ô -> 999)
        long maxPossible = 1;
        for (int i = 0; i < slotCount; i++) maxPossible *= 10;
        maxPossible -= 1;

        if (clampedVal > maxPossible)
        {
            clampedVal = (int)Mathf.Min(clampedVal, maxPossible);
        }

        int temp = clampedVal;
        for (int i = slotCount - 1; i >= 0; i--)
        {
            int digit = temp % 10;
            temp /= 10;

            Image slot = digitSlots[i];
            if (slot != null)
            {
                Sprite s = digitSprites[digit];
                if (s != null)
                {
                    slot.sprite = s;
                    slot.enabled = true;
                }
            }
        }
    }
}

