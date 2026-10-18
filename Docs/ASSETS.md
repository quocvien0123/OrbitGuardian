# ASSETS.md – Danh sách asset

> Điền từ thư mục `Assets/Art/` và `Assets/Audio/` (đọc `.meta` để lấy danh sách sprite con). Bạn kiểm tra lại các chỗ ghi "(cần người dùng xác nhận)".
> Nguồn: *Free Shoot'em Up starter Asset Pack – Timberlate007 (itch.io)*. Được dùng và sửa đổi, không được bán lại.

**Pixels Per Unit đã dùng:** `40` (mọi file trong `Assets/Art/` đều đang là 40)  
**Kích thước tàu người chơi:** `41 x 43 px` (tàu xanh dương `SpaceShips_Player-0001_2`, khoảng 1,0 x 1,1 đơn vị thế giới)

**Quy ước đọc tên sprite con:** Unity đánh số theo thứ tự trong `.meta`, bắt đầu từ **hàng trên cùng** của ảnh, từ trái sang phải. Ví dụ `Bullets-0001_3` = sprite thứ 4 của file `Bullets-0001.png`.

**Lưu ý chung về Sprite Mode:** hầu hết file đang để Sprite Mode = **Multiple** (chỉ `Background_Full-0001` là Single). Các sprite con ghi bên dưới dùng được ngay. Riêng các lớp nền tiling (xem mục Backgrounds) và tâm ngắm nguyên khối cần đổi sang **Single**.

## Player
| Mục đích | Đường dẫn file | Sprite con / số frame | Ghi chú |
|---|---|---|---|
| Tàu người chơi (màu chọn dùng) | `Assets/Art/ShmupPack/SpaceShips_Player-0001.png` | `SpaceShips_Player-0001_2` (xanh dương, 41x43 px), 1 frame | Mũi tàu hướng lên (+Y). Cùng file còn: `_0` xám, `_1` xanh lá, `_3` cam, `_4` đen, `_5` đỏ (cùng cỡ 41x43). `_6`–`_11` là 6 tàu lớn hơn (60x46), xem mục Chưa dùng |

## Thruster (lửa động cơ)
| Mục đích | Đường dẫn file | Sprite con / số frame | Ghi chú |
|---|---|---|---|
| Lửa động cơ người chơi | `Assets/Art/ShmupPack/Exhaust-0001.png` | `Exhaust-0001_16`, `_17`, `_18`, `_19` (lửa xanh dương, 4 frame, khoảng 8x15 → 10x18 px) | Hợp màu tàu xanh dương. Đặt phía sau tàu. Hướng ngọn lửa cần kiểm tra khi làm prefab, có thể phải lật trục Y (cần người dùng xác nhận). Lửa đỏ để dùng thay thế: `_0`–`_3`, `_4`–`_7` (4 frame mỗi nhóm), `_8`–`_15` (8 frame, nhỏ) |

## Bullets
| Mục đích | Đường dẫn file | Sprite con | Ghi chú |
|---|---|---|---|
| Đạn người chơi | `Assets/Art/ShmupPack/Bullets-0001.png` | `Bullets-0001_3` (viên dài xanh dương, 10x18 px) | Viên dài dựng đứng nên đã hướng +Y, không cần xoay. Phương án khác cùng màu: `_4` (8x16), `_5` (6x14), giọt nước `_6` (9x21) |
| Đạn địch | `Assets/Art/ShmupPack/Bullets-0001.png` | `Bullets-0001_21` (quả cầu đỏ, 16x16 px) | Khác màu đạn người chơi. Hình tròn nên không cần xoay. Phương án khác: `_20` (18x18), `_22` (12x12), `_23` (viên dài đỏ 10x18) |

> Đạn phải hướng mũi lên (+Y). Nếu sprite nằm ngang thì ghi chú để xoay khi làm prefab.
>
> Bố cục file `Bullets-0001.png`: mỗi hàng một màu, theo thứ tự xanh dương (`_0`–`_9`), xanh lá (`_10`–`_19`), đỏ (`_20`–`_29`), vàng cam (`_30`–`_38`), hồng (`_39`–`_48`), xám (`_49`–`_58`). Mỗi hàng có 3 quả cầu (to → nhỏ), 3 viên dài, 3 giọt nước. Các sprite 33x32, 33x20, 13x18 (`_9`, `_19`, `_29`, `_48`, `_57`, `_59`–`_62`) là đạn của boss (không dùng).
> Giọt nước (`_6`–`_8`) có thể đang chĩa lên hoặc xuống, nếu dùng thì kiểm tra hướng (cần người dùng xác nhận).

