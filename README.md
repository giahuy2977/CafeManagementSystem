# ☕ Cafe Management System — Ứng Dụng Quản Lý Quán Cà Phê Tại Quầy (WPF / C#)

Ứng dụng Desktop hỗ trợ nhân viên thu ngân và quản lý vận hành hoạt động bán hàng, quản lý bàn, gọi món tại quầy, thanh toán hóa đơn và thống kê doanh thu tại quán cà phê. Dự án được xây dựng theo kiến trúc phân tầng **MVVM + Service + DAO (ADO.NET thuần) + Microsoft SQL Server**.

## 1. Công Nghệ Sử Dụng

* **Ngôn ngữ & Nền tảng**: C# (.NET), Windows Presentation Foundation (WPF)
* **Kiến trúc phần mềm**: `View (XAML) → ViewModel → Service → DAO (ADO.NET) → SQL Server`
* **Truy xuất dữ liệu**: ADO.NET thuần (`SqlConnection`, `SqlCommand`, `SqlParameter`, `SqlDataReader`, `SqlTransaction`) — Không sử dụng ORM (Entity Framework / Dapper)
* **Cơ sở dữ liệu**: Microsoft SQL Server & SQL Server Management Studio (SSMS)
* **Công cụ phát triển**: Microsoft Visual Studio 2022, Git & GitHub

## 2. Phạm Vi Chức Năng & Nghiệp Vụ Cốt Lõi

* **Quản lý bàn & Gọi món tại quầy (`SalesView`)**:
  * Theo dõi danh sách bàn theo trạng thái trực quan kèm nhãn chữ: `Available` (Bàn trống) và `Occupied` (Đang phục vụ).
  * Tìm kiếm món theo từ khóa, lọc theo danh mục (`Category`) và thêm món vào đơn hàng của bàn đang chọn.
  * Lưu đơn hàng (`Orders` & `OrderDetail`) xuống SQL Server ngay khi thêm món đầu tiên (`Status = 'Pending'`).
  * Điều chỉnh số lượng món, áp dụng giảm giá (`DiscountAmount`) theo quyền và tính tổng tiền (`TotalAmount = SubTotal - DiscountAmount`) tự động.
* **Thanh toán đơn giản hóa (`PaymentDialog`)**:
  * Xác nhận thông tin thanh toán tại quầy; thực thi `SqlTransaction` chuyển trạng thái đơn từ `Pending → Completed`, ghi nhận `PaidAt` và giải phóng bàn về `Available`.
* **Quản lý Thực đơn (`MenuView`)**:
  * Quản lý danh mục (`Category`) và sản phẩm (`Product`) ngay trên **Inline Panel** (không mở cửa sổ phụ).
  * Hỗ trợ **Soft Delete (`IsDeleted = 1`)** và trạng thái mở bán (`IsAvailable`), đồng thời lưu cứng `UnitPrice` trong `OrderDetail` để bảo toàn lịch sử giá cho hóa đơn cũ.
* **Tra cứu Hóa đơn (`InvoiceView` & `InvoiceDetailDialog`)**:
  * Tìm kiếm theo mã đơn (`OrderId` / `OrderCode`), lọc theo khoảng ngày, bàn, trạng thái và xem chi tiết từng món trong hóa đơn.
* **Báo cáo Doanh thu (`RevenueView`)**:
  * Dành cho `Manager`: thống kê doanh thu hôm nay, doanh thu theo kỳ, tổng số hóa đơn hoàn thành, tổng số sản phẩm bán ra và bảng xếp hạng món bán chạy.
* **Cài đặt & Tài khoản (`SettingsView` & `LoginWindow`)**:
  * Đăng nhập phân quyền 2 vai trò (`Staff` và `Manager`), đổi mật khẩu cá nhân trên **Inline Panel** và đăng xuất an toàn.

## 3. Kiến Trúc Giao Diện (2 Window – 5 View – 2 Dialog)

```text
LoginWindow (Xác thực tài khoản Staff / Manager)
     │
     ▼
MainWindow (Cửa sổ chính duy nhất trong phiên làm việc)
 ├── Sidebar (Trái): Bán hàng | Thực đơn | Hóa đơn | Doanh thu | Cài đặt
 └── ContentControl (Phải):
      ├── 1. SalesView    (TablePanel + MenuPanel + OrderPanel) ──► Mở Modal: PaymentDialog
      ├── 2. MenuView     (Danh sách món + Inline Panel Thêm/Sửa/Xóa mềm)
      ├── 3. InvoiceView  (Tra cứu & lọc lịch sử hóa đơn)       ──► Mở Modal: InvoiceDetailDialog
      ├── 4. RevenueView  (Thống kê KPI doanh thu & Top sản phẩm - Chỉ Manager)
      └── 5. SettingsView (Inline Panel Đổi mật khẩu, Giao diện & Đăng xuất)
```

