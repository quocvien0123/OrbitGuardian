# 05 – Hướng dẫn lắp ráp trong Unity

Mỗi phần **Mx** làm **sau khi** AI viết xong code của milestone đó và Console không còn lỗi đỏ.
Tên sprite cụ thể: tra trong `Docs/ASSETS.md`.

## Thao tác cơ bản cần biết

| Thao tác | Cách làm |
|---|---|
| Tạo GameObject từ sprite | Kéo sprite từ Project window thả vào Hierarchy |
| Thêm component | Chọn object > Inspector > **Add Component** > gõ tên script |
| Gán tham chiếu | Kéo object (Hierarchy) hoặc prefab/asset (Project) thả vào ô trong Inspector |
| Gán nhiều sprite vào mảng | Chọn các sprite con trong Project (Shift + click), kéo thả lên **tên của mảng** (ví dụ chữ "Frames") |
| Tạo prefab | Kéo object từ Hierarchy thả vào thư mục trong Project |
| Sửa prefab | Double-click prefab trong Project, sửa xong bấm mũi tên quay lại |
| Lưu thay đổi của object trong scene vào prefab | Inspector > **Overrides** > **Apply All** |
| Xem biến private khi đang chạy | Inspector > nút ⋮ góc trên phải > **Debug** |

Quy ước:
- **Mũi của mọi đối tượng hướng lên (+Y).** Sprite hướng xuống thì tích **Flip Y** ở Sprite Renderer.
- Collider dùng **Circle Collider 2D**, bán kính khoảng **70–80% hình**, luôn tích **Is Trigger**.
- Rigidbody 2D luôn là **Kinematic**.

---

## Phần 0 – Cài đặt dự án (làm một lần, trước M1)

### 0.1 Layer
**Edit > Project Settings > Tags and Layers > Layers**:
`User Layer 6 = Player`, `7 = PlayerBullet`, `8 = Enemy`, `9 = EnemyBullet`, `10 = Hazard`, `11 = PowerUp`.

### 0.2 Sorting Layer
Cùng trang, mục **Sorting Layers**, bấm `+` thêm `Background`, `Gameplay`, `Player`, `Effects`. Kéo thả (nắm biểu tượng `=`) để thứ tự từ trên xuống là:

**Background → Default → Gameplay → Player → Effects**

Layer càng ở dưới danh sách càng được vẽ đè lên trên.

### 0.3 Ma trận va chạm
**Edit > Project Settings > Physics 2D > Layer Collision Matrix**. Bỏ tích tất cả, rồi chỉ tích:
- Player ↔ Enemy, EnemyBullet, Hazard, PowerUp
- PlayerBullet ↔ Enemy, Hazard

### 0.4 Khung hình Game view
Mở scene `Assets/Scenes/Game`. Tab **Game** > ô độ phân giải > chọn **Full HD (1920x1080)** hoặc **16:9 Aspect**.

### 0.5 Lưu
Ctrl+S. Commit: `Cai dat layer`.

---

## M1 – Nền tảng

### M1.1 Systems
1. Hierarchy > chuột phải > **Create Empty**, tên `Systems`, Position `(0,0,0)`.
2. Add Component: `GameManager`, `PoolManager`, `LootManager`, `AudioManager`.
3. Tạo 2 object con `Music` và `SFX`, mỗi cái Add Component **Audio Source**, bỏ tích **Play On Awake**. `Music` tích **Loop**, Volume `0.4`.
4. Kéo `Music` vào ô **Music Source**, `SFX` vào ô **Sfx Source** của AudioManager.

### M1.2 Bản đồ (Arena)
1. Create Empty tên `Arena`, Position `(0,0,0)`.
2. Add Component `Arena`: Size `(64, 36)`.
3. Add Component **Line Renderer**:
   - **Materials > Element 0**: bấm vòng tròn nhỏ, chọn **Sprite-Unlit-Default**
   - **Width**: `0.15`
   - **Color**: xanh lơ sáng
   - Tích **Loop** và **Use World Space**
   - **Sorting Layer**: Gameplay (trong mục *Additional Settings*)
