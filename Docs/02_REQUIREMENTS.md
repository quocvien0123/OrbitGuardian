# 02 – Yêu cầu & Đặc tả kỹ thuật

> Đây là **nguồn sự thật duy nhất** cho code. AI phải làm đúng tên class, field, method ở mục 5.
> `05_UNITY_BUILD_GUIDE.md` dựa trên chính các tên này để hướng dẫn kéo thả.

---

## 1. Yêu cầu chức năng (FR)

Mỗi yêu cầu có tiêu chí chấp nhận (✔).

### Bản đồ & camera
- **FR-01** Bản đồ hình chữ nhật 64 × 36 có viền nhìn thấy được. ✔ Tàu không ra khỏi viền.
- **FR-02** Camera đi theo tàu, có làm mượt, nhìn trước về hướng ngắm, không đi quá viền bản đồ (cho lố 1 đơn vị). ✔ Không giật khi tàu đổi hướng.
- **FR-03** Nền nhiều lớp parallax theo chuyển động camera. ✔ Không thấy mép ảnh nền dù bay tới góc bản đồ.

### Người chơi
- **FR-04** WASD di chuyển 8 hướng có tăng tốc / giảm tốc. ✔ Đi chéo không nhanh hơn đi thẳng.
- **FR-05** Tàu xoay mũi theo con trỏ chuột. ✔ Xoay mượt, không giật.
- **FR-06** Giữ chuột trái hoặc Space để bắn theo hướng mũi tàu.
- **FR-07** Q bật/tắt tự ngắm: xoay về mục tiêu gần nhất trong tầm và tự bắn, tốc độ bắn × 0,8. ✔ Không có mục tiêu thì ngắm theo chuột và không tự bắn.
- **FR-08** Shift lướt theo hướng di chuyển (hoặc hướng mũi tàu nếu đứng yên), bất tử khi lướt, hồi chiêu 1,2 giây.
- **FR-09** Tâm ngắm thay con trỏ chuột khi đang chơi; khi tự ngắm có mục tiêu thì tâm ngắm đỏ và nằm trên mục tiêu. ✔ Con trỏ hệ thống hiện lại khi mở panel.
- **FR-10** 3 mạng, trúng đòn mất 1 mạng, bất tử 2 giây, nhấp nháy, camera rung. Hết mạng: nổ, 1,5 giây sau Game Over.

### Chướng ngại vật & kẻ địch
- **FR-11** Thiên thạch 3 cỡ, lớn vỡ thành 2 vừa, vừa vỡ thành 2 nhỏ khi bị bắn hạ.
- **FR-12** Mìn nổ lan bán kính, gây sát thương cho người chơi trong bán kính.
- **FR-13** 3 loại hành vi địch: Straight, Shooter (bay vòng, bắn vào người chơi), Kamikaze (bám đuổi có giới hạn góc xoay). ✔ Thông số trong `EnemyData`.
- **FR-14** Đâm vào người chơi: người chơi mất 1 mạng, đối tượng nổ, không cho điểm.
- **FR-15** Đối tượng đi quá xa (ra ngoài bản đồ hoặc quá xa camera) tự trả về pool.

### Điểm & vật phẩm
- **FR-16** Bắn hạ: cộng điểm, tăng Kills, có cơ hội rơi vật phẩm.
- **FR-17** 3 vật phẩm: TripleShot, Shield, ExtraLife. Vật phẩm tồn tại 10 giây, nhấp nháy 3 giây cuối.
- **FR-18** Thắng: +1000 + 500 × mạng còn lại. Kỷ lục lưu `PlayerPrefs` khóa `"HighScore"`.

### Sinh đối tượng
- **FR-19** Đếm ngược 180 giây, hết giờ thì Victory.
- **FR-20** Sinh ngẫu nhiên có trọng số, mở khóa theo thời gian, khoảng sinh theo `AnimationCurve`, vị trí ngay ngoài tầm nhìn camera và trong bản đồ, tối đa `maxAlive` đối tượng.

