using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QLTH.Common.Entities

{
    /// <summary>
    /// Bảng cán bộ, giáo viên
    /// </summary>
    public class Employee : BaseEntity
    {

        /// <summary>
        /// ID cán bộ, giáo viên
        /// </summary>
        [Key]
        public Guid EmployeeID{ get; set; }

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
        public String? Email { get; set; }
        
        public Guid DepartmentID { get; set; }

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


        public Department? Department { get; set; }

        public IEnumerable<Subject>? Subjects { get; set; }

        public IEnumerable<Room>? Rooms { get; set; }

        public String? DepartmentName { get; set; }
    }
}