4. Kéo chính component Line Renderer (hoặc object `Arena`) vào ô **Border** của Arena.
5. Chọn `Arena` trong Scene view: thấy khung chữ nhật màu vàng (Gizmo) là đúng.

### M1.3 Camera đi theo
1. Create Empty tên `CameraRig`, Position `(0, 0, -10)`.
2. Kéo **Main Camera** thả **vào trong** `CameraRig`. Đặt Main Camera Position `(0, 0, 0)` (vị trí cục bộ so với Rig).
3. Main Camera: Projection **Orthographic**, Size **6**, Background **đen**, Tag vẫn là **MainCamera**.
4. Main Camera > Add Component `CameraShake`.
5. `CameraRig` > Add Component `CameraFollow`. Ô **Target** để trống (gán ở M2).

Vì sao tách 2 object: `CameraFollow` di chuyển Rig, `CameraShake` rung camera con, hai việc không đè lên nhau.

### M1.4 Nền parallax
1. Create Empty tên `Background`, Position `(0,0,0)`.
2. Kéo sprite **nền lớp 1** vào trong `Background`, tên `BG_1`.
3. Inspector `BG_1`: Sorting Layer **Background**, Order `0`. Add Component `ParallaxLayer`, **Parallax Factor** theo ASSETS.md (lớp 1 là `0.95`).
4. Lặp lại `BG_2` … `BG_5`: Order `1…4`, Parallax Factor `0.85, 0.7, 0.5, 0.3`. Lớp nào lộ đường nối khi bay ngang thì bỏ lớp đó (2–3 lớp là đủ).
5. Kéo `Background` vào `Assets/Prefabs/` tạo prefab (dùng lại ở menu).

### M1.5 Hiệu ứng nổ
1. Kéo **frame đầu** animation nổ to vào Hierarchy, tên `Explosion_Big`. Sorting Layer **Effects**.
2. Add `SimpleSpriteAnimator`: kéo các frame vào **Frames**, FPS `14`, **bỏ tích** Loop, **tích** Despawn On Complete.
3. Kéo vào `Assets/Prefabs/Effects/`, xóa khỏi Hierarchy.
4. Làm tương tự `Explosion_Small` (animation khác hoặc Scale `0.6`).

### M1 – Test
- [ ] Play: thấy viền bản đồ; nền phủ kín màn hình.
- [ ] Khi đang Play, chọn `CameraRig`, kéo nó đi trong Scene view: các lớp nền trượt với tốc độ khác nhau (có chiều sâu), không lộ mép ảnh.
- [ ] Kéo `Explosion_Big` vào scene, Play: nổ một lần rồi biến mất.
- [ ] Systems (Inspector Debug): Elapsed Time tăng; Esc thì dừng.
- [ ] Xóa object test. Ctrl+S. Commit: `M1 nen tang`.

---

## M2 – Người chơi & vũ khí

### M2.1 Đạn người chơi
1. Kéo sprite **đạn người chơi** vào Hierarchy, tên `PlayerBullet`.
2. Layer **PlayerBullet**. Sorting **Gameplay**, Order `5`.
3. **Rigidbody 2D** Kinematic. **Circle Collider 2D** Is Trigger, vừa viên đạn.
4. Add `Bullet`: Speed `14`, Damage `1`, Lifetime `1.5`.
5. Đầu đạn phải hướng **lên**. Nếu sprite nằm ngang: đặt sprite vào object con và xoay object con 90°, không xoay object gốc.
6. Kéo vào `Assets/Prefabs/Weapons/`, xóa khỏi Hierarchy.

