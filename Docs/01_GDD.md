# 01 – Game Design Document: "Vệ Binh Quỹ Đạo"

## 1. Tổng quan

| Mục | Nội dung |
|---|---|
| Tên game | Vệ Binh Quỹ Đạo (Orbit Guardian) |
| Thể loại | 2D arena shooter, nhìn từ trên xuống |
| Nền tảng | PC Windows, bàn phím + chuột |
| Màn hình | **Ngang, 16:9** |
| Bản đồ | Hình chữ nhật **64 × 36 đơn vị** (khoảng 3 × 3 màn hình), có viền phát sáng. Camera đi theo tàu |
| Thời lượng một lượt | Tối đa 3 phút |
| Phong cách | Pixel art (bộ asset Timberlate007) |

**Một câu mô tả:** Lái chiếc tàu chiến cuối cùng trong vùng không gian quanh Trạm Helios, xoay trở bắn hạ những đợt máy móc và thiên thạch lao tới từ mọi hướng, sống sót 3 phút để lá chắn của trạm kịp khởi động.

## 2. Cốt truyện

Năm 2387, Trạm Năng Lượng Helios là nguồn điện cuối cùng của loài người. Một đàn máy móc ngoài hành tinh đang bao vây vùng không gian quanh trạm, kéo theo cả vành đai thiên thạch. Lá chắn của trạm cần **3 phút** để khởi động.

Bạn là phi công của tàu chiến *Guardian*. Hệ thống ngắm của tàu có chế độ **tự khóa mục tiêu** cho những lúc bị bao vây. Nhiệm vụ: cầm chân kẻ địch đến khi lá chắn bật lên.

Thể hiện: 2–3 dòng ở menu chính và thông điệp ở màn hình kết thúc.
- Thắng: *"Lá chắn đã kích hoạt. Trạm Helios an toàn. Cảm ơn, Vệ Binh."*
- Thua: *"Tàu Guardian đã bị phá hủy. Trạm Helios đang gặp nguy..."*

## 3. Vòng lặp gameplay

```
Di chuyển & lướt né ──► Ngắm (chuột hoặc tự ngắm) ──► Bắn hạ ──► Điểm / vật phẩm
        ▲                                                            │
        └──────────── Kẻ địch dày hơn, đến từ nhiều phía ◄───────────┘
```

- **Thắng:** sống sót đến khi đồng hồ 3:00 về 0:00.
- **Thua:** mất hết 3 mạng.
- **Mục tiêu phụ:** điểm cao nhất (lưu lại).

## 4. Điều khiển

| Phím / chuột | Hành động |
|---|---|
| **W A S D** | Di chuyển 8 hướng theo màn hình (có quán tính nhẹ: tăng tốc, giảm tốc mượt) |
| **Di chuột** | Tàu xoay mũi theo con trỏ. Con trỏ hệ thống được thay bằng **tâm ngắm** |
| **Giữ chuột trái** (hoặc giữ **Space**) | Bắn liên tục theo hướng mũi tàu |
| **Q** | Bật / tắt **chế độ tự ngắm** |
| **Shift** | **Lướt né** (Dash) theo hướng đang di chuyển |
| **Esc** | Tạm dừng / tiếp tục |

Hướng di chuyển và hướng ngắm **độc lập**: bạn có thể bay lùi ra xa trong khi vẫn bắn về phía kẻ địch.

### 4.1 Chế độ tự ngắm (Q)
- Tàu tự xoay về **mục tiêu gần nhất** trong bán kính **10 đơn vị** và **tự bắn**, người chơi chỉ lo di chuyển.
- Tâm ngắm chuyển màu đỏ và **dính vào mục tiêu** đang bị khóa.
- Không có mục tiêu trong tầm: tàu quay lại ngắm theo chuột, không tự bắn.
- Đánh đổi: khi tự ngắm, tốc độ bắn **chậm hơn 20%**. Ngắm tay vẫn mạnh hơn nếu bạn khéo.
- HUD hiện rõ chế độ hiện tại.

### 4.2 Lướt né (Shift)
- Lao nhanh khoảng 3 đơn vị trong 0,15 giây theo hướng đang bấm WASD (đứng yên thì lướt theo hướng mũi tàu).
- **Bất tử trong lúc lướt.** Hồi chiêu **1,2 giây**, có thanh hồi chiêu trên HUD.

## 5. Bản đồ & camera

- Tàu không bay ra khỏi viền bản đồ.
- Camera bám theo tàu có độ trễ nhẹ cho mượt, hơi **nhìn về phía đang ngắm** để người chơi thấy rõ phía trước. Camera không đi quá viền bản đồ (chỉ lố ra 1 đơn vị để thấy viền).
- Nền nhiều lớp sao **parallax**: lớp xa gần như đứng yên, lớp gần trượt nhanh hơn khi tàu bay, tạo chiều sâu.

## 6. Người chơi

- 3 mạng. Trúng đòn: mất 1 mạng, camera rung, tàu nhấp nháy và **bất tử 2 giây**.
- Mạng về 0: tàu nổ, sau 1,5 giây hiện Game Over.
- Lửa động cơ phía sau tàu.

## 7. Chướng ngại vật

Xuất hiện ngay ngoài tầm nhìn, trôi **xuyên qua vùng quanh tàu** theo hướng ngẫu nhiên.

