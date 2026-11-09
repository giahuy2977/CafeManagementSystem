# 🎨 DESIGN.md — Quy Chuẩn Thiết Kế Giao Diện & Quy Tắc Đặt Tên (WPF / MVVM / C#)

Tài liệu này là quy chuẩn dùng chung bắt buộc cho cả 3 thành viên trong nhóm thực hiện đồ án **Cafe Management System**. Mọi màn hình (`Window`, `UserControl`, `Dialog`) và mã nguồn C# / XAML / SQL phải tuân thủ đồng nhất các quy định dưới đây.

## 1. Quy Tắc Thiết Kế Chung (Bắt Buộc)

| # | Quy tắc thiết kế |
| :--- | :--- |
| **1** | **Flat Design (Thiết kế phẳng)**: Không dùng gradient, không đổ bóng (`DropShadowEffect`), không hiệu ứng nổi 3D. Phân tách các khu vực bằng màu nền và đường viền mảnh `1px`. |
| **2** | **Tông màu đất trầm**: Tuyệt đối không dùng màu neon hoặc màu bão hòa cao. Chỉ sử dụng các mã màu đã khai báo trong `Resources/Colors.xaml`. |
| **3** | **Độ tương phản chữ**: Chữ nội dung phải đạt độ tương phản $\ge 4.5:1$ so với màu nền. Các màu trang trí (`Caramel`, `Tan`) không được dùng cho chữ nhỏ. |
| **4** | **Đồng bộ kích thước**: Sử dụng cùng một bộ font chữ, độ bo góc (`CornerRadius`), khoảng cách (`Margin`/`Padding`) và bộ icon ở tất cả 9 màn hình. |
| **5** | **Nhãn trạng thái rõ ràng**: Màu sắc không là cách duy nhất để phân biệt trạng thái — luôn hiển thị kèm nhãn chữ (ví dụ: `Bàn trống (Available)`, `Đang phục vụ (Occupied)`, `Đã thanh toán (Completed)`). |
| **6** | **Định dạng hiển thị chuẩn**: Tiền tệ định dạng `87.000đ` (dấu chấm ngăn nghìn, đuôi `đ`, dùng `StringFormat={}{0:N0}đ`); Thời gian định dạng `14:32 - 07/11/2026`. |

## 2. Bảng Màu Chuẩn Quán Cafe (`Resources/Colors.xaml`)

### 2.1. Bảng màu giao diện chính (Tông Espresso & Linen)

| Vai trò hiển thị | Tên Resource (`x:Key`) | Mã Hex | Ghi chú sử dụng |
| :--- | :--- | :--- | :--- |
| **Sidebar / Nút chính** | `PrimaryBrush` | `#4A2E1F` | Nâu Espresso đậm — Dùng cho thanh Sidebar trái, Header Dialog, Nút chính (chữ trắng `#FFFFFF`). |
| **Hover / Nhấn đậm** | `PrimaryDarkBrush` | `#33201A` | Dark Roast — Màu khi di chuột (`IsMouseOver`) lên nút chính hoặc mục Sidebar. |
| **Nút phụ / Đang chọn** | `SecondaryBrush` | `#7A4B2F` | Nâu Walnut — Dùng cho nút phụ hoặc viền nhấn thẻ đang chọn. |
| **Điểm nhấn / Hành động** | `AccentBrush` | `#8B5E3C` | Light Brown — Dùng cho nút Thanh toán, Lưu, cột biểu đồ chính. |
| **Nền tổng thể vùng phải** | `AppBackgroundBrush` | `#F1EAE3` | Nền Linen sáng — Màu nền của vùng nội dung chính bên phải Sidebar. |
| **Nền Card / Panel** | `SurfaceCardBrush` | `#FFFFFF` | Trắng tinh — Nền của `TablePanel`, `MenuPanel`, `OrderPanel`, `DataGrid` và Inline Form. |
| **Nền phụ / Dòng xen kẽ** | `SurfaceAltBrush` | `#F6F3EF` | Nền Paper — Dùng cho dòng xen kẽ trong bảng hoặc chân hộp thoại. |
| **Đường viền / Kẻ bảng** | `BorderBrushDefault` | `#D8C9BB` | Mocha Line — Viền `1px` bao quanh Card, `TextBox`, `DataGrid`. |
| **Nền Badge / Hàng chọn** | `TintBrush` | `#E6D6C6` | Cocoa Tint — Nền khi chọn dòng hoặc nhãn phân loại. |
| **Chữ nội dung chính** | `TextPrimaryBrush` | `#2B1B14` | Bean Ink — Màu chữ tiêu đề, tên món, số tiền. |
| **Chữ chú thích phụ** | `TextSecondaryBrush` | `#6F5A4C` | Muted Brown — Màu chữ nhãn phụ, thời gian, gợi ý. |
| **Chữ trên nền tối** | `TextOnDarkBrush` | `#FFFFFF` | Trắng — Dùng cho chữ trên nền Sidebar và nút chính. |

