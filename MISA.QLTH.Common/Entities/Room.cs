using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.QLTH.Common.Entities
{
    /// <summary>
    /// Bảng kho/phòng
    /// </summary>
    [Table("room")]
    public class Room : BaseEntity
    {
        /// <summary>
        /// ID kho/phòng
        /// </summary>
        [Key]
        public Guid RoomID { get; set; }

        /// <summary>
        /// Mã kho/phòng
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// Tên kho/phòng
        /// </summary>
        public string RoomName { get; set; }

        public Guid EmployeeID { get; set; }
    }
}