## 4. Cấu Trúc 7 Thư Mục Chuẩn Của Dự Án

Toàn bộ dự án chỉ gồm đúng **7 thư mục gốc** (không tạo thư mục con lồng nhau phức tạp):

```text
CafeManagementSystem/
├── Models/                         # 1. Chứa các class dữ liệu (Account, Category, Product, CoffeeTable, Order, OrderDetail...)
├── Views/                          # 2. Chứa toàn bộ 9 màn hình (LoginWindow, MainWindow, 5 View và 2 Dialog)
├── ViewModels/                     # 3. Chứa các ViewModel tương ứng (BaseViewModel, LoginViewModel, MainViewModel, SalesViewModel...)
├── Services/                       # 4. Chứa logic nghiệp vụ, kiểm tra quyền Staff/Manager & tính tiền
├── Data/                           # 5. Chứa DatabaseConnection.cs, các lớp DAO (ADO.NET) và file CafeManagementDb.sql
├── Helpers/                        # 6. Chứa RelayCommand.cs, UserSession.cs và các lớp hỗ trợ
└── Resources/                      # 7. Chứa Colors.xaml và MainStyles.xaml (Quy chuẩn Flat Design dùng chung)
```

## 5. Quy Chuẩn Giao Diện Dùng Chung (Design System — Tông Trầm Quán Cafe)

Cả 3 thành viên bắt buộc sử dụng các Resource đã định nghĩa sẵn trong `Resources/Colors.xaml` và `Resources/MainStyles.xaml`:

### 5.1. Nguyên tắc thiết kế bắt buộc
1. **Flat Design**: Không dùng gradient, không đổ bóng (`DropShadowEffect`), không hiệu ứng nổi 3D. Phân tách các khu vực bằng màu nền và viền mảnh `1px`.
2. **Tông màu đất trầm**: Tuyệt đối không dùng màu neon hoặc màu bão hòa cao. Chữ nội dung đảm bảo độ tương phản $\ge 4.5:1$.
3. **Nhãn trạng thái rõ ràng**: Màu sắc không là cách duy nhất để phân biệt trạng thái — luôn hiển thị kèm chữ (ví dụ: `Bàn trống (Available)`, `Đang phục vụ (Occupied)`, `Đã thanh toán (Completed)`).
4. **Định dạng chuẩn**: Tiền tệ định dạng `87.000đ` (dấu chấm ngăn nghìn, đuôi `đ`); Thời gian định dạng `14:32 - 06/11/2026`.

### 5.2. Bảng mã màu chuẩn (`Resources/Colors.xaml`)

| Vai trò hiển thị | Tên Resource Key (`x:Key`) | Mã Hex | Ghi chú sử dụng |
| :--- | :--- | :--- | :--- |
| **Sidebar / Nút chính** | `PrimaryBrush` | `#4A2E1F` | Nâu Espresso đậm, chữ trắng (`#FFFFFF`) |
| **Nhấn đậm / Hover** | `PrimaryDarkBrush` | `#33201A` | Dark Roast khi di chuột hoặc tiêu đề đậm |
| **Nút phụ / Bàn đang chọn** | `SecondaryBrush` | `#7A4B2F` | Nâu Walnut, chữ trắng |
| **Điểm nhấn / Biểu đồ** | `AccentBrush` | `#8B5E3C` | Light Brown cho nút hành động / cột thống kê |
| **Nền ứng dụng** | `AppBackgroundBrush` | `#F1EAE3` | Nền Linen sáng dịu mắt cho vùng nội dung |
| **Nền Card / Panel** | `SurfaceCardBrush` | `#FFFFFF` | Nền trắng cho các khung Panel và bảng |
| **Nền phụ / Xen kẽ** | `SurfaceAltBrush` | `#F6F3EF` | Nền Paper cho dòng bảng xen kẽ |
| **Viền / Đường kẻ** | `BorderBrushDefault` | `#D8C9BB` | Mocha Line `1px` |
| **Chữ chính** | `TextPrimaryBrush` | `#2B1B14` | Màu chữ nội dung chính (Bean Ink) |
| **Chữ phụ** | `TextSecondaryBrush` | `#6F5A4C` | Màu chữ chú thích, nhãn phụ |
| **Nền Badge / Dòng chọn** | `TintBrush` | `#E6D6C6` | Cocoa Tint |
| **Thành công (Chữ / Nền)** | `StatusOkBrush` / `StatusOkBgBrush` | `#2F5D3A` / `#DDEBDD` | Dùng cho `Completed`, `Available` |
| **Chờ xử lý (Chữ / Nền)** | `StatusWarnBrush` / `StatusWarnBgBrush` | `#7A4A0C` / `#F5E3C8` | Dùng cho `Pending` |
| **Đang phục vụ / Bàn có khách** | `TableOccupiedBrush` | `#1A1A1A` | Nền đen than cho thẻ bàn `Occupied` |
| **Lỗi / Hủy (Chữ / Nền)** | `StatusErrBrush` / `StatusErrBgBrush` | `#8F2C22` / `#F3DAD5` | Dùng cho `Cancelled`, xóa món, lỗi nhập liệu |

