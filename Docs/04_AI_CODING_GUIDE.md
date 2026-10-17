# 04 – Hướng dẫn dùng AI viết code

## 1. Mở project trong VS Code

- Cách 1: Trong Unity, double-click một script bất kỳ, VS Code mở cả project.
- Cách 2: VS Code > **File > Open Folder** > chọn **thư mục gốc project** (thư mục chứa `Assets/` và `AGENTS.md`). **Không** mở thư mục `Assets/`.

## 2. Thiết lập trợ lý AI

| Công cụ | Chế độ cần dùng | Ghi chú |
|---|---|---|
| GitHub Copilot | Mở Chat, chọn **Agent** | Tự đọc được `AGENTS.md` ở thư mục gốc |
| Gemini Code Assist | Chat, bật **Agent mode** nếu có | |
| Antigravity | Agent panel | Thêm nội dung `AGENTS.md` vào Customizations > Rules nếu muốn |

Dù dùng công cụ nào, **đầu mỗi prompt luôn yêu cầu đọc tài liệu** (đã có sẵn trong các prompt bên dưới).

Nếu công cụ cho phép đính kèm file vào chat (nút `+`, `#file` hoặc `@file`), hãy đính kèm `AGENTS.md`, `Docs/01_GDD.md`, `Docs/02_REQUIREMENTS.md`, `Docs/ASSETS.md`.

## 3. Quy trình mỗi milestone

```
1. Mở cuộc trò chuyện MỚI với AI
2. Dán prompt của milestone (mục 5)
3. AI viết code → xem tab Problems trong VS Code (Ctrl+Shift+M), yêu cầu AI sửa hết lỗi
4. Chuyển sang Unity → chờ biên dịch → Console không có lỗi đỏ
   (lỗi đỏ: copy nguyên văn, dùng Prompt "Sửa lỗi" ở mục 6)
5. Lắp ráp theo 05_UNITY_BUILD_GUIDE.md, phần tương ứng
6. Bấm Play, test theo checklist
7. Dùng Prompt "Giải thích code" để hiểu code
8. Git commit
```

## 4. Prompt 0 – Lập danh sách asset (chạy một lần, sau khi làm xong 03_SETUP_PROJECT.md)

```
Đọc AGENTS.md, Docs/01_GDD.md và mẫu Docs/ASSETS.md.

Nhiệm vụ: điền đầy đủ Docs/ASSETS.md dựa trên thư mục Assets/Art/ và Assets/Audio/.
- Liệt kê mọi file ảnh và âm thanh, ghi đường dẫn đầy đủ.
- Với ảnh đã cắt thành nhiều sprite (Sprite Mode = Multiple), đọc file .meta
  cùng tên để lấy danh sách tên các sprite con và số frame.
- Phân loại theo đúng các mục có sẵn trong ASSETS.md: Player, Thruster, Bullets,
  Enemies (gợi ý loại nào hợp với Diver/Gunner/Kamikaze dựa trên tên file),
  Asteroids, Mines, Explosions, PowerUps (gợi ý icon nào ứng với TripleShot,
  Shield, ExtraLife), Backgrounds (thứ tự lớp xa đến gần), Crosshair, UI, Audio.
- Asset của boss, tàu địch và icon vật phẩm không dùng đến: ghi vào mục "Chưa dùng".
- Chỗ nào không chắc, ghi "(cần người dùng xác nhận)".
Chỉ sửa Docs/ASSETS.md, không tạo hay sửa file khác.
```

Sau khi AI làm xong, **mở `ASSETS.md` kiểm tra** bằng cách đối chiếu với ảnh thật trong Unity, sửa các mục "(cần người dùng xác nhận)". Commit.

## 5. Prompt cho từng milestone

### Prompt mẫu chung
Mọi prompt milestone đều có dạng:
```
Đọc AGENTS.md, Docs/01_GDD.md, Docs/02_REQUIREMENTS.md, Docs/ASSETS.md.

Thực hiện milestone [Mx] trong mục 5 của 02_REQUIREMENTS.md:
viết đúng các script được liệt kê, đúng thư mục, đúng tên class, field, method.
Chỉ viết script của [Mx]. Các script của milestone trước đã có trong Assets/Scripts/,
hãy đọc chúng để dùng đúng API.

[Yêu cầu riêng của milestone]

Khi xong:
1. Kiểm tra Problems, sửa hết lỗi biên dịch.
2. Tự đối chiếu với đặc tả, liệt kê những chỗ khác đặc tả (nếu có) và lý do.
3. Liệt kê file đã tạo.
4. Nhắc tôi làm phần [Mx] trong Docs/05_UNITY_BUILD_GUIDE.md và cách test.
```

Phần **[Yêu cầu riêng]** cho từng milestone:

**M1 – Nền tảng**
```
Viết trong Assets/Scripts/Core/: IDamageable, Direction2D, Arena, CameraFollow,
CameraShake, ParallaxLayer, PoolManager (kèm PoolPreload), PooledObject,
SimpleSpriteAnimator, HitFlash, Targetable, GameState, GameManager, LootManager, AudioManager.
Lưu ý phụ thuộc vào M2 (chưa có):
- CameraFollow: CHƯA khai báo field playerAim, dùng hướng ngắm = Vector2.zero,
  đánh dấu "// TODO M2".
- GameManager.WinGame: phần thưởng theo mạng để trong comment "// TODO M2".
Để code biên dịch được ngay.
```