## Enemies
Cả 12 sprite nằm trong `Assets/Art/ShmupPack/SpaceShips_Enemy-0001.png`, gồm 3 hàng biến thể màu (hàng 1: `_0`–`_3`, hàng 2: `_4`–`_7` có chi tiết cam, hàng 3: `_8`–`_11`). Mỗi hàng có 4 cỡ: lớn 49x55, trung 47x38, nhỏ 34x22, rất nhỏ 23x19. Gợi ý dùng **hàng 1**.

| Vai trò | Đường dẫn file | Sprite con | Ghi chú |
|---|---|---|---|
| Diver (lao thẳng) | `Assets/Art/ShmupPack/SpaceShips_Enemy-0001.png` | `SpaceShips_Enemy-0001_2` (34x22) | Thân nhỏ, dẹt, hợp kiểu lao thẳng (cần người dùng xác nhận) |
| Weaver (lượn sóng) | (không dùng) | | GDD không có Weaver (mục 8 chỉ có 3 loại, mục 14 ghi địch lượn sóng là hướng phát triển) |
| Gunner (bắn trả) | `Assets/Art/ShmupPack/SpaceShips_Enemy-0001.png` | `SpaceShips_Enemy-0001_1` (47x38) | Thân to hơn, có vẻ như có pháo hai bên, hợp địch 4 máu bắn trả (cần người dùng xác nhận) |
| Kamikaze (lao vào người chơi) | `Assets/Art/ShmupPack/SpaceShips_Enemy-0001.png` | `SpaceShips_Enemy-0001_3` (23x19) | Nhỏ nhất, hợp kẻ lao vào tàu (cần người dùng xác nhận) |

> Sprite địch có vẻ đang quay mũi **xuống dưới** (kiểu shmup cuộn dọc). Quy ước của project là +Y là phía trước, nên có thể phải xoay sprite 180° khi làm prefab (cần người dùng xác nhận).
> `SpaceShips_Enemy-0001_0` (49x55, to nhất) chưa gán vai trò, xem mục Chưa dùng.

## Asteroids
Cả 6 sprite nằm trong `Assets/Art/ShmupPack/Asteroids-0001.png`: hàng 1 `_0` (nâu nhỏ), `_1` (xám nhỏ); hàng 2 `_2` (nâu vừa), `_3` (xám vừa); hàng 3 `_4` (nâu lớn), `_5` (xám lớn). Gợi ý dùng bộ nâu (bộ xám là phương án thay thế).

| Cỡ | Đường dẫn file | Sprite con | Ghi chú |
|---|---|---|---|
| Large | `Assets/Art/ShmupPack/Asteroids-0001.png` | `Asteroids-0001_4` (41x40) | Bản xám: `_5` |
| Medium | `Assets/Art/ShmupPack/Asteroids-0001.png` | `Asteroids-0001_2` (28x28) | Bản xám: `_3` |
| Small | `Assets/Art/ShmupPack/Asteroids-0001.png` | `Asteroids-0001_0` (18x18) | Bản xám: `_1` |

## Mines
| Mục đích | Đường dẫn file | Sprite con / số frame | Ghi chú |
|---|---|---|---|
| Mìn (animation nhấp nháy) | `Assets/Art/ShmupPack/Mine-0001.png` | `Mine-0001_4`, `_5`, `_6`, `_7` (đỏ, 34x34 px, 4 frame) | File có 6 hàng x 4 frame: đỏ lớn `_4`–`_7`, cam lớn `_12`–`_15`, hồng lớn `_20`–`_23` (34x34); bản nhỏ 18x18: đỏ `_0`–`_3`, cam `_8`–`_11`, hồng `_16`–`_19`. Chọn bản đỏ lớn cho nổi bật (cần người dùng xác nhận) |

## Explosions
Cả 32 sprite nằm trong `Assets/Art/ShmupPack/Explosion-0001.png`: 4 hàng màu x 8 frame. Cam `_0`–`_7`, xanh lá `_8`–`_15`, hồng `_16`–`_23`, đỏ `_24`–`_31`.