### M2.2 Tàu người chơi
1. Kéo sprite **tàu người chơi** vào Hierarchy, tên `Player`, Position `(0,0,0)`.
2. Tag **Player**. Layer **Player** (*Yes, change children*). Sorting **Player**, Order `0`.
3. **Rigidbody 2D** Kinematic. **Circle Collider 2D** Is Trigger, bán kính ~70% thân tàu.
4. Add Component: `PlayerMovement`, `PlayerAim`, `PlayerShooting`, `PlayerHealth`.

### M2.3 Object con của Player
| Con | Cách tạo | Cài đặt |
|---|---|---|
| `FirePoint` | Create Empty | Local Position ở **mũi tàu**, ví dụ `(0, 0.6, 0)` |
| `Thruster` | Kéo frame đầu lửa động cơ vào trong Player | Local Position ở **đuôi tàu** `(0, -0.6, 0)`. Sorting **Player**, Order `-1`. Add `SimpleSpriteAnimator`: frames, FPS `12`, Loop |
| `Shield` | **2D Object > Sprites > Circle** | Scale `(1.6, 1.6, 1)`, màu xanh lơ, Alpha ~80. Sorting **Player**, Order `1`. **Bỏ tích** để ẩn. Xóa Collider nếu có |

### M2.4 Gán tham chiếu cho Player
| Component | Ô | Kéo vào |
|---|---|---|
| PlayerShooting | Bullet Prefab | prefab `PlayerBullet` |
| PlayerShooting | Fire Point | con `FirePoint` |
| PlayerHealth | Explosion Prefab | prefab `Explosion_Big` |
| PlayerHealth | Shield Visual | con `Shield` |
| PlayerMovement | Dash Effect Prefab | (tùy chọn) `Explosion_Small` |

Kéo `Player` vào `Assets/Prefabs/Player/` (giữ trong scene).

### M2.5 Tâm ngắm
1. Kéo sprite tâm ngắm (xem ASSETS.md) vào Hierarchy (ở **gốc**, không nằm trong Player), tên `Crosshair`.
2. Sorting **Effects**, Order `10`. Chỉnh Scale cho vừa (khoảng 0.5 đơn vị). Không cần Collider, Rigidbody.
3. Add `Crosshair`: **Player Aim** = object `Player`.

### M2.6 Nối camera với người chơi
`CameraRig` > CameraFollow: **Target** = `Player`, **Player Aim** = `Player`.

### M2.7 Pool
Systems > PoolManager > **Preload** > `+`: `PlayerBullet`, Count `40`.

### M2 – Test
- [ ] WASD: tàu tăng tốc rồi giảm tốc mượt, đi chéo không nhanh hơn. Không ra khỏi viền bản đồ.
- [ ] Camera bám theo mượt, hơi nhìn về phía chuột. Bay tới góc bản đồ: camera dừng lại, thấy viền.
- [ ] Tàu xoay theo chuột; con trỏ hệ thống biến mất, thay bằng tâm ngắm.
- [ ] Giữ chuột trái hoặc Space: bắn theo hướng mũi tàu. Bay lùi vẫn bắn được về phía trước.
- [ ] Shift: lướt nhanh một đoạn; bấm liên tục thì phải chờ hồi chiêu.
- [ ] Q: chế độ đổi (xem PlayerAim ở Inspector Debug, mục Is Auto Aim). Chưa có mục tiêu nên chưa tự bắn, đúng thiết kế.
- [ ] Esc: tàu đứng yên, con trỏ hệ thống hiện lại.
- [ ] Ctrl+S. Commit: `M2 nguoi choi`.

---

## M3 – Chướng ngại vật

Thứ tự **Small → Medium → Large**.

### M3.1 Thiên thạch (làm 3 lần)
1. Kéo sprite thiên thạch vào Hierarchy, đặt tên theo bảng.
2. Layer **Hazard**. Sorting **Gameplay**, Order `0`.
3. **Rigidbody 2D** Kinematic. **Circle Collider 2D** Is Trigger.
4. Add Component `Asteroid`, `Targetable`, `HitFlash`.
5. Điền theo bảng, kéo vào `Assets/Prefabs/Hazards/`, xóa khỏi Hierarchy.

