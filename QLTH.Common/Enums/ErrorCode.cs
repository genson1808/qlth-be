namespace QLTH.Common.Enums
{
        public enum ErrorCode
    {
        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Exception = 101,

        /// <summary>
        /// Lỗi khi truy vấn database
        /// </summary>
        Uneffected = 102,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        DuplicateCode = 103,

        /// <summary>
        /// Lỗi không tìm thấy
        /// </summary>
        NotFound = 104,
        
        /// <summary>
        /// Lỗi validate dữ liệu
        /// </summary>
        Validate = 4
    }
}