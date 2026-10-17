# 03 – Cài đặt & tạo project

Làm theo thứ tự. Mỗi bước có ô ✅ để đánh dấu.

## Phần A – Cài công cụ

- [ ] **Unity Hub** từ unity.com/download, đăng nhập Unity ID (gói Personal miễn phí).
- [ ] Trong Unity Hub > **Installs** > **Install Editor**: chọn bản **Unity 6 LTS** mới nhất. Ở mục Modules tích **Windows Build Support (Mono)** (thường có sẵn) và **bỏ tích** Visual Studio nếu bạn dùng VS Code.
- [ ] **Visual Studio Code** từ code.visualstudio.com.
- [ ] Trong VS Code, tab Extensions, cài **Unity** (của Microsoft). Extension này tự cài kèm **C# Dev Kit** và **C#**. Nếu được hỏi cài **.NET SDK** thì đồng ý.
- [ ] Cài một trợ lý AI trong VS Code: **GitHub Copilot** (có gói miễn phí, dùng chế độ *Agent*) hoặc **Gemini Code Assist** (dùng tài khoản Google). Nếu bạn dùng **Antigravity** thì không cần bước này: Antigravity là editor riêng, làm thay VS Code được.
- [ ] **Git** từ git-scm.com. Khuyên dùng thêm **GitHub Desktop** cho dễ thao tác.
- [ ] **7-Zip** (7-zip.org) để giải nén file `.rar`.

## Phần B – Tạo project

- [ ] Unity Hub > **Projects** > **New project** > chọn template **Universal 2D**.
- [ ] Project name: `VeBinhQuyDao`. Chọn thư mục lưu **không có dấu tiếng Việt và khoảng trắng**, ví dụ `D:\UnityProjects\`.
- [ ] Bấm **Create project** và chờ Unity mở.
- [ ] Kiểm tra Input System: **Window > Package Manager**, mục *In Project*, phải có **Input System**. Nếu không có: chuyển sang *Unity Registry*, tìm Input System, bấm Install, đồng ý khởi động lại Editor.
- [ ] **Edit > Project Settings > Player > Other Settings > Active Input Handling**: chọn **Input System Package (New)** hoặc **Both**.

## Phần C – Kết nối VS Code với Unity

- [ ] Unity: **Edit > Preferences** (Mac: *Unity > Settings*) **> External Tools > External Script Editor**: chọn **Visual Studio Code** (hoặc Browse tới file chạy của Antigravity).
- [ ] Bấm **Regenerate project files**.
- [ ] Kiểm tra trong Package Manager có package **Visual Studio Editor** (thường có sẵn).
- [ ] Test: Project window > chuột phải > **Create > MonoBehaviour Script** (hoặc *Create > Scripting > MonoBehaviour Script*), đặt tên `TestScript`, double-click. VS Code phải mở ra và tô màu, gợi ý code bình thường. Test xong thì xóa script này.

## Phần D – Git

- [ ] Tải file `.gitignore` mẫu cho Unity: trên GitHub tìm repo `github/gitignore`, lấy file `Unity.gitignore`, đổi tên thành `.gitignore`, đặt ở **thư mục gốc project** (cùng cấp với `Assets/`).
- [ ] Mở GitHub Desktop > **Add existing repository** > chọn thư mục project > *create a repository*. Commit đầu tiên: `Khoi tao project`.

## Phần E – Đưa bộ tài liệu vào project

Cấu trúc sau khi đặt xong:
```
VeBinhQuyDao/
├── AGENTS.md
├── .gitignore
├── Docs/
│   ├── 00_PLAN.md
│   ├── 01_GDD.md
│   ├── 02_REQUIREMENTS.md
│   ├── 03_SETUP_PROJECT.md
│   ├── 04_AI_CODING_GUIDE.md
│   ├── 05_UNITY_BUILD_GUIDE.md
│   └── ASSETS.md
├── Assets/
├── Packages/
└── ProjectSettings/
```
- [ ] `AGENTS.md` ở thư mục gốc, các file còn lại trong thư mục `Docs/`. Đặt `Docs/` **ngoài** `Assets/` để Unity không import các file này.
- [ ] Commit: `Them tai lieu`.

## Phần F – Tạo cấu trúc thư mục trong Unity

Trong Project window, chuột phải > **Create > Folder**, tạo đúng cây sau:
```
Assets/
├── Art/ShmupPack/
├── Audio/
├── Data/
├── Prefabs/
│   ├── Player/
│   ├── Weapons/
│   ├── Hazards/
│   ├── Enemies/
│   ├── PowerUps/
│   ├── Effects/
│   └── UI/
├── Scenes/
└── Scripts/
    ├── Core/  Player/  Weapons/  Hazards/  Enemies/  PowerUps/  Spawning/  UI/