| Prefab | Max Health | Score Value | Split Prefab | Explosion Prefab |
|---|---|---|---|---|
| `Asteroid_Small` | 1 | 10 | (trống) | Explosion_Small |
| `Asteroid_Medium` | 2 | 20 | **Asteroid_Small** | Explosion_Small |
| `Asteroid_Large` | 3 | 30 | **Asteroid_Medium** | Explosion_Big |

### M3.2 Mìn
1. Kéo frame đầu của mìn vào Hierarchy, tên `Mine`. Layer **Hazard**, Sorting **Gameplay**.
2. **Rigidbody 2D** Kinematic, **Circle Collider 2D** Is Trigger.
3. Add `SimpleSpriteAnimator` (frames, Loop, Random Start Frame).
4. Add `Mine`: **Player Layer** = `Player`, Explosion Prefab = `Explosion_Big`.
5. Add `Targetable`, `HitFlash`. Tạo prefab, xóa khỏi Hierarchy.

### M3 – Test
Kéo 2 `Asteroid_Large` và 1 `Mine` vào scene, cách Player khoảng 5–7 đơn vị ở các hướng khác nhau.
- [ ] Thiên thạch trôi về phía tàu, tự xoay. Bắn: nháy đỏ, vỡ 2 vừa → nhỏ → nổ.
- [ ] Bấm **Q**: tâm ngắm đỏ, dính vào thiên thạch gần nhất, tàu tự xoay và tự bắn. Hạ xong tự chuyển mục tiêu khác.
- [ ] Đâm vào thiên thạch: mất mạng, nhấp nháy, camera rung.
- [ ] **Lướt xuyên qua** thiên thạch: không mất mạng.
- [ ] Bắn mìn từ xa: an toàn. Đứng sát rồi bắn: bị sát thương.
- [ ] Xóa object test. Ctrl+S. Commit: `M3 chuong ngai vat`.

---

## M4 – Kẻ địch

### M4.1 Đạn địch
1. Project > chọn `PlayerBullet` > **Ctrl+D**, đổi tên `EnemyBullet`.
2. Mở prefab: sprite **đạn địch**, Layer **EnemyBullet** (Yes, change children), Bullet > Speed `8`, Lifetime `3`.

### M4.2 Dữ liệu kẻ địch
`Assets/Data/` > chuột phải > **Create > SpaceShooter > Enemy Data**, tạo 3 asset:

| Asset | Pattern | Max Health | Move Speed | Score Value | Drop Chance | Thông số riêng |
|---|---|---|---|---|---|---|
| `Enemy_Diver` | Straight | 1 | 6 | 10 | 0.04 | Aim Spread 10 |
| `Enemy_Gunner` | Shooter | 4 | 3 | 40 | 0.15 | Preferred Distance 6, Orbit Speed 40, Stay Duration 10, Fire Interval 1.4, **Bullet Prefab = EnemyBullet** |
| `Enemy_Kamikaze` | Kamikaze | 1 | 2 | 30 | 0.06 | Dash Delay 0.8, Dash Speed 7, Turn Speed 120 |

### M4.3 Prefab kẻ địch (làm 3 lần)
1. Kéo sprite kẻ địch vào Hierarchy, đặt tên trùng tên asset (`Enemy_Diver`...).
2. Layer **Enemy**. Sorting **Gameplay**, Order `2`.
3. Mũi phải **hướng lên**: nếu sprite hướng xuống, tích **Flip Y**.
4. **Rigidbody 2D** Kinematic. **Circle Collider 2D** Is Trigger.
5. Add `Enemy`: **Data** = asset tương ứng, Explosion Prefab = `Explosion_Small`.
6. Add `Targetable`, `HitFlash`. Tạo prefab, xóa khỏi Hierarchy.