### 2.2. Bảng màu trạng thái nghiệp vụ (Chữ đậm trên nền nhạt)

| Trạng thái nghiệp vụ | Resource Chữ (`Foreground`) | Resource Nền (`Background`) | Áp dụng cho |
| :--- | :--- | :--- | :--- |
| **Thành công / Khả dụng** | `StatusOkBrush` (`#2F5D3A`) | `StatusOkBgBrush` (`#DDEBDD`) | Hóa đơn `Completed`, Sản phẩm `Available` (Đang bán). |
| **Chờ xử lý / Tạm tính** | `StatusWarnBrush` (`#7A4A0C`) | `StatusWarnBgBrush` (`#F5E3C8`) | Đơn hàng `Pending` (Đang phục vụ). |
| **Thông tin phụ** | `StatusInfoBrush` (`#33506B`) | `StatusInfoBgBrush` (`#DCE6EE`) | Nhãn thông tin bổ sung. |
| **Lỗi / Đã hủy / Xóa** | `StatusErrBrush` (`#8F2C22`) | `StatusErrBgBrush` (`#F3DAD5`) | Đơn `Cancelled`, Món `Out of Stock`, Nút Xóa/Đăng xuất, Báo lỗi nhập liệu. |
| **Thẻ Bàn Trống (`Available`)** | `TextOnDarkBrush` (`#FFFFFF`) | `TableAvailableBrush` (`#2F5D3A`) | Thẻ bàn trống trên `TablePanel` (Kèm chữ `Available`). |
| **Thẻ Bàn Có Khách (`Occupied`)** | `TextOnDarkBrush` (`#FFFFFF`) | `TableOccupiedBrush` (`#1A1A1A`) | Thẻ bàn đang có khách trên `TablePanel` (Kèm chữ `Occupied`). |

## 3. Quy Chuẩn Font Chữ, Bo Góc, Khoảng Cách & Icon

* **Font chữ (`FontFamily`)**: Sử dụng `"Be Vietnam Pro, Segoe UI"` (ưu tiên `Be Vietnam Pro`, tự động dùng `Segoe UI` có sẵn trên mọi máy Windows nếu máy chưa cài font).
  * Tiêu đề màn hình (View Header): `FontSize="20"`, `FontWeight="Bold"`
  * Tiêu đề khu vực / Card Header / Tổng tiền: `FontSize="16"`, `FontWeight="SemiBold"`
  * Nội dung chính / Tên món / Dòng bảng / Nút bấm: `FontSize="14"`, `FontWeight="Normal"` (Nút bấm dùng `SemiBold`)
  * Chữ phụ / Nhãn chú thích / Thời gian: `FontSize="12"`, `Foreground="{StaticResource TextSecondaryBrush}"`
* **Độ bo góc (`CornerRadius`)**:
  * Khung Dialog (`PaymentDialog`, `InvoiceDetailDialog`): `CornerRadius="12"`
  * Khung Card / Panel lớn & Thẻ bàn / Thẻ món: `CornerRadius="10"`
  * Nút bấm (`Button`) & Ô nhập (`TextBox`, `ComboBox`, `PasswordBox`): `CornerRadius="8"`
  * Nhãn trạng thái (`Badge`): `CornerRadius="6"`
* **Khoảng cách (`Margin` & `Padding` theo hệ lưới 4px)**:
  * Chỉ dùng các giá trị là bội số của `4`: `4`, `8`, `12`, `16`, `20`, `24`.
  * Lề ngoài của mỗi View (`UserControl`): `Margin="16"`.
  * Khoảng cách giữa các Panel cạnh nhau: `16px`.
  * Padding bên trong Card/Panel: `Padding="16"` (Thẻ món/bàn nhỏ dùng `Padding="12"`).
  * Chiều cao chuẩn của `Button`, `TextBox`, `ComboBox`: `Height="38"` hoặc `Height="40"`.