### Giao diện & âm thanh
- **FR-21** HUD: điểm, đồng hồ mm:ss, Kills, icon mạng, chế độ ngắm, thanh hồi chiêu lướt.
- **FR-22** Esc tạm dừng (`Time.timeScale = 0`). Panel Tạm dừng, Game Over, Victory, menu chính, Credits.
- **FR-23** Âm thanh bắn, nổ, trúng đòn, nhặt vật phẩm, lướt, bật/tắt tự ngắm, nhạc nền. ✔ Để trống clip không gây lỗi.

## 2. Yêu cầu phi chức năng (NFR)

- **NFR-01** ≥ 60 FPS trên laptop phổ thông.
- **NFR-02** Đối tượng sinh nhiều lần **bắt buộc** dùng Object Pooling.
- **NFR-03** Không có lỗi đỏ trong Console suốt một lượt chơi.
- **NFR-04** Thông số cân bằng chỉnh trong Inspector hoặc ScriptableObject.
- **NFR-05** Tìm mục tiêu không dùng `FindObjectsOfType`; dùng danh sách `Targetable.All`.
- **NFR-06** Code tuân thủ `AGENTS.md`.

---

## 3. Quy ước dự án

### 3.1 Thư mục
```
Assets/
├── Art/ShmupPack/        ← asset tải về (không sửa)
├── Audio/
├── Data/                 ← EnemyData, SpawnConfig
├── Prefabs/              ← Player/, Weapons/, Hazards/, Enemies/, PowerUps/, Effects/
├── Scenes/               ← MainMenu.unity, Game.unity
└── Scripts/
    ├── Core/      ← M1
    ├── Player/    ← M2, M5
    ├── Weapons/   ← M2
    ├── Hazards/   ← M3
    ├── Enemies/   ← M4
    ├── PowerUps/  ← M5
    ├── Spawning/  ← M6
    └── UI/        ← M7
```

### 3.2 Layer
| Số | Tên | Dùng cho |
|---|---|---|
| 6 | `Player` | Tàu người chơi |
| 7 | `PlayerBullet` | Đạn người chơi |
| 8 | `Enemy` | Kẻ địch |
| 9 | `EnemyBullet` | Đạn địch |
| 10 | `Hazard` | Thiên thạch, mìn |
| 11 | `PowerUp` | Vật phẩm |

### 3.3 Ma trận va chạm (chỉ bật các cặp sau)
| | Player | PlayerBullet | Enemy | EnemyBullet | Hazard | PowerUp |
|---|---|---|---|---|---|---|
| **Player** | | | ✔ | ✔ | ✔ | ✔ |
| **PlayerBullet** | | | ✔ | | ✔ | |
| **Enemy** | ✔ | ✔ | | | | |
| **EnemyBullet** | ✔ | | | | | |
| **Hazard** | ✔ | ✔ | | | | |
| **PowerUp** | ✔ | | | | | |

### 3.4 Tag: tàu người chơi dùng tag `Player`. Camera dùng tag `MainCamera`.

### 3.5 Sorting Layer (từ dưới lên): `Background` → `Default` → `Gameplay` → `Player` → `Effects`

### 3.6 Vật lý & hướng
- Mọi đối tượng di chuyển có `Rigidbody2D` **Kinematic**, collider **Is Trigger**, di chuyển bằng `transform` trong `Update()`.
- **`transform.up` là phía trước.** Kẻ địch có sprite hướng xuống thì bật **Flip Y** trên SpriteRenderer để sprite hướng lên.
- Collider của đối tượng có xoay dùng **Circle Collider 2D** (không dùng Polygon vì Flip Y không lật Polygon).

### 3.7 Trách nhiệm xử lý va chạm
| Script | Xử lý trong `OnTriggerEnter2D` |
|---|---|
| `Bullet` | Gây sát thương cho `IDamageable`, tự Despawn |
| `Enemy`, `Asteroid`, `Mine` | Chỉ khi chạm `PlayerHealth`: gây 1 sát thương cho người chơi, tự nổ (không cho điểm) |
| `PowerUp` | Chỉ khi chạm `PlayerPowerUps`: áp dụng, tự Despawn |
| `PlayerHealth` | **Không** xử lý va chạm |

### 3.8 Hợp đồng Object Pool
- Tạo: `PoolManager.Instance.Spawn(prefab, position, rotation)`, vị trí đặt **trước** khi object active.
- Trả: `PoolManager.Instance.Despawn(gameObject)`. Object không từ pool (đặt tay để test) thì `Destroy`.
- Khởi tạo lại trạng thái trong `OnEnable()`.