### M4 – Test
Kéo 3 prefab vào scene, mỗi con một phía quanh Player, cách khoảng 8 đơn vị.
- [ ] Mọi kẻ địch quay mặt về phía tàu khi xuất hiện.
- [ ] Diver lao thẳng tới; dịch sang bên thì nó bay vượt qua.
- [ ] Gunner dừng ở khoảng cách 6, bay vòng quanh, bắn vào tàu; sau ~10 giây bay đi.
- [ ] Kamikaze khựng lại rồi bám đuổi; vòng gấp hoặc lướt thì né được.
- [ ] Tự ngắm (Q) khóa được kẻ địch.
- [ ] Xóa object test. Ctrl+S. Commit: `M4 ke dich`.

---

## M5 – Vật phẩm

### M5.1 Prefab vật phẩm (làm 3 lần)
1. Kéo icon vào Hierarchy, tên: `PowerUp_TripleShot`, `PowerUp_Shield`, `PowerUp_ExtraLife`.
2. Layer **PowerUp**. Sorting **Gameplay**, Order `3`.
3. **Rigidbody 2D** Kinematic, **Circle Collider 2D** Is Trigger.
4. Add `PowerUp`, chọn đúng **Type**. **Không** thêm `Targetable` (để tự ngắm không bắn vào vật phẩm).
5. Icon có nhiều frame: Add `SimpleSpriteAnimator`, Loop.
6. Tạo prefab trong `Assets/Prefabs/PowerUps/`, xóa khỏi Hierarchy.

### M5.2 Gắn vào Player và LootManager
1. `Player` > Add Component `PlayerPowerUps` > **Overrides > Apply All**.
2. Systems > LootManager > **Power Up Prefabs**: kéo 3 prefab vào.

### M5 – Test
Kéo 3 vật phẩm vào scene quanh Player.
- [ ] Vật phẩm nhấp nhô; sau 7 giây nhấp nháy, 10 giây thì biến mất.
- [ ] TripleShot chạy đúng 8 giây; nhặt thêm khi đang có thì thời gian được làm mới.
- [ ] Shield chặn 1 đòn. ExtraLife tăng mạng khi đang thiếu.
- [ ] Test rơi đồ: tạm đặt Drop Chance của `Enemy_Diver` = `1`, bắn hạ một Diver. **Trả về `0.04`.**
- [ ] Xóa object test. Ctrl+S. Commit: `M5 vat pham`.

---

## M6 – Sinh đối tượng ngẫu nhiên

### M6.1 Cấu hình
1. `Assets/Data/` > **Create > SpaceShooter > Spawn Config**, tên `SpawnConfig_Default`.
2. Entries:

| Label | Prefab | Weight | Unlock Time |
|---|---|---|---|
| Asteroid L | Asteroid_Large | 3 | 0 |
| Asteroid M | Asteroid_Medium | 2 | 0 |
| Diver | Enemy_Diver | 4 | 0 |
| Mine | Mine | 1 | 30 |
| Gunner | Enemy_Gunner | 1.5 | 60 |
| Kamikaze | Enemy_Kamikaze | 1.5 | 90 |

3. Kiểm tra đường cong **Spawn Interval Over Time**: đầu 1.4, cuối 0.35. Min/Max Spawn Margin `1.5 / 4`, Max Alive `45`.

### M6.2 SpawnManager
Systems > Add Component `SpawnManager` > **Config** = `SpawnConfig_Default`.

### M6.3 Pool tạo sẵn
Systems > PoolManager > Preload thêm: `Asteroid_Large 5`, `Asteroid_Medium 10`, `Asteroid_Small 15`, `Enemy_Diver 10`, `EnemyBullet 20`, `Explosion_Small 10`, `Explosion_Big 5`.

### M6.4 Dọn scene
Hierarchy chỉ còn: `CameraRig` (chứa Main Camera), `Global Light 2D`, `Systems`, `Arena`, `Background`, `Player`, `Crosshair`.

