using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTH.Common.Entities
{
    /// <summary>
    /// Bảng môn học
    /// </summary>
    [Table("Subject")]
    public class Subject : BaseEntity
    {
        /// <summary>
        /// ID môn học
        /// </summary>
        [Key]
        public Guid SubjectID{ get; set; }

        /// <summary>
        /// Mã môn học
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Tên môn học
        /// </summary>
        public string SubjectName { get; set; }

        public Guid EmployeeID { get; set; }
    }
}