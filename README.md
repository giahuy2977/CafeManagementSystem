# CafeManagementSystem (WPF Desktop)

Ứng dụng desktop Windows quản lý hoạt động bán hàng và gọi món tại quầy cho quán cà phê, được xây dựng bằng C#, WPF (.NET) theo kiến trúc MVVM và truy xuất cơ sở dữ liệu qua ADO.NET thuần với Microsoft SQL Server.

Repository này dành cho các thành viên trong nhóm clone mã nguồn, mở bằng Visual Studio 2022, cấu hình cơ sở dữ liệu và chạy thử ứng dụng.

---

## 1. Công nghệ sử dụng

- **Ngôn ngữ:** C# (.NET 8.0 / .NET 6.0+)
- **Nền tảng giao diện:** Windows Presentation Foundation (WPF), XAML
- **Mô hình kiến trúc:** MVVM (Model – View – ViewModel)
- **Truy xuất dữ liệu:** ADO.NET thuần (`SqlConnection`, `SqlCommand`, `SqlParameter`, `SqlDataReader`, `SqlTransaction`) — *Không sử dụng Entity Framework, Dapper hoặc ORM khác*
- **Hệ quản trị CSDL:** Microsoft SQL Server
- **Công cụ quản lý CSDL:** SQL Server Management Studio (SSMS)

> **Lưu ý:** Đây là ứng dụng desktop chạy trực tiếp trên Windows và kết nối trực tiếp với SQL Server qua ADO.NET. Để các chức năng bán hàng, gọi món, thực đơn và doanh thu hoạt động, SQL Server trên máy phải đang chạy và cơ sở dữ liệu phải được khởi tạo trước.

---

## 2. Yêu cầu trước khi chạy

Cài đặt trên máy tính (hệ điều hành Windows 10/11):

