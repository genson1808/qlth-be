using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.QLTH.Common.Entities
{
    /// <summary>
    /// Bảng tổ chuyên môn
    /// </summary>
    [Table("department")]
    public class Department : BaseEntity
    {
        /// <summary>
        /// ID tổ chuyên môn
        /// </summary>
        [Key]
        public Guid? DepartmentID{ get; set; }

        /// <summary>
        /// Mã tổ chuyên môn
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên tổ chuyên môn
        /// </summary>
        public string DepartmentName{ get; set; }
    }
}