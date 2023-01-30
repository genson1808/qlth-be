using QLTH.DL.BaseDL;

namespace QLTH.BL.BaseBL;

public class BaseBL<T> : IBaseBL<T>
{
    private readonly IBaseDL<T> _baseDL;

    public BaseBL(IBaseDL<T> baseDL)
    {
        _baseDL = baseDL;
    }

    /// <summary>
    /// Lấy thông tin của tất cả bản ghi
    /// </summary>
    /// <returns>Danh sách dữ liệu bản ghi</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public virtual async Task<IEnumerable<dynamic>> GetAllRecords()
    {
        return await _baseDL.GetAllRecords();
    }

    /// <summary>
    /// Thêm mới một bản ghi
    /// </summary>
    /// <param name="record">Dữ liệu bản ghi</param>
    /// <returns>ID bản ghi vừa thêm</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public virtual async Task<Guid> InsertRecord(T record)
    {
        return await _baseDL.InsertRecord(record);
    }

    /// <summary>
    /// Lấy thông tin một bản ghi
    /// </summary>
    /// <param name="id">ID của bản ghi muốn lấy</param>
    /// <returns>Dữ liệu của bản ghi</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public virtual async Task<T> GetRecordByID(Guid id)
    {
        return await _baseDL.GetRecordByID(id);
    }

    /// <summary>
    /// Xoá một bản ghi
    /// </summary>
    /// <param name="id">ID của bản ghi muốn xoá</param>
    /// <returns>Số bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public virtual async Task<int> DeleteRecordByID(Guid id)
    {
        return await _baseDL.DeleteRecordByID(id);
    }

    /// <summary>
    /// Xoá nhiều bản ghi
    /// </summary>
    /// <param name="idRecordList">Danh sách ID của các bản ghi muốn xoá</param>
    /// <returns>Số bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public virtual async Task<int> DeleteMultipleRecord(List<Guid> idRecordList)
    {
        return await _baseDL.DeleteMultipleRecord(idRecordList);
    }

    public async Task<bool> CheckDuplicateCode(string code)
    {
        return await _baseDL.CheckDuplicateCode(code);
    }
}