### 3.9 Tham chiếu người chơi
Script cần vị trí người chơi dùng `PlayerHealth.Instance`. Phải kiểm tra `Instance != null && Instance.gameObject.activeInHierarchy`. Không có người chơi thì dùng tâm camera.

---

## 4. Sơ đồ kiến trúc

```
   Input (Keyboard/Mouse)
          │
  ┌───────▼────────┐  ┌──────────────┐  ┌──────────────┐
  │ PlayerMovement │  │ PlayerAim    │──│ Targetable   │◄── Enemy, Asteroid, Mine
  │ (quán tính,    │  │ (chuột / Q   │  │ .All (danh   │    tự đăng ký khi bật
  │  Dash)         │  │  tự ngắm)    │  │  sách mục    │
  └───────┬────────┘  └──────┬───────┘  │  tiêu)       │
          │                  │          └──────────────┘
          │           ┌──────▼───────┐
  ┌───────▼──────┐    │PlayerShooting│──► Bullet ──► IDamageable.TakeDamage
  │ PlayerHealth │    └──────────────┘
  │ (Instance)   │──► event LivesChanged ──► HUD
  └──────────────┘
  ┌────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
  │ Arena      │◄─│ SpawnManager │◄─│ SpawnConfig  │  │ GameManager  │──► events
  │ (biên, điểm│  └──────┬───────┘  └──────────────┘  │ (state, giờ, │    ScoreChanged...
  │  sinh, xóa)│         │ Spawn                      │  điểm)       │
  └────────────┘  ┌──────▼───────┐                    └──────────────┘
  CameraRig ──►   │ PoolManager  │◄── mọi Spawn / Despawn
  CameraFollow    └──────────────┘
   └ Main Camera (CameraShake)
```

---

## 5. Đặc tả script

Ký hiệu: `[SF]` = `[SerializeField] private`. Số trong ngoặc là mặc định.

### M1 – Nền tảng (`Scripts/Core/`)

**`IDamageable`** (interface): `void TakeDamage(int amount);`

**`Direction2D`** (static class)
- `static Quaternion ToRotation(Vector2 direction)`: rotation để `transform.up` trùng `direction` (góc = `Atan2(y, x) * Rad2Deg - 90`).
- `static Vector2 FromAngle(float degrees)`: vector đơn vị theo góc (0° = hướng +Y).
- `static Vector2 Rotate(Vector2 v, float degrees)`.

**`Arena`** (singleton, gắn trên object `Arena`)
- `[SF] Vector2 size (64, 36)`, `[SF] float despawnMargin (3)`, `[SF] float despawnDistance (30)`, `[SF] LineRenderer border` (tùy chọn).
- `Rect Bounds`: hình chữ nhật tâm tại `transform.position`, kích thước `size`.
- `Start()`: nếu có `border` thì đặt 4 điểm góc + `loop = true`, `useWorldSpace = true`.
- `Vector3 ClampInside(Vector3 pos, float padding)`.
- `bool ShouldDespawn(Vector3 pos)`: true nếu ra ngoài `Bounds` mở rộng `despawnMargin`, **hoặc** cách tâm camera quá `despawnDistance`.
- `static Rect GetCameraView(float margin = 0)`: vùng camera chính đang nhìn thấy (theo thế giới), mở rộng `margin`.
- `Vector3 GetSpawnPointOutsideView(float minMargin, float maxMargin)`: thử tối đa 15 lần một điểm ngẫu nhiên nằm trong vùng camera mở rộng `maxMargin` nhưng **ngoài** vùng camera mở rộng `minMargin`, đồng thời **trong** `Bounds`. Thất bại thì trả về một điểm ngẫu nhiên trong `Bounds` cách tâm camera ít nhất 12 đơn vị.
- `OnDrawGizmos`: vẽ khung `Bounds`.

**`CameraFollow`** (gắn trên `CameraRig`, cha của Main Camera)
- `[SF] Transform target`, `[SF] PlayerAim playerAim` (tùy chọn), `[SF] float smoothTime (0.15)`, `[SF] float lookAhead (1.5)`, `[SF] float edgeOverscroll (1)`.
- `LateUpdate()`: vị trí mong muốn = target + hướng ngắm × `lookAhead`; `Vector3.SmoothDamp`; kẹp sao cho vùng camera nằm trong `Arena.Bounds` mở rộng `edgeOverscroll` (nếu bản đồ nhỏ hơn camera theo trục nào thì căn giữa trục đó). Giữ nguyên z. Target bị tắt thì đứng yên.