### 5.3. Quy ước Kích thước, Bo góc & Khoảng cách
* **Font chữ**: `Be Vietnam Pro` (hoặc `Segoe UI`), cỡ chữ chuẩn: `12` (chú thích), `14` (nội dung/bảng), `16` (tiêu đề khu vực), `20` (tiêu đề màn hình), `24` (số KPI lớn).
* **Bo góc (`CornerRadius`)**: Khung Panel/Card `10px`, Nút bấm & Ô nhập `8px`, Badge trạng thái `6px`.
* **Khoảng cách (`Margin` / `Padding`)**: Bội số của `4px` (`4, 8, 12, 16, 24`). Chiều cao chuẩn của `Button` và `TextBox` là `38px` – `42px`.

## 6. Phân Công Nhiệm Vụ Nhóm 3 Thành Viên

| Thành viên | Vai trò | Màn hình & Module phụ trách |
| :--- | :--- | :--- |
| **Thành viên 1 (Trưởng nhóm)** | Kiến trúc lõi, Database & Bán hàng POS | Khung `MainWindow`, `NavigationService`, `Resources/`, `SalesView` (`TablePanel`, `MenuPanel`, `OrderPanel`), `PaymentDialog`, `OrderService` & `OrderDao`. |
| **Thành viên 2** | Xác thực, Thực đơn & Cài đặt | `LoginWindow`, `AuthenticationService`, `UserSession`, `MenuView` (Inline Panel + Soft Delete), `SettingsView` (Đổi mật khẩu & Logout), `ProductService` & `CategoryService`. |
| **Thành viên 3** | Hóa đơn, Doanh thu & Kiểm thử | `InvoiceView`, `InvoiceDetailDialog`, `RevenueView` (KPI & Top sản phẩm bán chạy), `RevenueService`, `RevenueDao` và kịch bản kiểm thử tích hợp. |

## 7. Hướng Dẫn Cài Đặt & Chạy Dự Án

1. **Khởi tạo Cơ sở dữ liệu**:
   * Mở **SQL Server Management Studio (SSMS)**, kết nối tới SQL Server cục bộ (`.\SQLEXPRESS` hoặc `(local)`).
   * Mở file `Data/CafeManagementDb.sql` và nhấn **Execute (F5)** để tạo Database `CafeManagementDb`, 6 bảng và dữ liệu mẫu.
2. **Cấu hình Chuỗi kết nối**:
   * Mở file `Data/DatabaseConnection.cs` trong Visual Studio và kiểm tra tên `Server=...` khớp với tên SQL Server trên máy của bạn.
3. **Biên dịch & Chạy ứng dụng**:
   * Mở `CafeManagementSystem.sln` bằng **Visual Studio 2022**.
   * Nhấn **Start (F5)** để chạy ứng dụng.
   * Tài khoản đăng nhập mẫu:
     * Quản lý (`Manager`): Username `manager` / Password `123456`
     * Nhân viên (`Staff`): Username `staff` / Password `123456`

## 8. Quy Trình Làm Việc Với Git Cho Nhóm

1. Tuyệt đối không push trực tiếp lên nhánh `main`.
2. Trước khi bắt đầu làm việc, luôn cập nhật code mới nhất:
   ```bash
   git checkout main
   git pull origin main
   ```
3. Tạo nhánh riêng theo màn hình/chức năng đang làm:
   ```bash
   git checkout -b ui/ten-man-hinh
   ```
4. Commit code rõ ràng và đẩy nhánh lên GitHub để tạo **Pull Request (PR)** vào `main`.