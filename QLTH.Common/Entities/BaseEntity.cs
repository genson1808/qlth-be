namespace QLTH.Common.Entities
{
    /// <summary>
    /// Entity cơ sở cho các enities
    /// </summary>
    public class BaseEntity {
        /// <summary>
        /// Thời gin tạo
        /// </summary>
        public DateTime? CreatedDate{ get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate{ get; set; }
        /// <summary>
        /// Người chỉnh sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}