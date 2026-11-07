# 🎨 DESIGN.md — Quy chuẩn Thiết kế Giao diện (WPF Flat Design)

Tài liệu quy định màu sắc, font chữ và kích thước để cả 3 thành viên thiết kế giao diện đồng nhất 100%.

## 1. Bảng màu chuẩn Quán Cafe (Đã khai báo trong `Resources/Colors.xaml`)
- **PrimaryBrush (`#4A2E1F`)**: Nâu Espresso đậm — Dùng cho Sidebar, Nút chính.
- **AccentBrush (`#8B5E3C`)**: Nâu Caramel/Light Brown — Nút phụ, Điểm nhấn.
- **AppBackgroundBrush (`#F1EAE3`)**: Nền Linen sáng — Nền tổng thể vùng làm việc.
- **SurfaceCardBrush (`#FFFFFF`)**: Nền trắng — Dùng cho các Card, Panel, Bảng DataGrid.
- **BorderBrushDefault (`#D8C9BB`)**: Viền mảnh 1px.
- **Trạng thái**:
  - Hoàn tất / Bàn trống: Chữ `#2F5D3A` trên nền `#DDEBDD` (Kèm chữ "Available" / "Completed").
  - Đang phục vụ / Bàn có khách: Nền `#1A1A1A` chữ trắng (Kèm chữ "Occupied").
  - Hủy / Xóa / Lỗi: Chữ `#8F2C22` trên nền `#F3DAD5`.

## 2. Quy tắc Flat Design & Kích thước
- **Không dùng hiệu ứng nổi 3D, Gradient hay DropShadow** (để app chạy mượt).
- **Font chữ**: `Segoe UI` (hoặc `Be Vietnam Pro`). Cỡ chữ: `12` (phụ), `14` (chuẩn), `16` (tiêu đề nhóm), `20` (tiêu đề màn hình).
- **Bo góc (CornerRadius)**: Card/Panel = `10`, Nút bấm/Ô nhập = `8`, Badge = `6`.
- **Chiều cao chuẩn của Button / TextBox**: `38px` - `40px`.