| Mục đích | Đường dẫn file | Sprite con / số frame | Ghi chú |
|---|---|---|---|
| Nổ to (người chơi, thiên thạch lớn, mìn) | `Assets/Art/ShmupPack/Explosion-0001.png` | `Explosion-0001_24`–`_29` và `_31` (đỏ, 7 frame) | **Bỏ `_30`**: chỉ rộng 7x19 px, bị cắt thiếu (mảnh vỡ của vòng cuối), `_31` mới là vòng đầy đủ. Kích thước từ 12x12 tới 54x59 px |
| Nổ nhỏ (địch, thiên thạch nhỏ) | `Assets/Art/ShmupPack/Explosion-0001.png` | `Explosion-0001_0`–`_5` và `_7` (cam, 7 frame) | Bỏ `_6` vì lý do tương tự. Có thể thu nhỏ scale khi làm prefab. Chọn màu là gợi ý, đổi sang hàng khác cũng được (cần người dùng xác nhận) |

## PowerUps
Icon nằm trong `Assets/Art/ShmupPack/Bonuses-0001.png` (160x160, lưới 5 cột x 5 hàng, mỗi ô 24x24). Mỗi cột là một loại: cột 1 dấu cộng xanh lá, cột 2 khiên xanh dương, cột 3 sao nổ đỏ, cột 4 mũi tên cam, cột 5 "2x" hồng. Năm hàng là 5 biến thể / frame của cùng icon (cần người dùng xác nhận có phải frame animation không). Hàng 1 (trên cùng) là `_0`–`_4`.

| Loại | Đường dẫn file | Sprite con / số frame | Ghi chú |
|---|---|---|---|
| TripleShot | `Assets/Art/ShmupPack/Bonuses-0001.png` | `Bonuses-0001_3` (mũi tên cam tỏa ra, 24x24) | Hình mũi tên tỏa hợp "bắn 3 tia" (cần người dùng xác nhận). Các hàng sau cùng cột: `_8`, `_13`, `_18`, `_23` |
| RapidFire | (không dùng) | | GDD không có (hướng phát triển) |
| Shield | `Assets/Art/ShmupPack/Bonuses-0001.png` | `Bonuses-0001_1` (khiên xanh dương, 24x24) | Các hàng sau cùng cột: `_6`, `_11`, `_16`, `_21`. Hiệu ứng khiên bao quanh tàu có thể dùng `Assets/Art/ShmupPack/Barrier-0001.png`, sprite `Barrier-0001_1` (xanh dương, 70x70) (cần người dùng xác nhận) |
| ExtraLife | `Assets/Art/ShmupPack/Bonuses-0001.png` | `Bonuses-0001_0` (dấu cộng xanh lá, 24x24) | Hợp ý nghĩa hồi máu / thêm mạng. Các hàng sau cùng cột: `_5`, `_10`, `_15`, `_20` |
| Homing (đạn đuổi) | (không dùng) | | GDD không có (hướng phát triển) |

## Backgrounds (thứ tự từ xa đến gần)
Cài đặt: Mesh Type = Full Rect, Wrap Mode = Repeat (xem 03_SETUP_PROJECT.md G6).

Mọi ảnh nền đều 320x320 px. Các file này đang để Sprite Mode = **Multiple** (Wrap Mode hiện đang mặc định). Để tiling hãy đổi sang **Single** rồi áp dụng cài đặt trên. Lớp 1 là nền đặc (màu xanh đen), các lớp còn lại là ảnh trong suốt chồng lên.

| Lớp | Đường dẫn file | parallaxFactor gợi ý | Order in Layer |
|---|---|---|---|
| 1 (xa nhất) | `Assets/Art/ShmupPack/Background_Space-0001.png` (nền xanh đen đặc) | 0.95 | 0 |
| 2 | `Assets/Art/ShmupPack/Background_Nebula-0001.png` (tinh vân xanh đen, tối) | 0.85 | 1 |
| 3 | `Assets/Art/ShmupPack/Background_Nebula-0002.png` (tinh vân xanh sáng hơn) | 0.7 | 2 |
| 4 | `Assets/Art/ShmupPack/Background_SmallStars-0001.png` (sao li ti dày) | 0.5 | 3 |
| 5 (gần nhất) | `Assets/Art/ShmupPack/Background_Stars-0001.png` (sao to, thưa) | 0.3 | 4 |