* **Biểu tượng (`Icon`)**:
  * Không dùng Emoji nhiều màu làm icon.
  * Sử dụng đồng nhất font icon có sẵn của Windows **`Segoe MDL2 Assets`** (hoặc `Path` nét mảnh `1.8px` – `2px`):
    * Bán hàng (`Sales`): `&#xE7BF;` | Thực đơn (`Menu`): `&#xE8FD;` | Hóa đơn (`Invoices`): `&#xE8A1;`
    * Doanh thu (`Revenue`): `&#xE9D2;` | Cài đặt (`Settings`): `&#xE713;` | Đăng xuất (`Logout`): `&#xF3B1;`
    * Tìm kiếm (`Search`): `&#xE721;` | Thêm (`+`): `&#xE710;` | Xóa (`Delete`): `&#xE74D;`

## 4. Quy Tắc Đặt Tên Thống Nhất (Naming Conventions)

Để code của cả 3 thành viên ghép lại như do một người viết, toàn bộ dự án bắt buộc tuân thủ quy ước đặt tên C# / WPF / SQL Server dưới đây:

### 4.1. Đặt tên File và Class theo từng tầng

| Tầng (Thư mục) | Quy tắc đặt tên | Ví dụ ĐÚNG | Ví dụ SAI (Không dùng) |
| :--- | :--- | :--- | :--- |
| **`Views/Windows/`** | `PascalCase` + hậu tố `Window` | `LoginWindow.xaml`, `MainWindow.xaml` | `frmLogin.xaml`, `Main.xaml` |
| **`Views/UserControls/`** | `PascalCase` + hậu tố `View` | `SalesView.xaml`, `MenuView.xaml`, `InvoiceView.xaml`, `RevenueView.xaml`, `SettingsView.xaml` | `ucSales.xaml`, `MenuPage.xaml`, `SalesScreen.xaml` |
| **`Views/Dialogs/`** | `PascalCase` + hậu tố `Dialog` | `PaymentDialog.xaml`, `InvoiceDetailDialog.xaml` | `PopupPayment.xaml`, `DetailWindow.xaml` |
| **`ViewModels/`** | Tên View + hậu tố `ViewModel` | `BaseViewModel.cs`, `SalesViewModel.cs`, `MenuViewModel.cs`, `PaymentDialogViewModel.cs` | `SalesVM.cs`, `VM_Menu.cs` |
| **`Services/`** | Tên thực thể/nghiệp vụ + hậu tố `Service` | `AuthenticationService.cs`, `NavigationService.cs`, `OrderService.cs`, `ProductService.cs` | `OrderBUS.cs`, `BllProduct.cs` |
| **`Data/` (DAO)** | Tên thực thể + hậu tố `Dao` | `DatabaseConnection.cs`, `AccountDao.cs`, `ProductDao.cs`, `OrderDao.cs`, `OrderDetailDao.cs` | `ProductDAL.cs`, `OrderRepository.cs`, `SQL_Order.cs` |
| **`Models/`** | `PascalCase` danh từ số ít | `Account.cs`, `Category.cs`, `Product.cs`, `CoffeeTable.cs`, `Order.cs`, `OrderDetail.cs` | `tb_Product.cs`, `Products.cs`, `clsOrder.cs` |

### 4.2. Đặt tên trong Code C# (ViewModel, Service, DAO, Model)

| Thành phần C# | Quy chuẩn | Ví dụ ĐÚNG | Ví dụ SAI |
| :--- | :--- | :--- | :--- |
| **Property & Public Method** | `PascalCase` | `SelectedTable`, `TotalAmount`, `LoadProducts()`, `CompletePayment()` | `selected_table`, `total_amount`, `load_products()` |
| **Lệnh nút bấm (`ICommand`)** | `PascalCase` + Động từ + `Command` | `LoginCommand`, `SelectTableCommand`, `AddProductCommand`, `ConfirmPaymentCommand` | `btnLogin_Click`, `CmdAdd`, `PayCmd` |
| **Biến Private trong Class** | `_camelCase` (có dấu `_` ở đầu) | `_selectedTable`, `_searchKeyword`, `_orderService`, `_productDao` | `m_table`, `selectedTable_`, `Order_Service` |
| **Tham số hàm & Biến cục bộ** | `camelCase` | `int orderId`, `decimal discountAmount`, `string searchKeyword` | `int OrderID`, `decimal Discount_Amount` |
| **Cờ trạng thái Boolean** | Tiền tố `Is` / `Has` / `Can` + `PascalCase` | `IsAvailable`, `IsDeleted`, `IsActive`, `IsManager`, `HasError` | `Available_Flag`, `Deleted`, `CheckManager` |