**M2 – Người chơi & vũ khí**
```
Viết: PlayerMovement, PlayerAim, PlayerShooting, PlayerHealth, Crosshair (Scripts/Player/),
Bullet (Scripts/Weapons/).
Sau đó hoàn thiện các chỗ "// TODO M2" trong CameraFollow (thêm field playerAim theo
đặc tả, dùng playerAim.AimDirection cho lookAhead) và GameManager.WinGame.
Giải thích ngắn trong comment: cách đổi vị trí chuột sang tọa độ thế giới, và vì sao
dùng RotateTowards.
```

**M3 – Chướng ngại vật**
```
Viết: Asteroid, Mine trong Scripts/Hazards/.
Chú ý phần sinh mảnh vỡ của Asteroid: Spawn qua PoolManager rồi gọi Launch,
các mảnh tỏa đều theo góc quanh hướng bay hiện tại.
```

**M4 – Kẻ địch**
```
Viết: MovementPattern, EnemyData, Enemy trong Scripts/Enemies/.
Viết 3 kiểu di chuyển thành 3 hàm private riêng (MoveStraight, MoveShooter,
MoveKamikaze), mỗi hàm có comment giải thích công thức.
Dùng PlayerHealth.Instance để lấy vị trí người chơi, kiểm tra null và activeInHierarchy.
```

**M5 – Vật phẩm**
```
Viết: PowerUpType, PowerUp (Scripts/PowerUps/), PlayerPowerUps (Scripts/Player/).
```

**M6 – Sinh đối tượng**
```
Viết: SpawnEntry, SpawnConfig, SpawnManager trong Scripts/Spawning/.
SpawnConfig phải có giá trị mặc định cho AnimationCurve như đặc tả.
Viết hàm chọn ngẫu nhiên có trọng số thành hàm riêng PickWeightedEntry() có comment giải thích.
```

**M7 – Giao diện**
```
Viết: HUDController, GameUI, MainMenuController trong Scripts/UI/.
Dùng TextMeshPro (TMP_Text) và UnityEngine.UI.Image.
```

**M8 – Hoàn thiện**: không viết script mới. Dùng Prompt "Rà soát" ở mục 6.

## 6. Các prompt hỗ trợ

### Sửa lỗi
```
Đọc AGENTS.md. Unity Console báo lỗi sau:
[DÁN NGUYÊN VĂN LỖI, KÈM DÒNG "at ...cs:dòng"]
Hiện tượng khi chơi: [mô tả, ví dụ: đạn bắn ra nhưng không trúng thiên thạch]
Tìm nguyên nhân, giải thích ngắn gọn, rồi sửa. Nếu nguyên nhân nằm ở cài đặt
trong Unity (layer, collider, thiếu kéo thả...), hãy hướng dẫn tôi sửa trong Editor
thay vì sửa code.
```

### Giải thích code (dùng sau mỗi milestone để học)
```
Giải thích các script của milestone [Mx] cho người mới học Unity:
- Mỗi script làm gì, liên kết với script nào.
- Đi qua từng hàm quan trọng, giải thích bằng ví dụ cụ thể.
- 3 câu hỏi giảng viên có thể hỏi về phần này, kèm câu trả lời gợi ý.
Không sửa code.
```

### Rà soát (dùng ở M8)
```
Đọc AGENTS.md và Docs/02_REQUIREMENTS.md. Rà soát toàn bộ Assets/Scripts/:
1. Đối chiếu từng FR và NFR, đánh dấu đạt / chưa đạt / không chắc.
2. Tìm lỗi tiềm ẩn: NullReference, event chưa hủy đăng ký, object pool không reset
   trạng thái trong OnEnable, chỗ dùng Instantiate/Destroy sai quy tắc.
3. Đề xuất sửa, nhưng CHỜ tôi đồng ý rồi mới sửa.
```

### Khi AI làm sai nhiều
Đừng ra lệnh sửa chồng chéo. Làm theo các bước:
1. Dùng Git hủy thay đổi: GitHub Desktop > chuột phải các file > **Discard changes**.
2. Mở cuộc trò chuyện mới.
3. Gửi lại prompt, thêm mô tả chỗ sai lần trước, ví dụ: *"Lần trước bạn dùng Input.GetKey, sai quy tắc AGENTS.md, phải dùng Keyboard.current."*

## 7. Mẹo cho AI đọc lỗi Unity mà không phải copy

Unity ghi log vào file:
- Windows: `%LOCALAPPDATA%\Unity\Editor\Editor.log`
- Mac: `~/Library/Logs/Unity/Editor.log`

Có thể yêu cầu: *"Đọc 100 dòng cuối file Editor.log và tìm lỗi biên dịch C#."* (Công cụ AI cần có quyền đọc file ngoài thư mục project hoặc chạy lệnh terminal.)