> Thứ tự lớp 2–5 là gợi ý theo độ sáng và kích thước hạt, cần người dùng xác nhận khi xem trong Scene.
> `Background_Full-0001.png` (nền hoàn chỉnh, đã gồm tinh vân và sao) không đưa vào 5 lớp, xem mục Chưa dùng.

## Crosshair
| Mục đích | Đường dẫn file | Ghi chú |
|---|---|---|
| Tâm ngắm | `Assets/Art/Crosshair/crosshair-187.png` | Ảnh 74x74 px, hình hai ngoặc `( · )` màu xám nhạt. Đang cắt Multiple thành 3 sprite: `crosshair-187_0` (ngoặc trái 14x28), `_1` (chấm giữa 10x10), `_2` (ngoặc phải 14x28). Cần dùng nguyên khối thì đổi Sprite Mode sang Single. Ở PPU 40 ảnh nguyên khối rộng khoảng 1,85 đơn vị, có thể phải thu nhỏ (cần người dùng xác nhận). Ảnh màu xám nên đổi sang đỏ bằng `SpriteRenderer.color` / `Image.color` khi khóa mục tiêu |

## UI
Cả 23 sprite nằm trong `Assets/Art/ShmupPack/UI_sprites-0001.png` (128x128).

| Mục đích | Tên sprite | Kích thước | Ghi chú |
|---|---|---|---|
| Icon tim đầy (HP) | `UI_sprites-0001_15` | 15x13 | Trái tim đỏ viền kim cương |
| Icon tim rỗng (HP) | `UI_sprites-0001_16` | 15x13 | Trái tim xám (mất máu; KHÔNG phải ngọc xanh) |
| Icon đồng hồ | `UI_sprites-0001_17` | 16x16 | Hình tròn đồng hồ |
| Icon kills | `UI_sprites-0001_18` | 12x16 | Hình đầu lâu |
| Thanh nhãn SCORE | `UI_sprites-0001_0` | 75x23 | Thanh ngang có sẵn chữ "SCORE" |
| Thanh nhãn trống | `UI_sprites-0001_2` | 75x23 | Thanh ngang trống không chữ |
| Chữ số 0..9 | `_13` (0), `_4` (1), `_5` (2), `_6` (3), `_7` (4), `_8` (5), `_9` (6), `_10` (7), `_11` (8), `_12` (9) | 6x6 | 10 chữ số pixel, sắp xếp: 1..9 là `_4`..`_12`, số 0 là `_13`. **Lưu ý: sprite sheet không có ký tự ':'** |
| Thanh thời lượng dọc | Ruột: `UI_sprites-0001_20`<br>Khung: `UI_sprites-0001_21` | 8x24 | Ruột dải màu chuyển sắc xanh/vàng/đỏ; Khung xám ngoài |
| Icon vật phẩm tròn | `UI_sprites-0001_1` (+ xanh lá, ExtraLife)<br>`UI_sprites-0001_3` (khiên xanh dương, Shield)<br>`UI_sprites-0001_14` (sao đỏ)<br>`UI_sprites-0001_19` (cầu lửa cam, TripleShot)<br>`UI_sprites-0001_22` ("2x" hồng) | 24x24 | Các icon tròn hiển thị trạng thái vật phẩm đang dùng trên HUD |

## Audio
Toàn bộ âm thanh `.ogg` (bộ Kenney Sci-fi Sounds, CC0, theo tên file) nằm ở `Assets/Audio/`, mỗi nhóm có nhiều biến thể `_000`, `_001`, ... Có thể dùng nhiều biến thể trong một danh sách để tránh lặp.

| Mục đích | Đường dẫn file | Nguồn |
|---|---|---|
| Nhạc nền | `Assets/Audio/spaceranger3.wav` | Chưa rõ nguồn (cần người dùng xác nhận, ghi vào Credits). Là file `.wav` duy nhất, chắc là nhạc nền theo tên file, chưa nghe thử |
| Bắn (người chơi) | `Assets/Audio/laserSmall_000.ogg` (thêm `_001`–`_004`) | Kenney.nl (CC0) |
| Bắn (địch) | `Assets/Audio/laserRetro_000.ogg` (thêm `_001`–`_004`) | Kenney.nl (CC0) |
| Nổ | `Assets/Audio/explosionCrunch_000.ogg` (thêm `_001`–`_004`). Nổ to (người chơi, mìn): `Assets/Audio/lowFrequency_explosion_000.ogg` hoặc `_001` | Kenney.nl (CC0) |
| Trúng đòn | `Assets/Audio/impactMetal_000.ogg` (thêm `_001`–`_004`) | Kenney.nl (CC0) |
| Nhặt vật phẩm | `Assets/Audio/forceField_000.ogg` (thêm `_001`–`_004`) | Kenney.nl (CC0) |
| Lướt (Dash) | `Assets/Audio/thrusterFire_000.ogg` (thêm `_001`–`_004`) | Kenney.nl (CC0) |
| Bật/tắt tự ngắm | `Assets/Audio/computerNoise_000.ogg` (bật), `computerNoise_001.ogg` (tắt) | Kenney.nl (CC0) |