### M6 – Test
Mẹo test nhanh: tạm đặt **Survive Duration = 40** và Unlock Time Mine/Gunner/Kamikaze = `5, 10, 15`.
- [ ] Đối tượng xuất hiện từ ngoài mép màn hình, **ở mọi phía**, mật độ tăng dần.
- [ ] Bay vào góc bản đồ: kẻ địch chỉ đến từ các phía còn trống, không sinh ra ngoài viền.
- [ ] Hierarchy (trong PoolManager) không phình mãi: đối tượng bay xa bị thu hồi.
- [ ] Hết giờ: không sinh thêm (State = Victory). Chết: không sinh thêm (GameOver).
- [ ] **Trả số về như cũ** (180, unlock 30/60/90). Ctrl+S. Commit: `M6 sinh ngau nhien`.

---

## M7 – Giao diện

### M7.1 Canvas
1. **UI > Canvas** (tự tạo `EventSystem`).
2. Canvas Scaler: **Scale With Screen Size**, Reference `1920 x 1080`, Match `0.5`.
3. `EventSystem` có cảnh báo Input Module: bấm **Replace with InputSystemUIInputModule**.
4. Lần đầu tạo chữ TMP: **Import TMP Essentials**.

### M7.2 HUD
1. Trong Canvas: Create Empty `HUD`, anchor **stretch-stretch** (giữ Alt khi chọn preset).
2. Chữ (**UI > Text - TextMeshPro**) trong `HUD`:

| Tên | Anchor | Nội dung mẫu | Gợi ý |
|---|---|---|---|
| `ScoreText` | trên trái | `0` | Font 48 |
| `TimerText` | trên giữa | `03:00` | Font 60 |
| `KillsText` | trên phải | `0` | Font 48, căn phải |
| `AimModeText` | dưới trái | `NGẮM: CHUỘT [Q]` | Font 32 |

3. **Mạng:** Create Empty `Lives` (anchor trên trái, dưới ScoreText) + **Horizontal Layout Group**; bên trong 3 **UI > Image** = icon HP, Preserve Aspect, `56 x 56`.
4. **Thanh hồi chiêu lướt** (dưới trái, trên AimModeText):
   - **UI > Image** tên `DashBarBG`, kích thước `300 x 20`, màu xám tối.
   - Bên trong tạo **UI > Image** tên `DashBarFill`, anchor stretch-stretch, màu xanh lơ. **Source Image** chọn sprite bất kỳ (ví dụ *UISprite* hoặc Square), **Image Type = Filled**, **Fill Method = Horizontal**, **Fill Origin = Left**.
   - Thêm chữ nhỏ "LƯỚT [Shift]" cạnh thanh.
5. `HUD` > Add `HUDController`, gán:

| Ô | Kéo vào |
|---|---|
| Score / Timer / Kills / Aim Mode Text | các chữ tương ứng |
| Dash Cooldown Fill | `DashBarFill` |
| Life Icons | 3 Image trong `Lives` |
| Player Health / Player Aim / Player Movement | object `Player` (kéo 3 lần) |

### M7.3 Ba panel
Mỗi panel: **UI > Panel**, màu đen Alpha ~180, thêm **Vertical Layout Group** (Middle Center, Spacing 30).

| Panel | Bên trong |
|---|---|
| `PausePanel` | "TẠM DỪNG"; nút **Tiếp tục**, **Chơi lại**, **Về menu** |
| `GameOverPanel` | thông điệp thua; `GameOverScoreText`; `GameOverNewRecord` ("KỶ LỤC MỚI!", vàng); nút **Chơi lại**, **Về menu** |
| `VictoryPanel` | thông điệp thắng; `VictoryScoreText`; `VictoryNewRecord`; nút **Chơi lại**, **Về menu** |

Nút (**UI > Button - TextMeshPro**) khoảng `400 x 100`. **Bỏ tích** cả 3 panel.

### M7.4 GameUI
`Canvas` > Add `GameUI`, gán các panel, chữ điểm, nhãn kỷ lục. Gán **On Click ()** từng nút (kéo `Canvas` vào ô object):

