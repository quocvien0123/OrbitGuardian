# AGENTS.md – Quy tắc cho AI khi làm việc với project "Vệ Binh Quỹ Đạo"

> File này đặt ở **thư mục gốc** của project Unity (cùng cấp với thư mục `Assets/`).
> Mọi AI agent (Copilot, Gemini Code Assist, Antigravity...) phải đọc file này trước khi làm bất cứ việc gì.

## 1. Bối cảnh

- Game 2D **arena shooter**, màn hình **ngang 16:9**, nhìn từ trên xuống.
- Tàu bay tự do trong một **bản đồ rộng hình chữ nhật** (lớn hơn màn hình), **camera đi theo tàu**.
- Kẻ địch và chướng ngại vật xuất hiện ngẫu nhiên **ngay ngoài tầm nhìn camera, từ mọi phía**.
- Điều khiển: WASD di chuyển có quán tính, **chuột để ngắm** (tàu xoay theo con trỏ), **Q bật/tắt tự ngắm** mục tiêu gần nhất, Shift lướt né.
- Một màn chơi duy nhất, sống sót 3 phút để thắng.
- Engine: **Unity 6 LTS**, template **Universal 2D**, ngôn ngữ **C#**.
- Tài liệu bắt buộc phải đọc trước khi code:
  - `Docs/01_GDD.md` – thiết kế game
  - `Docs/02_REQUIREMENTS.md` – yêu cầu và **đặc tả kỹ thuật từng script** (nguồn sự thật duy nhất)
  - `Docs/ASSETS.md` – danh sách asset có trong project

## 2. Quy tắc quan trọng nhất

1. **Làm đúng đặc tả.** Tên class, tên file, tên field `[SerializeField]`, tên method public phải **giống hệt** mục 5 của `02_REQUIREMENTS.md`. Hướng dẫn lắp ráp trong Unity (`05_UNITY_BUILD_GUIDE.md`) dựa vào chính những cái tên này.
2. **Chỉ làm milestone được yêu cầu.** Không viết trước script của milestone sau.
3. **Không sửa trực tiếp** file `.unity`, `.prefab`, `.asset`, `.meta`. Chỉ tạo và sửa file `.cs` (và file `.md` trong `Docs/` khi được yêu cầu).
4. Nếu buộc phải thay đổi đặc tả, **hỏi người dùng trước**, sau đó cập nhật `02_REQUIREMENTS.md` và `05_UNITY_BUILD_GUIDE.md` cho khớp.

## 3. Quy tắc code

- Mỗi class một file, tên file trùng tên class, đặt đúng thư mục trong đặc tả (`Assets/Scripts/<Nhóm>/`). Không dùng namespace.
- Field cấu hình dùng `[SerializeField] private`, có giá trị mặc định như đặc tả, thêm `[Tooltip("...")]` tiếng Việt cho field quan trọng.
- Input: dùng **Input System package** qua `UnityEngine.InputSystem.Keyboard.current` và `Mouse.current`. **Không** dùng class `UnityEngine.Input` cũ và **không** tạo Input Action Asset.
- Text UI dùng **TextMeshPro** (`TMPro.TMP_Text`).
- Không dùng Animator. Animation sprite dùng `SimpleSpriteAnimator`.
- **Quy ước hướng:** mọi đối tượng có hướng (tàu, địch, đạn) coi **`transform.up` (+Y) là phía trước**. Đổi hướng → rotation dùng `Direction2D.ToRotation(dir)`.
- Đạn, kẻ địch, thiên thạch, mìn, vật phẩm, hiệu ứng nổ: **luôn** tạo bằng `PoolManager.Instance.Spawn(...)` và trả về bằng `PoolManager.Instance.Despawn(...)`. **Không** gọi `Instantiate`/`Destroy` trực tiếp cho các đối tượng này.
- Trạng thái của đối tượng pool được **khởi tạo lại trong `OnEnable()`**.
- Điều kiện xóa đối tượng khi đi quá xa: dùng chung `Arena.Instance.ShouldDespawn(position)`.
- Va chạm: mọi collider là **Trigger**, mọi Rigidbody2D là **Kinematic**, xử lý trong `OnTriggerEnter2D`. Di chuyển bằng `transform` trong `Update()` với `Time.deltaTime`.
- Manager dùng singleton đơn giản: `public static X Instance { get; private set; }` gán trong `Awake()`.
- UI nhận dữ liệu qua **C# event**, không đọc liên tục trong `Update()` (ngoại lệ duy nhất: thanh hồi chiêu Dash được đọc mỗi frame). Hủy đăng ký event trong `OnDestroy()`.
- Không gọi `FindObjectOfType`, `GameObject.Find`, `GetComponent` trong `Update()`. Cache trong `Awake()`/`Start()`. Tìm mục tiêu dùng danh sách `Targetable.All`.
- Tham chiếu tùy chọn (âm thanh, hiệu ứng) phải an toàn khi để trống. Tham chiếu bắt buộc bị trống: `Debug.LogWarning` rõ ràng.
- Comment ngắn gọn **bằng tiếng Việt** cho class và hàm quan trọng.

## 4. Quy trình mỗi lần làm việc

1. Đọc file này, `01_GDD.md`, mục liên quan trong `02_REQUIREMENTS.md`, và `ASSETS.md`.
2. Viết code cho đúng milestone được yêu cầu.
3. Kiểm tra tab **Problems**, sửa hết lỗi biên dịch.
4. Tự đối chiếu: tên class, field, method có khớp đặc tả không.
5. Báo cáo: danh sách file đã tạo hoặc sửa; những gì cần làm trong Unity (trỏ tới mục trong `05_UNITY_BUILD_GUIDE.md`); cách test nhanh.