> Nguồn các file `.ogg` suy ra từ tên file (cần người dùng xác nhận). Chưa nghe thử, các lựa chọn trên chỉ dựa vào tên.

## Chưa dùng
| Đường dẫn file | Ghi chú |
|---|---|
| `Assets/Art/ShmupPack/SpaceShip_Boss-0001.png` | Toàn bộ sprite boss: `_0`–`_3` (12x21 lửa/bộ phận), `_4`–`_9` (108x108, thân boss). Để dành cho hướng phát triển |
| `Assets/Art/ShmupPack/SpaceShips_Enemy-0001.png` | Chưa dùng: `_0` (49x55, địch to nhất), các hàng biến thể màu `_4`–`_7` và `_8`–`_11` |
| `Assets/Art/ShmupPack/SpaceShips_Player-0001.png` | Chưa dùng: các màu khác `_0`, `_1`, `_3`, `_4`, `_5` (41x43) và 6 tàu lớn `_6`–`_11` (60x46) |
| `Assets/Art/ShmupPack/Bonuses-0001.png` | Icon vật phẩm không dùng: cột 3 sao nổ đỏ (`_2`, `_7`, `_12`, `_17`, `_22`), cột 4 mũi tên cam nếu gán RapidFire sau này, cột 5 "2x" hồng (`_4`, `_9`, `_14`, `_19`, `_24`), và các hàng 2–5 của ba cột đang dùng |
| `Assets/Art/ShmupPack/Barrier-0001.png` | 4 vòng khiên 70x70: `_0` xanh lá, `_1` xanh dương, `_2` hồng, `_3` đỏ. Chỉ dùng `_1` nếu chọn làm hiệu ứng khiên |
| `Assets/Art/ShmupPack/Background_Full-0001.png` | Nền hoàn chỉnh 320x320 (Single). Phương án thay thế cho 5 lớp parallax |
| `Assets/Art/ShmupPack/Bullets-0001.png` | Các màu đạn còn lại (xanh lá, vàng cam, hồng, xám) và toàn bộ đạn boss (xem mục Bullets) |
| `Assets/Art/ShmupPack/Exhaust-0001.png` | Các nhóm lửa đỏ `_0`–`_15` |
| `Assets/Art/ShmupPack/Mine-0001.png` | Mìn cam, hồng và các bản nhỏ 18x18 |
| `Assets/Art/ShmupPack/Explosion-0001.png` | Hàng nổ xanh lá (`_8`–`_15`) và hồng (`_16`–`_23`), các frame cắt lỗi `_6`, `_14`, `_22`, `_30` |
| `Assets/Art/ShmupPack/Asteroids-0001.png` | Bộ thiên thạch xám `_1`, `_3`, `_5` |
| `Assets/Audio/spaceEngine_000.ogg` – `_003.ogg`, `spaceEngineSmall_000.ogg` – `_004.ogg`, `spaceEngineLow_000.ogg` – `_004.ogg`, `spaceEngineLarge_000.ogg` – `_004.ogg`, `engineCircular_000.ogg` – `_004.ogg` | Tiếng động cơ. Có thể dùng làm tiếng động cơ tàu (ngoài đặc tả) |
| `Assets/Audio/laserLarge_000.ogg` – `_004.ogg`, `computerNoise_002.ogg`, `_003.ogg`, `doorOpen_000.ogg` – `_002.ogg`, `doorClose_000.ogg` – `_002.ogg`, `slime_000.ogg`, `_001.ogg` | Chưa gán. Có thể dùng làm âm thanh UI (cửa mở/đóng cho nút bấm, menu) |
| `Assets/Audio/desktop.ini` | File hệ thống Windows, không phải asset |