| Nút | Hàm |
|---|---|
| Tiếp tục | `GameUI > OnResumeClicked` |
| Chơi lại | `GameUI > OnRestartClicked` |
| Về menu | `GameUI > OnMainMenuClicked` |

### M7.5 Menu chính
1. **File > New Scene** (Basic 2D URP hoặc tương đương), lưu `Assets/Scenes/MainMenu`.
2. Main Camera: Orthographic, Size 6, nền đen. Kéo prefab `Background` vào. Để nền trôi nhẹ: chọn từng lớp, đặt **Drift Speed** nhỏ (ví dụ lớp gần `(0.3, 0.1)`, lớp xa `(0.05, 0.02)`).
3. Canvas (cài như M7.1), Vertical Layout Group:
   - "VỆ BINH QUỸ ĐẠO" (Font ~96)
   - 2–3 dòng cốt truyện
   - Bảng phím: *WASD di chuyển · Chuột ngắm · Chuột trái bắn · Q tự ngắm · Shift lướt · Esc tạm dừng*
   - `HighScoreText`
   - Nút **Chơi**, **Credits**, **Thoát**
4. `CreditsPanel`: chữ ghi nguồn + nút **Đóng**, bỏ tích.
5. Canvas > Add `MainMenuController`, gán `HighScoreText`, `CreditsPanel`. On Click: Chơi → `OnPlayClicked`, Credits → `OnCreditsClicked`, Đóng → `OnCloseCreditsClicked`, Thoát → `OnQuitClicked`.

### M7.6 Danh sách scene
**File > Build Profiles** (bản cũ: *Build Settings*) > **Scene List**: `MainMenu` index **0**, `Game` index **1**.

### M7 – Test (Play từ MainMenu)
- [ ] Menu: kỷ lục, Credits, Chơi.
- [ ] HUD: điểm, đồng hồ, kills, mạng cập nhật đúng; bấm Q đổi chữ chế độ ngắm; lướt thì thanh tụt về 0 rồi đầy lại.
- [ ] Esc → PausePanel, có con trỏ chuột, Tiếp tục hoạt động.
- [ ] Thua / thắng (tạm giảm Survive Duration): panel đúng, điểm đúng, "KỶ LỤC MỚI!" khi phá kỷ lục. **Trả về 180.**
- [ ] Ctrl+S cả 2 scene. Commit: `M7 giao dien`.

---

## M8 – Hoàn thiện & build

### M8.1 Âm thanh
| Nơi gán | Ô | Âm thanh |
|---|---|---|
| Systems > AudioManager | Music Clip | nhạc nền |
| Player > PlayerShooting | Shoot Sfx | laser |
| Player > PlayerHealth | Hit Sfx / Death Sfx | trúng đòn / nổ to |
| Player > PlayerMovement | Dash Sfx | tiếng "vút" |
| Player > PlayerAim | Toggle Sfx | tiếng "bíp" |
| Prefab Asteroid_*, Mine, Enemy_* | Explode Sfx | nổ |
| Prefab Enemy_Gunner | Shoot Sfx | laser khác |
| Prefab PowerUp_* | Pickup Sfx | nhặt đồ |

### M8.2 Cân chỉnh
| Cảm giác | Chỉnh |
|---|---|
| Quá đông đầu game | Kéo đường cong SpawnConfig lên; giảm Weight Diver |
| Tự ngắm quá mạnh | Giảm Auto Aim Range (8) hoặc Auto Aim Fire Rate Multiplier (0.7) |
| Tàu "trơn" khó dừng | Tăng Deceleration (40) |
| Tàu ì | Tăng Acceleration (60), Max Speed (8) |
| Kamikaze không né được | Giảm Turn Speed (90) hoặc tăng Dash Delay |
| Gunner bắn quá dày | Fire Interval 1.8 |
| Bị đánh lén từ ngoài màn hình | Tăng Min Spawn Margin; tăng Look Ahead của camera |
| Camera chóng mặt | Tăng Smooth Time (0.25), giảm Look Ahead (1) |
| Ít vật phẩm | Tăng Drop Chance các EnemyData |