**`CameraShake`** (gắn trên Main Camera, singleton)
- `void Shake(float duration = 0.2f, float magnitude = 0.15f)`: chỉ thay đổi **`localPosition`** quanh `(0,0,0)`, xong thì trả về `(0,0,0)`.

**`ParallaxLayer`** (`[RequireComponent(typeof(SpriteRenderer))]`)
- `[SF] float parallaxFactor (0.5)` (0 = gắn chặt với bản đồ, gần 1 = gần như đứng yên theo camera, tức là rất xa), `[SF] Vector2 driftSpeed (0, 0)`.
- `Start()`: `drawMode = Tiled`, `size` = kích thước Arena + 2 × kích thước vùng camera (tính theo đơn vị cục bộ, chia cho scale). **Không có `Arena` trong scene** (ví dụ menu chính) thì dùng 3 × kích thước vùng camera. Ghi nhớ vị trí ban đầu.
- `LateUpdate()`: vị trí = vị trí ban đầu + vị trí camera × `parallaxFactor` + độ trôi tích lũy. Độ trôi lấy **phần dư theo kích thước một ô sprite** để không bao giờ lộ mép.

**`PoolManager`** (singleton)
- `[SF] List<PoolPreload> preload`; `[System.Serializable] public class PoolPreload { public GameObject prefab; public int count = 10; }` (cùng file).
- `GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)`; `void Despawn(GameObject obj)` (chống trả 2 lần; object không có `PooledObject` thì `Destroy`).
- Object được gom vào GameObject chứa theo tên prefab, dưới PoolManager. `Start()` tạo sẵn theo `preload`.

**`PooledObject`** (PoolManager tự thêm): `public GameObject SourcePrefab; public bool InPool;`

**`SimpleSpriteAnimator`** (`[RequireComponent(typeof(SpriteRenderer))]`)
- `[SF] Sprite[] frames`, `[SF] float framesPerSecond (12)`, `[SF] bool loop (true)`, `[SF] bool randomStartFrame (false)`, `[SF] bool despawnOnComplete (false)`.
- `OnEnable()` về frame đầu (hoặc ngẫu nhiên). Không loop và hết frame thì Despawn nếu `despawnOnComplete`.

**`HitFlash`**: `[SF] Color flashColor (1, 0.4, 0.4)`, `[SF] float duration (0.08)`. Dùng `GetComponentInChildren<SpriteRenderer>()`. `void Flash()`. `OnEnable()` trả màu gốc.

**`Targetable`** (gắn lên Enemy, Asteroid, Mine)
- `public static IReadOnlyList<Targetable> All`. Thêm vào danh sách trong `OnEnable()`, xóa trong `OnDisable()`.
- `public static Targetable FindNearest(Vector3 from, float range)`: duyệt `All`, so sánh `sqrMagnitude`, trả về gần nhất trong `range` hoặc `null`.

**`GameState`** (enum): `Playing, Paused, GameOver, Victory`

**`GameManager`** (singleton)
- `[SF] float surviveDuration (180)`, `[SF] float gameOverDelay (1.5)`, `[SF] int victoryBonus (1000)`, `[SF] int bonusPerLife (500)`, `[SF] string mainMenuSceneName ("MainMenu")`.
- Chỉ đọc: `GameState State`, `int Score`, `int Kills`, `float ElapsedTime`, `float RemainingTime`, `int HighScore`, `bool IsNewHighScore`.
- Event: `event Action<int> ScoreChanged`, `event Action<int> KillsChanged`, `event Action<float> TimeChanged` (RemainingTime), `event Action<GameState> StateChanged`.
- `Start()`: `Time.timeScale = 1`, đọc HighScore, State = Playing.
- `Update()`: Playing thì tăng thời gian, phát TimeChanged, hết giờ thì `WinGame()`. Esc gọi `TogglePause()`.
- `void AddScore(int amount)` (chỉ khi Playing), `void AddKill()`, `void OnPlayerDied()` (sau `gameOverDelay` → GameOver, lưu kỷ lục), `void TogglePause()`, `void RestartGame()`, `void LoadMainMenu()` (đặt `Time.timeScale = 1` trước khi load).
- `WinGame()` (private): thưởng theo `PlayerHealth.Instance.CurrentLives`, State = Victory, lưu kỷ lục.