1. **Git for Windows** để clone và quản lý mã nguồn.
2. **Visual Studio 2022** (bản Community/Professional) đã tích hợp workload **.NET Desktop Development** (bao gồm WPF và C#).
3. **.NET SDK** tương thích với phiên bản trong project (khuyến nghị .NET 8.0 SDK).
4. **Microsoft SQL Server** (bản Developer hoặc Express, ví dụ `SQLEXPRESS` hoặc `MSSQLSERVER`) đang chạy dịch vụ (`SQL Server Services` ở trạng thái Running).
5. **SQL Server Management Studio (SSMS)** để chạy script tạo bảng và kiểm tra dữ liệu.

---

## 3. Clone repository

Mở Terminal hoặc PowerShell tại thư mục bạn muốn lưu dự án trên máy tính, sau đó chạy:

```powershell
git clone <URL_REPOSITORY>
cd CafeManagementSystem
```

*(Thay `<URL_REPOSITORY>` bằng URL GitHub của nhóm).*

Nếu repository đã có sẵn trên máy, luôn cập nhật code mới nhất trước khi làm việc bằng lệnh:

```powershell
git pull
```

---

## 4. Khởi tạo Cơ sở dữ liệu (Database Setup)

Trước khi chạy ứng dụng trên Visual Studio, cơ sở dữ liệu bắt buộc phải được tạo sẵn:

1. Mở **SQL Server Management Studio (SSMS)**.
2. Kết nối tới SQL Server trên máy cục bộ của bạn (ví dụ Server name: `.\SQLEXPRESS` hoặc `localhost` hoặc `(local)` bằng chế độ **Windows Authentication**).
3. Trong SSMS, vào **File → Open → File...** và chọn file script nằm trong thư mục project:
   ```text
   Data/CafeManagementDb.sql
   ```
4. Nhấn **Execute (F5)** để thực thi script.
5. Kiểm tra kết quả: Database `CafeManagementDb` được tạo thành công cùng 6 bảng cốt lõi (`Account`, `Category`, `Product`, `CoffeeTable`, `Orders`, `OrderDetail`) và dữ liệu mẫu (Seed Data gồm bàn, món và tài khoản thử nghiệm).

---

## 5. Mở project bằng Visual Studio 2022

1. Mở Visual Studio 2022.
2. Chọn **Open a project or solution**.
3. Điều hướng tới thư mục gốc vừa clone và chọn file solution:
   ```text
   CafeManagementSystem.sln (hoặc CafeManagementSystem.slnx)
   ```
4. Chờ Visual Studio tải solution và nạp các thành phần trong **Solution Explorer**.
5. Nhìn thanh trạng thái góc dưới bên phải kiểm tra nhánh Git đang làm việc.

> **Lưu ý:** Không mở lẻ từng file `.cs` hay `.xaml`. Hãy mở toàn bộ file Solution (`.sln` / `.slnx`) để Visual Studio quản lý đầy đủ các liên kết giữa các thư mục.

---

## 6. Cấu hình chuỗi kết nối Database (Connection String)

Ứng dụng quản lý kết nối tập trung tại một nơi duy nhất theo đúng kiến trúc.

Mở file:
```text
Data/DatabaseConnection.cs
```

Kiểm tra biến `ConnectionString` và đảm bảo tên Server khớp với máy tính cá nhân của bạn:

```csharp
private static readonly string ConnectionString =
    @"Server=.\SQLEXPRESS;Database=CafeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";
```

### Bảng cấu hình tên Server thường gặp:

| Môi trường máy cá nhân | Cấu hình Server tham khảo |
|---|---|
| Dùng bản SQL Server Express mặc định | `Server=.\SQLEXPRESS;Database=CafeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;` |
| Dùng bản SQL Server Developer / Local mặc định | `Server=localhost;Database=CafeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;` |
| Máy dùng tên máy tính cụ thể | `Server=TEN_MAY_TINH\SQLEXPRESS;Database=CafeManagementDb;Trusted_Connection=True;TrustServerCertificate=True;` |

*(Nếu SQL Server của bạn yêu cầu tài khoản `sa` thay vì Windows Authentication, bạn có thể điều chỉnh chuỗi kết nối cục bộ thành `User Id=sa;Password=your_password;`, nhưng **tuyệt đối không commit mật khẩu cá nhân lên GitHub**).*

---

## 7. Build và chạy ứng dụng

### Cách 1: Chạy trực tiếp từ Visual Studio (Khuyên dùng)
1. Trên thanh công cụ phía trên, đảm bảo cấu hình chạy là **Debug** và **Any CPU** (hoặc `x64`).
2. Nhấn nút **Start ▶ (F5)** hoặc **Start Without Debugging (Ctrl + F5)**.
3. Ứng dụng sẽ biên dịch và cửa sổ chính xuất hiện trên màn hình.

### Cách 2: Build thử bằng Terminal (Developer PowerShell / Command Prompt)
Mở PowerShell tại thư mục gốc chứa file `.sln`:

```powershell
dotnet build
```

Nếu build thành công, kết quả sẽ hiển thị:
```text
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

File thực thi `.exe` của ứng dụng sẽ nằm tại:
```text
bin/Debug/net8.0-windows/CafeManagementSystem.exe
```

---

## 8. Tài khoản đăng nhập kiểm thử

Dữ liệu mẫu (`Seed Data`) đã tạo sẵn 2 nhóm quyền để kiểm tra phân quyền:

| Tài khoản (Username) | Mật khẩu (Password) | Vai trò (Role) | Quyền hạn trong hệ thống |
|---|---|---|---|
| `manager` | `123456` | **Manager** (Quản lý) | Sử dụng toàn bộ chức năng: Bán hàng, Gọi món, Quản lý thực đơn (Thêm/Sửa/Xóa mềm/Đổi giá), Xem doanh thu, Tra cứu hóa đơn, Cài đặt |
| `staff` | `123456` | **Staff** (Thu ngân) | Bán hàng, Gọi món tại quầy, Thanh toán, Tra cứu hóa đơn, Xem thực đơn (chỉ đọc, khóa chức năng chỉnh sửa), bị chặn truy cập mục Doanh thu |

---

## 9. Những file và thư mục không cần commit thủ công (.gitignore)

Repository đã cấu hình file `.gitignore` chuẩn cho Visual Studio / .NET. Các thư mục và file sau sẽ được Git tự động bỏ qua và tự sinh lại khi build trên từng máy:

- `bin/`, `obj/`: Thư mục chứa file nhị phân, `.dll`, `.exe` sinh ra khi biên dịch.
- `.vs/`: Cấu hình môi trường cá nhân của Visual Studio.
- `*.user`, `*.suo`: Thiết lập cửa sổ, breakpoint cá nhân.
- Các file log, file tạm của Windows.

> **Hành vi bình thường:** Không copy thư mục `bin/` hay `obj/` sang máy của thành viên khác. Khi clone code mới về, Visual Studio sẽ tự động sinh lại khi bạn bấm Build.

---

## 10. Một số lỗi thường gặp và cách xử lý

### Lỗi 1: Build thất bại ngay khi vừa clone về
- Kiểm tra phiên bản .NET SDK trên máy bằng lệnh `dotnet --version`. Đảm bảo đã cài đặt .NET SDK tương ứng với project (ví dụ .NET 8.0 SDK).
- Kiểm tra xem Visual Studio đã cài workload **.NET Desktop Development** chưa (mở *Visual Studio Installer → Modify* để kiểm tra).
- Đọc lỗi đầu tiên trong cửa sổ **Error List** (`Ctrl + \ , E`).

### Lỗi 2: Không kết nối được Database (SqlException / Network-related error)
- **Dịch vụ chưa chạy:** Mở `Services` (bấm phím Windows, gõ `services.msc`), tìm dịch vụ **SQL Server (SQLEXPRESS)** hoặc **SQL Server (MSSQLSERVER)** và bấm **Start**.
- **Sai tên Server:** Kiểm tra lại thuộc tính `Server=...` trong file `Data/DatabaseConnection.cs`. Mở SSMS xem tên Server bạn dùng để đăng nhập hàng ngày là gì thì sao chép y hệt vào file C#.
- **Chưa tạo Database:** Đảm bảo đã chạy file `Data/CafeManagementDb.sql` trên SSMS để tạo database `CafeManagementDb`.
- **Lỗi chứng chỉ (Certificate):** Đảm bảo chuỗi kết nối có `TrustServerCertificate=True;`.

### Lỗi 3: Ứng dụng mở lên bị lỗi hiển thị Style / Báo thiếu Resource
- Mở file `App.xaml` và kiểm tra 2 dòng nhúng từ điển tài nguyên màu sắc và style:
  ```xml
  <ResourceDictionary Source="Resources/Colors.xaml" />
  <ResourceDictionary Source="Resources/MainStyles.xaml" />
  ```
- Đảm bảo 2 file này tồn tại đúng vị trí trong thư mục `Resources/`.

---

## 11. Quy trình làm việc nhóm qua Git

Trước khi bắt đầu code tính năng mới:
```powershell
git pull
```

Tạo branch riêng cho màn hình / tính năng đang thực hiện (không code trực tiếp trên nhánh chính):
```powershell
git checkout -b feature/ten-tinh-nang
```

Sau khi sửa và kiểm tra chạy thử (Build 0 Error):
```powershell
git status
git add .
git commit -m "Mô tả ngắn gọn công việc đã hoàn thành"
git push -u origin feature/ten-tinh-nang
```

Tạo **Pull Request (PR)** trên GitHub để trưởng nhóm review và gộp vào nhánh chính. Tuyệt đối không commit file cấu hình máy cá nhân hoặc thông tin mật khẩu nhạy cảm.

---

## Trạng thái hiện tại

Mã nguồn khung sườn (Skeleton) đã được build thử trên máy phát triển bằng Visual Studio 2022 và nhận kết quả **Build succeeded (0 Error, 0 Warning)**. 

Các thành viên clone về chỉ cần chạy script `Data/CafeManagementDb.sql` trên SSMS, chỉnh lại tên Server trong `DatabaseConnection.cs` cho khớp máy cá nhân là có thể chạy thử ngay.