```
- [ ] Template Universal 2D có sẵn `Assets/Scenes/SampleScene`. Đổi tên nó thành **`Game`**.

## Phần G – Tải và import asset

### G1. Tải
- [ ] Vào trang **itch.io: Free Shoot'em Up starter Asset Pack (Timberlate007)** > bấm **Download Now**. Ở hộp hỏi giá, bấm **"No thanks, just take me to the downloads"** rồi tải file `.rar`.
- [ ] (Cho bước M8) Vào **kenney.nl/assets**, tải các bộ âm thanh CC0, gợi ý: **Sci-Fi Sounds** (laser, nổ), **Interface Sounds** (nhặt vật phẩm, nút bấm). Nhạc nền: tìm "space music CC0" trên **OpenGameArt.org**.

### G2. Import
- [ ] Giải nén `.rar` bằng 7-Zip.
- [ ] Kéo **toàn bộ nội dung** đã giải nén vào `Assets/Art/ShmupPack/` trong Project window của Unity.
- [ ] Kéo file âm thanh (định dạng `.wav` hoặc `.ogg`) vào `Assets/Audio/`.

### G3. Cài đặt import cho sprite
- [ ] Trong `Assets/Art/ShmupPack/`, chọn **tất cả file PNG** (click file đầu, Shift + click file cuối; làm cho từng thư mục con).
- [ ] Trong Inspector đặt:
  - **Texture Type**: Sprite (2D and UI)
  - **Pixels Per Unit**: xem mục G4
  - **Filter Mode**: Point (no filter)
  - **Compression**: None
- [ ] Bấm **Apply**.

### G4. Chọn Pixels Per Unit (PPU)
- [ ] Click vào ảnh tàu người chơi, nhìn kích thước ở dưới khung preview trong Inspector (ví dụ `32x32`).
- [ ] Đặt PPU cho **mọi sprite** bằng **chiều rộng một chiếc tàu** (ví dụ tàu rộng 32 px thì PPU = 32). Khi đó tàu rộng khoảng 1 đơn vị Unity, khớp với các thông số tốc độ trong tài liệu.
- [ ] Nếu ảnh tàu là sprite sheet chứa nhiều tàu, sau bước G5 xem kích thước một ô.

### G5. Cắt sprite sheet
Ảnh nào chứa **nhiều hình** (animation nổ, lửa động cơ, mìn, các màu tàu, icon vật phẩm, đạn) thì phải cắt:
- [ ] Chọn ảnh > Inspector: **Sprite Mode = Multiple** > Apply.
- [ ] Bấm **Open Sprite Editor**. Nếu Unity báo thiếu package, cài **2D Sprite** trong Package Manager.
- [ ] Menu **Slice**:
  - Các hình cách nhau rõ ràng: **Type = Automatic** > Slice.
  - Hình xếp lưới đều nhau (animation): **Type = Grid By Cell Size**, nhập kích thước một ô (ví dụ 32 × 32) > Slice.
- [ ] Bấm **Apply** ở góc trên phải Sprite Editor.
- [ ] Kiểm tra: bấm vào mũi tên nhỏ cạnh ảnh trong Project window, phải thấy các sprite con `tênảnh_0`, `tênảnh_1`...

### G6. Ảnh nền (bắt buộc cho parallax kiểu lát gạch)
Nền được lát lặp lại (Tiled) để phủ kín bản đồ rộng, nên 5 ảnh nền cần cài riêng:
- [ ] Sprite Mode: **Single**
- [ ] Mesh Type: **Full Rect** (nằm trong mục *Advanced*)
- [ ] Wrap Mode: **Repeat**
- [ ] Bấm **Apply**

Nếu khi bay thấy đường kẻ mảnh giữa các ô lặp: kiểm tra lại Filter Mode = Point và Compression = None cho ảnh nền.

### G6b. Tâm ngắm
Game cần một sprite **tâm ngắm** (crosshair). Nếu bộ asset không có, chọn một trong các cách:
- Dùng sprite có sẵn của Unity: Project window > chuột phải > **Create > 2D > Sprites > Circle** (hoặc *Square*), sau này thu nhỏ lại.
- Tải bộ **Crosshair Pack** (CC0) của Kenney trên kenney.nl, import vào `Assets/Art/Crosshair/` với cài đặt như G3.

### G7. Commit
- [ ] Commit: `Import asset va cat sprite`.

Tiếp theo: sang **`04_AI_CODING_GUIDE.md`**, chạy **Prompt 0** để AI lập `ASSETS.md`.