**`LootManager`** (singleton): `[SF] GameObject[] powerUpPrefabs`; `void TryDrop(Vector3 position, float chance)`.

**`AudioManager`** (singleton): `[SF] AudioSource musicSource`, `[SF] AudioSource sfxSource`, `[SF] AudioClip musicClip`; `static void PlaySfx(AudioClip clip, float volume = 1f)` an toàn khi null.

### M2 – Người chơi & vũ khí

**`PlayerMovement`** (`Scripts/Player/`)
- `[SF] float maxSpeed (7)`, `[SF] float acceleration (45)`, `[SF] float deceleration (30)`, `[SF] float arenaPadding (0.6)`, `[SF] float dashSpeed (20)`, `[SF] float dashDuration (0.15)`, `[SF] float dashCooldown (1.2)`, `[SF] GameObject dashEffectPrefab` (tùy chọn), `[SF] AudioClip dashSfx`.
- Chỉ đọc: `Vector2 Velocity`, `bool IsDashing`, `float DashCooldownNormalized` (1 = vừa dùng, 0 = sẵn sàng).
- Đọc WASD, chuẩn hóa. Có input: vận tốc tiến tới `input × maxSpeed` theo `acceleration`; không có: giảm về 0 theo `deceleration` (`Vector2.MoveTowards`). **Không xoay tàu** (việc của PlayerAim).
- Shift (`wasPressedThisFrame`) khi hết hồi chiêu: lướt theo hướng input (không có input thì `transform.up`) với `dashSpeed` trong `dashDuration`, Spawn `dashEffectPrefab` tại chỗ nếu có.
- Kẹp vị trí bằng `Arena.Instance.ClampInside`. Chỉ hoạt động khi State = Playing.

**`PlayerAim`** (`Scripts/Player/`)
- `[SF] float rotationSpeed (900)` (độ/giây), `[SF] float autoAimRange (10)`, `[SF] float retargetInterval (0.1)`, `[SF] bool startWithAutoAim (false)`, `[SF] AudioClip toggleSfx`.
- Chỉ đọc: `bool IsAutoAim`, `Targetable CurrentTarget`, `Vector2 AimDirection`, `Vector3 MouseWorldPosition`.
- `event Action<bool> AimModeChanged`.
- Q (`wasPressedThisFrame`) đổi chế độ, phát event và âm thanh.
- Mỗi `retargetInterval` giây (chỉ khi tự ngắm): `CurrentTarget = Targetable.FindNearest(...)`. Mục tiêu bị tắt thì bỏ.
- Điểm ngắm = vị trí mục tiêu (tự ngắm có mục tiêu) hoặc vị trí chuột trong thế giới (`Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())`, z = 0).
- Xoay bằng `Quaternion.RotateTowards` tới `Direction2D.ToRotation(điểm ngắm − vị trí tàu)`.
- `Start()`: phát `AimModeChanged` lần đầu. Chỉ hoạt động khi State = Playing.

**`PlayerShooting`** (`Scripts/Player/`)
- `[SF] GameObject bulletPrefab`, `[SF] Transform firePoint`, `[SF] float fireRate (0.15)` (giây giữa 2 phát), `[SF] float tripleShotAngle (15)`, `[SF] float autoAimFireRateMultiplier (0.8)`, `[SF] AudioClip shootSfx`.
- Bắn khi State = Playing và (giữ chuột trái **hoặc** giữ Space **hoặc** (`PlayerAim.IsAutoAim` và có `CurrentTarget`)).
- Khoảng thời gian bắn = `fireRate`; đang tự ngắm thì chia cho `autoAimFireRateMultiplier`.
- Spawn đạn tại `firePoint` với rotation của `firePoint`; Triple Shot thêm 2 viên xoay ±`tripleShotAngle`.
- `void SetTripleShot(bool active)`.