| Tên | Máu | Hành vi | Điểm |
|---|---|---|---|
| Thiên thạch lớn | 3 | Trôi chậm, tự xoay. Bị bắn hạ: **vỡ thành 2 thiên thạch vừa** bay tỏa ra | 30 |
| Thiên thạch vừa | 2 | Vỡ thành **2 thiên thạch nhỏ** | 20 |
| Thiên thạch nhỏ | 1 | Nổ biến mất | 10 |
| Mìn | 2 | Trôi rất chậm, nhấp nháy. Khi nổ gây sát thương **trong bán kính 1,5** | 25 |

Đâm vào người chơi: người chơi mất 1 mạng, đối tượng nổ (không vỡ, không cho điểm). Bắn mìn khi đứng gần cũng bị dính sát thương.

## 8. Kẻ địch

Xuất hiện ngay ngoài tầm nhìn, từ **mọi phía**, đều hướng về tàu. Có 3 loại.

| Tên | Máu | Hành vi | Điểm | Rơi đồ |
|---|---|---|---|---|
| **Diver** | 1 | Lao thẳng về vị trí tàu lúc xuất hiện (lệch ngẫu nhiên vài độ), rồi bay thẳng mãi. Né bằng cách dịch sang bên | 10 | 4% |
| **Gunner** | 4 | Tiến đến khoảng cách 6 đơn vị rồi **bay vòng quanh tàu**, luôn hướng về tàu và **bắn vào tàu**. Sau 10 giây thì bay đi | 40 | 15% |
| **Kamikaze** | 1 | Trôi chậm 0,8 giây, sau đó **bám đuổi** tàu, xoay hướng có giới hạn (né được bằng lướt hoặc vòng gấp) | 30 | 6% |

Đâm vào người chơi: người chơi mất 1 mạng, kẻ địch nổ (không cho điểm).

## 9. Vật phẩm

Rơi ra khi bắn hạ kẻ địch hoặc thiên thạch, **lơ lửng tại chỗ** và nhấp nhô. Tồn tại **10 giây** (3 giây cuối nhấp nháy). Chạm vào để nhặt.

| Tên | Hiệu ứng | Thời gian |
|---|---|---|
| **Triple Shot** | Bắn 3 tia tỏa ra | 8 giây |
| **Shield** | Khiên chặn 1 đòn | Đến khi bị trúng |
| **Extra Life** | +1 mạng (tối đa 3). Đã đủ mạng: +200 điểm | Tức thì |

Nhặt lại Triple Shot khi đang có hiệu lực: làm mới thời gian.

## 10. Độ khó

Một màn duy nhất. Đối tượng chọn ngẫu nhiên **có trọng số**, mở khóa dần theo thời gian, sinh ở vị trí ngẫu nhiên **ngay ngoài tầm nhìn camera** (luôn nằm trong bản đồ, nên khi tàu ở sát góc bản đồ thì kẻ địch chỉ đến từ các phía còn trống). Tối đa 45 đối tượng cùng lúc.

| Thời điểm | Mở khóa thêm | Khoảng cách giữa 2 lần sinh (xấp xỉ) |
|---|---|---|
| 0:00 | Thiên thạch lớn, vừa, Diver | 1,4 giây |
| 0:30 | Mìn | 1,1 giây |
| 1:00 | Gunner | 0,8 giây |
| 1:30 | Kamikaze | 0,6 giây |
| 2:30 – 3:00 | Giai đoạn cao trào | 0,35 giây |

Cấu hình trong ScriptableObject `SpawnConfig`.

## 11. Điểm số

- Theo bảng ở mục 7 và 8.
- Thắng: **+1000** và **+500 mỗi mạng còn lại**.
- Điểm cao nhất lưu bằng `PlayerPrefs`, có thông báo "Kỷ lục mới!".
- Đếm Kills.

## 12. Giao diện

| Màn hình | Thành phần |
|---|---|
| **Menu chính** | Tên game, cốt truyện, kỷ lục, bảng phím điều khiển ngắn, nút Chơi / Credits / Thoát |
| **HUD** | Điểm (trái trên), Đồng hồ (giữa trên), Kills (phải trên), Icon mạng (dưới điểm), **chế độ ngắm** và **thanh hồi chiêu lướt** (trái dưới) |
| **Tâm ngắm** | Theo chuột; màu đỏ và dính vào mục tiêu khi tự ngắm đang khóa |
| **Tạm dừng** | Tiếp tục, Chơi lại, Về menu |
| **Game Over / Chiến thắng** | Thông điệp, điểm, kỷ lục, "Kỷ lục mới!", Chơi lại, Về menu |
| **Credits** | Nguồn asset, âm thanh, nút Đóng |

## 13. Âm thanh

Nhạc nền, tiếng bắn, nổ, trúng đòn, nhặt vật phẩm, lướt, bật/tắt tự ngắm. Nguồn: Kenney.nl (CC0), OpenGameArt.

## 14. Phạm vi

**Có:** mọi thứ ở trên.

**Không làm (hướng phát triển):** boss, nhiều màn, thêm loại địch (ví dụ địch bay lượn sóng), vật phẩm đạn đuổi và bắn nhanh, mũi tên chỉ địch ngoài màn hình, minimap, cửa hàng nâng cấp, nhiều tàu, chơi mạng, tay cầm.