Chỉnh trên **prefab/asset** trong Project, không chỉnh lúc đang Play.

### M8.3 Build
1. **Edit > Project Settings > Player**: Product Name `Ve Binh Quy Dao`. **Resolution and Presentation**: Fullscreen Mode **Windowed**, `1280 x 720`, tích **Resizable Window** nếu muốn (game tự co giãn theo 16:9).
2. **File > Build Profiles** > **Windows** > **Build**, chọn thư mục `Build/` ngoài `Assets/`.
3. Chạy `.exe` thử, nén thư mục build thành `.zip`, thử trên máy khác.

### M8 – Checklist cuối
- [ ] Chơi đủ 3 phút không lỗi; thử cả ngắm tay và tự ngắm.
- [ ] Bản build chạy được, kỷ lục được lưu.
- [ ] Credits đầy đủ.
- [ ] Commit: `M8 hoan thien`, đặt tag `v1.0`.

---

## Phụ lục – Lỗi thường gặp

| Hiện tượng | Nguyên nhân thường gặp | Cách sửa |
|---|---|---|
| Tàu xoay lệch hướng chuột (ví dụ lệch 90°) | Sprite tàu không hướng lên | Đặt sprite vào object con và xoay object con cho mũi hướng lên |
| Tàu không xoay theo chuột / xoay lung tung | Main Camera mất tag `MainCamera`, hoặc Rig không ở z = -10 | Kiểm tra M1.3 |
| Đạn xuyên qua mục tiêu | Sai Layer, thiếu Rigidbody2D, chưa tích Is Trigger, ma trận va chạm | Kiểm tra lại và Phần 0.3 |
| Trigger không chạy dù đúng layer | Một số bản Unity cần bật tiếp xúc Kinematic | Ở Rigidbody2D bật *Use Full Kinematic Contacts* cho các prefab |
| Tự ngắm không khóa mục tiêu | Prefab thiếu `Targetable` | Thêm `Targetable` (M3, M4) |
| Tự ngắm bắn cả vật phẩm | Vật phẩm có `Targetable` | Gỡ `Targetable` khỏi prefab vật phẩm |
| Nền lộ mép hoặc có vạch | Ảnh nền chưa cài Full Rect + Repeat, hoặc ảnh không lặp liền mạch theo chiều ngang | 03_SETUP_PROJECT.md mục G6; bỏ lớp bị lỗi |
| Camera rung xong bị lệch | `CameraShake` gắn trên CameraRig | Chuyển `CameraShake` sang Main Camera (con) |
| Không thấy con trỏ ở menu tạm dừng | `Crosshair` chưa nghe StateChanged | Prompt "Sửa lỗi", mô tả hiện tượng |
| `NullReferenceException` lúc Play | Quên kéo tham chiếu | Đọc tên script + dòng lỗi, tìm ô ghi *None* |
| Lỗi "using the UnityEngine.Input class" | Code dùng Input cũ | Yêu cầu AI dùng `Keyboard.current` / `Mouse.current` |
| Nút UI bấm không ăn | EventSystem module cũ hoặc panel che | M7.1 bước 3; kiểm tra thứ tự Hierarchy |
| Chữ tiếng Việt hiện ô vuông | Font TMP mặc định thiếu ký tự | Tải font hỗ trợ tiếng Việt (ví dụ *Be Vietnam Pro*, Google Fonts), kéo `.ttf` vào Project, chuột phải > **Create > TextMeshPro > Font Asset**, đặt **Atlas Population Mode = Dynamic**, gán cho các chữ |
| Hierarchy ngày càng nhiều object | Đối tượng không Despawn | Kiểm tra Despawn Distance của Arena; Prompt "Sửa lỗi" |
| Chơi lại thì game đứng yên | `Time.timeScale` vẫn 0 | Kiểm tra `RestartGame` đặt `Time.timeScale = 1` |