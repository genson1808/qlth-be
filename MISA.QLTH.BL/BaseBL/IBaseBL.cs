namespace MISA.QLTH.BL.BaseBL;

public interface IBaseBL<T>
{
    /// <summary>
    /// Lấy thông tin của tất cả bản ghi
    /// </summary>
    /// <returns>Danh sách dữ liệu bản ghi</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public Task<IEnumerable<dynamic>> GetAllRecords();

    /// <summary>
    /// Thêm mới một bản ghi
    /// </summary>
    /// <param name="record">Dữ liệu bản ghi</param>
    /// <returns>ID bản ghi vừa thêm</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public Task<Guid> InsertRecord(T record);

    /// <summary>
    /// Lấy thông tin một bản ghi
    /// </summary>
    /// <param name="id">ID của bản ghi muốn lấy</param>
    /// <returns>Dữ liệu của bản ghi</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public Task<T> GetRecordByID(Guid id);

    /// <summary>
    /// Xoá một bản ghi
    /// </summary>
    /// <param name="id">ID của bản ghi muốn xoá</param>
    /// <returns>Số bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public Task<int> DeleteRecordByID(Guid id);

    /// <summary>
    /// Kiểm tra mã bản ghi có bị trùng lặp hay không
    /// </summary>
    /// <param name="code">Code của bản ghi muốn lấy</param>
    /// <returns>true/false</returns>
    /// AUTHOR: SONTB (10/10/2022)
    public Task<bool> CheckDuplicateCode(string code);


    /// <summary>
    /// Xoá nhiều bản ghi
    /// </summary>
    /// <param name="idRecordList">Danh sách ID của các bản ghi muốn xoá</param>
    /// <returns>Số bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public Task<int> DeleteMultipleRecord(List<Guid> idRecordList);
}