### 4.3. Đặt tên trong XAML (`Views/` và `Resources/`)

* **Tên Resource màu sắc (`Colors.xaml`)**: `PascalCase` + hậu tố `Brush` (ví dụ: `PrimaryBrush`, `SurfaceCardBrush`, `StatusOkBrush`).
* **Tên Style dùng chung (`MainStyles.xaml`)**: `PascalCase` + Loại Control + hậu tố `Style` (ví dụ: `PrimaryButtonStyle`, `SecondaryButtonStyle`, `DangerButtonStyle`, `FlatCardBorderStyle`).
* **Đặt tên `x:Name` trên Control XAML**:
  * **Nguyên tắc MVVM**: Hạn chế tối đa đặt `x:Name` cho `TextBox`, `Button`, `DataGrid` vì dữ liệu đã gắn qua `{Binding ...}` và `{Binding ...Command}`.
  * Trường hợp bắt buộc cần tên trên UI (như `PasswordBox` hoặc vùng khung chính), đặt theo `PascalCase` + Loại Control: `PasswordInputBox`, `ConfirmPasswordInputBox`. Tuyệt đối không để tên mặc định `textBox1`, `button1`, `grid2`.

### 4.4. Đặt tên trong Cơ sở dữ liệu SQL Server (`CafeManagementDb.sql`)

* **Tên Bảng**: `PascalCase` (`Account`, `Category`, `Product`, `CoffeeTable`, `Orders`, `OrderDetail`).
* **Tên Khóa chính (`Primary Key`)**: `TênBảng + Id` (`AccountId`, `CategoryId`, `ProductId`, `CoffeeTableId`, `OrderId`, `OrderDetailId`).
* **Tên Khóa ngoại (`Foreign Key`)**: Giữ nguyên tên khóa chính của bảng được tham chiếu (`CategoryId`, `CoffeeTableId`, `AccountId`, `OrderId`, `ProductId`).
* **Cấm dùng**: Không dùng gạch dưới hay viết hoa toàn bộ như `ID_Product`, `Product_Name`, `CATEGORY_ID`, `Ma_Ban`.
* **Tên Tham số SQL (`SqlParameter`)**: `@PascalCase` khớp với tên cột (ví dụ: `@ProductId`, `@CategoryId`, `@UnitPrice`, `@DiscountAmount`).

### 4.5. Đặt tên Nhánh Git (`Branch`) cho 3 thành viên

* Nhánh giao diện: `ui/ten-man-hinh` (ví dụ: `ui/sales-view`, `ui/menu-view`, `ui/invoice-view`).
* Nhánh chức năng/nghiệp vụ: `feature/ten-chuc-nang` (ví dụ: `feature/login-auth`, `feature/order-payment`, `feature/revenue-report`).
* Nhánh sửa lỗi: `fix/ten-loi` (ví dụ: `fix/discount-validation`).

## 5. Checklist Kiểm Tra Trước Khi Đẩy Code (Push / Pull Request)

Trước khi commit và đẩy một màn hình lên GitHub, mỗi thành viên tự kiểm tra 6 mục sau:

- [ ] **1. Màu sắc & Flat Design**: Không dùng mã màu lạ ngoài `Resources/Colors.xaml`; không có `DropShadowEffect` hay Gradient.
- [ ] **2. Nhãn trạng thái & Định dạng**: Trạng thái bàn và hóa đơn có chữ đi kèm màu; tiền tệ hiển thị đúng định dạng `87.000đ`.
- [ ] **3. Đúng quy tắc đặt tên**: Tên View, ViewModel, Property, Command, DAO, Service và cột SQL đúng chuẩn `PascalCase` / `_camelCase` ở Mục 4.
- [ ] **4. Không phá vỡ MVVM**: Không viết câu lệnh SQL (`SqlConnection`, `SqlCommand`) trong file `.xaml.cs` hoặc trong `ViewModel`.
- [ ] **5. Đúng phạm vi giao diện**: Không tự tạo thêm cửa sổ `Window` mới ngoài 2 Window và 2 Dialog đã chốt (Thêm/Sửa món và Đổi mật khẩu phải nằm ở **Inline Panel**).
- [ ] **6. Build sạch**: Bấm `Ctrl + Shift + B` trong Visual Studio đạt `Build succeeded (0 Error)`.
