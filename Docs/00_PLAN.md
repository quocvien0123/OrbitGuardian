# 00 – Kế hoạch end-to-end: "Vệ Binh Quỹ Đạo"

Bài tập lớn giữa kỳ, một người làm. Game 2D **arena shooter** màn hình ngang: tàu bay tự do trong bản đồ rộng, camera đi theo, kẻ địch kéo tới **từ mọi phía**. Ngắm bằng chuột, bấm **Q** để bật tự ngắm, **Shift** để lướt né. Sống sót 3 phút để thắng.

## 1. Bộ tài liệu

| File | Nội dung | Ai đọc |
|---|---|---|
| `AGENTS.md` (thư mục gốc) | Quy tắc bắt buộc cho AI | AI |
| `Docs/00_PLAN.md` | Kế hoạch tổng thể (file này) | Bạn |
| `Docs/01_GDD.md` | Thiết kế game: luật chơi, điều khiển, đối tượng, điểm số | Bạn + AI |
| `Docs/02_REQUIREMENTS.md` | Yêu cầu và đặc tả kỹ thuật từng script | AI (chính) + Bạn |
| `Docs/03_SETUP_PROJECT.md` | Cài công cụ, tạo project, tải và import asset | Bạn |
| `Docs/04_AI_CODING_GUIDE.md` | Cách cho AI đọc tài liệu và viết code, kèm prompt mẫu | Bạn |
| `Docs/05_UNITY_BUILD_GUIDE.md` | Hướng dẫn tạo đối tượng, kéo thả, lắp ráp trong Unity | Bạn |
| `Docs/ASSETS.md` | Danh sách asset (AI điền ở Giai đoạn 2) | AI + Bạn |

## 2. Quy trình tổng quan

```
Giai đoạn 1: Cài đặt & tạo project ............ 03_SETUP_PROJECT.md
        │
Giai đoạn 2: Import asset + AI lập ASSETS.md .... 03 (phần cuối) + 04 (Prompt 0)
        │
Giai đoạn 3: Lặp theo milestone M1 → M8
        │    ┌──────────────────────────────────────────────┐
        │    │ a. AI viết code milestone (04_AI_CODING_GUIDE)│
        │    │ b. Unity biên dịch, sửa hết lỗi đỏ            │
        │    │ c. Lắp ráp trong Unity (05_UNITY_BUILD_GUIDE) │
        │    │ d. Bấm Play, test theo checklist              │
        │    │ e. Git commit                                 │
        │    └──────────────────────────────────────────────┘
        │
Giai đoạn 4: Cân chỉnh, build file .exe, chuẩn bị báo cáo
```

Nguyên tắc: **không bắt đầu milestone mới khi milestone hiện tại chưa chạy được.**

## 3. Các milestone

| Mã | Nội dung | Kết quả nhìn thấy được |
|---|---|---|
| M1 | Nền tảng: bản đồ (Arena), camera đi theo, nền parallax, pool, game manager | Thấy viền bản đồ, nền sao có chiều sâu, đồng hồ chạy |
| M2 | Người chơi: di chuyển quán tính, lướt né, ngắm chuột + tự ngắm (Q), bắn, máu, tâm ngắm | Bay khắp bản đồ, camera bám theo, tàu xoay theo chuột và bắn |
| M3 | Chướng ngại vật: thiên thạch 3 cỡ (vỡ đôi), mìn nổ lan | Bắn thiên thạch vỡ mảnh, tự ngắm khóa được mục tiêu |
| M4 | Kẻ địch: 3 loại hành vi đến từ mọi phía, dùng ScriptableObject | Diver lao tới, Gunner bay vòng quanh bắn, Kamikaze bám đuổi |
| M5 | Vật phẩm: 3 loại | Nhặt được 3 tia, khiên, thêm mạng |
| M6 | Sinh ngẫu nhiên ngoài tầm nhìn, độ khó tăng dần | Màn chơi tự chạy từ đầu đến cuối |
| M7 | Giao diện: HUD, tạm dừng, thua, thắng, menu chính | Game hoàn chỉnh có menu |
| M8 | Âm thanh, cân chỉnh, build | File .exe để nộp |

## 4. Lịch 4 tuần (điều chỉnh theo hạn nộp)

| Tuần | Việc | Hoàn thành khi |
|---|---|---|
| 1 | Giai đoạn 1, 2 + M1 + M2 | Bay, ngắm, bắn, lướt né trong bản đồ |
| 2 | M3 + M4 | Có thiên thạch, mìn, 3 loại địch (đặt tay để test) |
| 3 | M5 + M6 + M7 | Chơi được từ menu đến thắng/thua |
| 4 | M8 + báo cáo + slide | Có bản build, báo cáo, video demo |

Thiếu thời gian thì cắt theo thứ tự: mìn → Kamikaze → âm thanh. **Không cắt** M1, M2, M3, M6, M7.

## 5. Quy tắc vàng

1. **Commit Git sau mỗi milestone chạy được.** AI làm hỏng thì quay lại, đừng cố sửa chồng chéo.
2. **Mỗi milestone mở một cuộc trò chuyện mới với AI**, luôn kèm `AGENTS.md`, `01_GDD.md`, `02_REQUIREMENTS.md`, `ASSETS.md`.
3. **Hiểu code trước khi đi tiếp.** Sau mỗi milestone dùng prompt "Giải thích code" (file 04).
4. Số liệu cân bằng game chỉnh trong **Inspector** hoặc **ScriptableObject**, không sửa code.

## 6. Chuẩn bị nộp bài

- [ ] Bản build Windows chạy được trên máy khác.
- [ ] Video demo 2–3 phút: menu → chơi (ngắm chuột, bấm Q tự ngắm, lướt né) → nhặt vật phẩm → thắng hoặc thua.
- [ ] Báo cáo gồm: giới thiệu và cốt truyện, luật chơi, sơ đồ kiến trúc (mục 4 của `02_REQUIREMENTS.md`), giải thích các kỹ thuật chính:
  - **Object Pooling**
  - **ScriptableObject** cho dữ liệu kẻ địch và độ khó
  - **Ngắm bằng chuột** (đổi tọa độ màn hình sang thế giới, `Atan2`)
  - **Tự ngắm**: tìm mục tiêu gần nhất bằng danh sách đăng ký `Targetable`
  - **Sinh ngẫu nhiên có trọng số** ngay ngoài tầm nhìn camera
  - **Camera đi theo** có làm mượt và giới hạn trong bản đồ, **parallax** nhiều lớp
- [ ] Mục Credits ghi nguồn asset: *Shoot'em Up Starter Asset Pack – Timberlate007 (itch.io)*, âm thanh từ *Kenney.nl*.
- [ ] Ghi rõ phần nào có hỗ trợ của AI, nếu quy định môn học yêu cầu.