namespace MISA.QLTH.Common.Entities.DTO
{
    /// <summary>
    /// Dữ liệu phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng muốn lấy</typeparam>
    /// AUTHOR: SONTB (05/10/2022)
    public class PagingData<T> {
        /// <summary>
        /// Danh sách dữ liệu muốn lấy
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Tổng các bản ghi có trong database
        /// </summary>
        public long Total {get; set; }

        /// <summary>
        /// Page hiện tại
        /// </summary>
        public int PageNumber {get; set; }

        /// <summary>
        /// Số lượng record phân trang
        /// </summary>
        public int PageSize {get; set; }
    }
}