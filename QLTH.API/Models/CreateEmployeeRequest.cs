namespace QLTH.API.Models;

public class CreateEmployeeRequest
{
    /// <summary>
    /// Mã cán bộ, giáo viên
    /// </summary>
    public string EmployeeCode{ get; set; }

    /// <summary>
    /// Tên cán bộ, giáo viên
    /// </summary>
    public string EmployeeName{ get; set; }

    /// <summary>
    /// Số điện thoại cán bộ, giáo viên
    /// </summary>
    public string? PhoneNumber{ get; set; }

    /// <summary>
    /// Email cán bộ, giáo viên
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Cán bộ, giáo viên là đào tạo quản lý thiết bị
    /// </summary>
    public bool IsEquipmentManagement { get; set; }

    /// <summary>
    /// Đang làm việc
    /// </summary>
    public bool IsWorking { get; set; }

    /// <summary>
    /// Ngày nghỉ việc
    /// </summary>
    public DateTime? DayOfResignation { get; set; }

    public Guid DepartmentID { get; set; }
    
    public List<Guid> Subjects { get; set; }

    public List<Guid> Rooms { get; set; }
}