**`PlayerHealth`** (`Scripts/Player/`, `IDamageable`, singleton `Instance`)
- `[SF] int maxLives (3)`, `[SF] float invincibleDuration (2)`, `[SF] float blinkInterval (0.1)`, `[SF] GameObject explosionPrefab`, `[SF] GameObject shieldVisual`, `[SF] AudioClip hitSfx`, `[SF] AudioClip deathSfx`.
- `int CurrentLives`, `int MaxLives`, `bool HasShield`, `event Action<int> LivesChanged`.
- `TakeDamage`: bỏ qua nếu State ≠ Playing, đang bất tử, đang lướt (`PlayerMovement.IsDashing`) hoặc đã chết. Có khiên: mất khiên, bất tử 0,5 giây. Không: trừ mạng, `CameraShake.Instance?.Shake()`, nhấp nháy. Hết mạng: nổ, `SetActive(false)`, `GameManager.Instance.OnPlayerDied()`.
- `void AddLife()` (đủ mạng thì +200 điểm), `void ActivateShield()`. `Start()` phát `LivesChanged`.

**`Crosshair`** (`Scripts/Player/`, gắn trên object `Crosshair` có SpriteRenderer)
- `[SF] PlayerAim playerAim`, `[SF] Color manualColor (trắng)`, `[SF] Color lockedColor (đỏ)`.
- `LateUpdate()`: tự ngắm có mục tiêu thì đặt tại mục tiêu + `lockedColor`, ngược lại đặt tại chuột + `manualColor`.
- `Cursor.visible = false` khi State = Playing, `true` ở các trạng thái khác (lắng nghe `StateChanged`). `OnDestroy()` đặt lại `Cursor.visible = true`.

**`Bullet`** (`Scripts/Weapons/`)
- `[SF] float speed (14)`, `[SF] int damage (1)`, `[SF] float lifetime (1.5)`.
- `OnEnable()`: reset thời gian sống và cờ đã trúng.
- Bay theo `transform.up`.
- Trúng `IDamageable`: gây sát thương, Despawn (chỉ trúng 1 lần). Hết `lifetime` hoặc `Arena.Instance.ShouldDespawn`: Despawn.

### M3 – Chướng ngại vật (`Scripts/Hazards/`)

**`Asteroid`** (`IDamageable`)
- `[SF] int maxHealth (3)`, `[SF] int scoreValue (30)`, `[SF] float minSpeed (1)`, `[SF] float maxSpeed (2.5)`, `[SF] float aimOffsetRadius (5)`, `[SF] float maxRotationSpeed (90)`, `[SF] GameObject splitPrefab`, `[SF] int splitCount (2)`, `[SF] float splitSpeed (2.5)`, `[SF] float dropChance (0.05)`, `[SF] GameObject explosionPrefab`, `[SF] AudioClip explodeSfx`.
- `OnEnable()`: hồi máu; vận tốc hướng về (vị trí người chơi + điểm ngẫu nhiên trong bán kính `aimOffsetRadius`), độ lớn ngẫu nhiên; tốc độ tự xoay ngẫu nhiên.
- `void Launch(Vector2 velocity)`: ghi đè vận tốc (mảnh vỡ).
- `TakeDamage` → `HitFlash` → hết máu `Die(true)`. `Die(bool killedByPlayer)`: nổ, âm thanh; nếu do người chơi: điểm, AddKill, TryDrop, sinh `splitCount` mảnh, mỗi mảnh vận tốc = vận tốc hiện tại xoay đều các góc tỏa ra (ví dụ ±45°) với độ lớn `splitSpeed`. Despawn.
- Chạm người chơi: gây 1 sát thương, `Die(false)`. `ShouldDespawn`: Despawn.

**`Mine`** (`IDamageable`)
- `[SF] int maxHealth (2)`, `[SF] int scoreValue (25)`, `[SF] float speed (0.6)`, `[SF] float explosionRadius (1.5)`, `[SF] LayerMask playerLayer`, `[SF] float dropChance (0.05)`, `[SF] GameObject explosionPrefab`, `[SF] AudioClip explodeSfx`.
- Trôi chậm về hướng chung của người chơi (lệch ngẫu nhiên ±30°). Nổ: `Physics2D.OverlapCircle` theo `playerLayer`, trúng `PlayerHealth` thì gây 1 sát thương. Bị bắn hạ thì cộng điểm, AddKill, TryDrop. `OnDrawGizmosSelected` vẽ bán kính.

### M4 – Kẻ địch (`Scripts/Enemies/`)

**`MovementPattern`** (enum): `Straight, Shooter, Kamikaze`

**`EnemyData`** (ScriptableObject, `[CreateAssetMenu(menuName = "SpaceShooter/Enemy Data")]`, field `public`)
- Chung: `int maxHealth (1)`, `float moveSpeed (4)`, `int scoreValue (10)`, `float dropChance (0.05)`, `MovementPattern pattern`, `float aimSpread (10)` (độ lệch ngẫu nhiên khi chọn hướng ban đầu).
- Shooter: `float preferredDistance (6)`, `float orbitSpeed (40)` (độ/giây), `float stayDuration (10)`, `float fireInterval (1.4)`, `GameObject bulletPrefab`.
- Kamikaze: `float dashDelay (0.8)`, `float dashSpeed (7)`, `float turnSpeed (120)` (độ/giây).

**`Enemy`** (`IDamageable`)
- `[SF] EnemyData data`, `[SF] GameObject explosionPrefab`, `[SF] AudioClip explodeSfx`, `[SF] AudioClip shootSfx`.
- `OnEnable()`: hồi máu, reset hẹn giờ, tính **hướng ban đầu** = hướng tới người chơi xoay ngẫu nhiên ±`aimSpread`, xoay mặt theo hướng đó.
- Ba hàm riêng:
  - `MoveStraight()`: đi thẳng theo hướng ban đầu với `moveSpeed`.
  - `MoveShooter()`: còn xa hơn `preferredDistance` thì tiến tới người chơi; tới nơi thì bay vòng quanh người chơi (`orbitSpeed`, chiều ngẫu nhiên khi spawn) giữ khoảng cách; luôn xoay mặt về người chơi; bắn theo `fireInterval` (Spawn `bulletPrefab` ở phía trước, rotation hướng về người chơi). Sau `stayDuration` thì bay ra xa người chơi.
  - `MoveKamikaze()`: trong `dashDelay` giây trôi chậm về phía người chơi với `moveSpeed`; sau đó xoay `transform.up` về người chơi tối đa `turnSpeed` độ/giây và tiến theo `transform.up` với `dashSpeed`.
- `TakeDamage`, `Die(bool killedByPlayer)`, va chạm người chơi, `ShouldDespawn`: như `Asteroid` (không vỡ đôi).

### M5 – Vật phẩm

**`PowerUpType`** (`Scripts/PowerUps/`): `TripleShot, Shield, ExtraLife`

**`PowerUp`** (`Scripts/PowerUps/`)
- `[SF] PowerUpType type`, `[SF] float lifetime (10)`, `[SF] float blinkTime (3)`, `[SF] float bobAmplitude (0.15)`, `[SF] float bobFrequency (3)`, `[SF] AudioClip pickupSfx`.
- Lơ lửng nhấp nhô quanh vị trí sinh. `blinkTime` giây cuối nhấp nháy, hết `lifetime` thì Despawn. Chạm `PlayerPowerUps`: `Apply(type)`, âm thanh, Despawn.

**`PlayerPowerUps`** (`Scripts/Player/`)
- `[SF] float effectDuration (8)`.
- `void Apply(PowerUpType type)`: TripleShot → `PlayerShooting.SetTripleShot` có hẹn giờ; Shield → `PlayerHealth.ActivateShield()`; ExtraLife → `PlayerHealth.AddLife()`. Nhặt lại TripleShot khi đang có thì làm mới thời gian.

### M6 – Sinh đối tượng (`Scripts/Spawning/`)

**`SpawnEntry`** (`[System.Serializable]`): `public string label`, `public GameObject prefab`, `public float weight (1)`, `public float unlockTime (0)`.

**`SpawnConfig`** (ScriptableObject, `[CreateAssetMenu(menuName = "SpaceShooter/Spawn Config")]`)
- `public List<SpawnEntry> entries`
- `public AnimationCurve spawnIntervalOverTime` mặc định (0, 1.4), (0.5, 0.8), (0.85, 0.45), (1, 0.35); x = ElapsedTime / surviveDuration.
- `public float minSpawnMargin (1.5)`, `public float maxSpawnMargin (4)`, `public int maxAlive (45)`.

**`SpawnManager`**
- `[SF] SpawnConfig config`, `[SF] float startDelay (1.5)`.
- Coroutine: chờ `startDelay`, lặp khi Playing hoặc Paused: chờ theo curve; nếu Playing và `Targetable.All.Count < maxAlive` thì `PickWeightedEntry()` trong các entry đã mở khóa, Spawn tại `Arena.Instance.GetSpawnPointOutsideView(minSpawnMargin, maxSpawnMargin)`. Dừng khi GameOver/Victory.

### M7 – Giao diện (`Scripts/UI/`)

**`HUDController`**
- `[SF] TMP_Text scoreText`, `[SF] TMP_Text timerText`, `[SF] TMP_Text killsText`, `[SF] TMP_Text aimModeText`, `[SF] Image dashCooldownFill`, `[SF] Image[] lifeIcons`, `[SF] PlayerHealth playerHealth`, `[SF] PlayerAim playerAim`, `[SF] PlayerMovement playerMovement`.
- Đăng ký event trong `Start()`, hủy trong `OnDestroy()`. `aimModeText`: "NGẮM: CHUỘT [Q]" hoặc "NGẮM: TỰ ĐỘNG [Q]". `Update()`: `dashCooldownFill.fillAmount = 1 - DashCooldownNormalized`.

**`GameUI`**
- `[SF] GameObject pausePanel`, `[SF] GameObject gameOverPanel`, `[SF] GameObject victoryPanel`, `[SF] TMP_Text gameOverScoreText`, `[SF] TMP_Text victoryScoreText`, `[SF] GameObject gameOverNewRecordLabel`, `[SF] GameObject victoryNewRecordLabel`.
- Lắng nghe `StateChanged`. Nút: `OnResumeClicked()`, `OnRestartClicked()`, `OnMainMenuClicked()`.

**`MainMenuController`**
- `[SF] TMP_Text highScoreText`, `[SF] GameObject creditsPanel`, `[SF] string gameSceneName ("Game")`.
- Nút: `OnPlayClicked()`, `OnCreditsClicked()`, `OnCloseCreditsClicked()`, `OnQuitClicked()`.

---

## 6. Bảng thông số mặc định

### EnemyData
| Asset | pattern | maxHealth | moveSpeed | scoreValue | dropChance | Khác |
|---|---|---|---|---|---|---|
| `Enemy_Diver` | Straight | 1 | 6 | 10 | 0.04 | aimSpread 10 |
| `Enemy_Gunner` | Shooter | 4 | 3 | 40 | 0.15 | preferredDistance 6, orbitSpeed 40, stayDuration 10, fireInterval 1.4, bulletPrefab = EnemyBullet |
| `Enemy_Kamikaze` | Kamikaze | 1 | 2 | 30 | 0.06 | dashDelay 0.8, dashSpeed 7, turnSpeed 120 |

### Chướng ngại vật
| Prefab | maxHealth | scoreValue | splitPrefab |
|---|---|---|---|
| `Asteroid_Large` | 3 | 30 | Asteroid_Medium |
| `Asteroid_Medium` | 2 | 20 | Asteroid_Small |
| `Asteroid_Small` | 1 | 10 | (trống) |
| `Mine` | 2 | 25 | — |

### SpawnConfig_Default
| label | prefab | weight | unlockTime |
|---|---|---|---|
| Asteroid L | Asteroid_Large | 3 | 0 |
| Asteroid M | Asteroid_Medium | 2 | 0 |
| Diver | Enemy_Diver | 4 | 0 |
| Mine | Mine | 1 | 30 |
| Gunner | Enemy_Gunner | 1.5 | 60 |
| Kamikaze | Enemy_Kamikaze | 1.5 | 90 |

---

## 7. Định nghĩa "Hoàn thành" cho mỗi milestone

- [ ] Không có lỗi biên dịch, không có lỗi đỏ khi chạy.
- [ ] Đạt các tiêu chí ✔ liên quan ở mục 1.
- [ ] Tên class, field, method khớp mục 5.
- [ ] Đã lắp ráp theo `05_UNITY_BUILD_GUIDE.md` và đã Git commit.