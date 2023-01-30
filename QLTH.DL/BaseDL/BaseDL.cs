using System.ComponentModel.DataAnnotations;
using Dapper;
using QLTH.Common.Utils;
using MySqlConnector;

namespace QLTH.DL.BaseDL;

public class BaseDL<T> : IBaseDL<T>
{
    /// <summary>
    /// Lấy thông tin của tất cả bản ghi
    /// </summary>
    /// <returns>Dữ liệu của tất cả bản ghi</returns>
    /// AUTHOR: SONTB (05/10/2022)
    public async Task<IEnumerable<dynamic>> GetAllRecords()
    {
        // Chuẩn bị câu lệnh SELECT
        string classname = typeof(T).Name;
        string getAllRecordsCommand = $"SELECT * FROM {classname} ORDER BY ModifiedDate DESC";
        using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
        {
            // Thực hiện truy vấn SELECT vào DB
            var records = await mysqlConnection.QueryAsync(getAllRecordsCommand);

            return records;
        }
    }

    /// <summary>
    /// Thêm mới một bản ghi
    /// </summary>
    /// <param name="record">Thông tin bản ghi</param>
    /// <returns>ID của bản ghi vừa thêm</returns>
    /// AUTHOR: SONTB (05/10/2022)
    public async Task<Guid> InsertRecord(T record)
    {
        string className = typeof(T).Name;
        string insertStoreProcedureName = $"Proc_{className}_Insert{className}";
        string localDate = DateTime.UtcNow.ToString("ddTHH-MM-yyyy\\:mm\\:ss.fffffffzzz");
        record.GetType().GetProperty($"{className}ID").SetValue(record, Guid.NewGuid(), null);
        var nơ = DateTime.Now;
        record.GetType().GetProperty("CreatedDate").SetValue(record, DateTime.Now, null);
        record.GetType().GetProperty("ModifiedDate").SetValue(record, DateTime.Now, null);

        // Chuẩn bị tham số đầu vào stored procedure
        var parameters = Utils.EntityAllDynamicParams(record);

        // Thực hiện câu lệnh stored procedure với tham số ở trên
        using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
        {
            var numberOfAffectedRow = await mysqlConnection
                .ExecuteAsync(
                    insertStoreProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure
                );

            var result = Guid.Empty;
            if (numberOfAffectedRow > 0)
            {
                var primaryKeyProperty = typeof(T).GetProperties()
                    .FirstOrDefault(
                        prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0
                    );

                var newId = primaryKeyProperty?.GetValue(record);
                if (newId != null)
                {
                    result = (Guid)newId;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Lấy thông tin một bản ghi theo ID
    /// </summary>
    /// <param name="id">ID của bản ghi</param>
    /// <returns>Dữ liệu bản ghi</returns>
    ///  AUTHOR: SONTB (05/10/2022)
    public async Task<T> GetRecordByID(Guid id)
    {
        string className = typeof(T).Name;
        // Chuẩn bị tên Stored procedure
        string storedProcedureName = $"Proc_${className}_Get{className}ByID";

        // Chuẩn bị tham số cho stored procedure
        var parameters = new DynamicParameters();
        parameters.Add($"v_{className}ID", id);

        using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
        {
            // Thực hiện gọi stored procedure vào Db với tham số ở trên
            var result = await mysqlConnection.QueryFirstOrDefault(
                storedProcedureName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            return result;
        }
    }

    /// <summary>
    /// Xoá một bản ghi theo ID
    /// </summary>
    /// <param name="id">ID bản ghi muốn xoá</param>
    /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (05/10/2022)
    public async Task<int> DeleteRecordByID(Guid id)
    {
        string className = typeof(T).Name;
        // Chuẩn bị tên Stored  Procedure
        string storedProcedureName = $"Proc_{className}_Delete{className}ByID";

        // Chuẩn bị tham số cho stored procedure
        var parameters = new DynamicParameters();
        parameters.Add($"v_{className}ID", id);

        using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
        {
            // Thực hiện gọi stored procedure vào Db với tham số ở trên
            int numberOfAffectedRow = await mysqlConnection.ExecuteAsync(
                storedProcedureName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            return numberOfAffectedRow;
        }
    }

    /// <summary>
    /// Xoá nhiều bản ghi
    /// </summary>
    /// <param name="idRecordList">Danh sách ID của các bản ghi muốn xoá</param>
    /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (05/10/20
    public async Task<int> DeleteMultipleRecord(List<Guid> idRecordList)
    {
        // Biến lưu số lượng bản ghi bị xoá
        var numberOfAffectedRow = 0;

        await Parallel.ForEachAsync(idRecordList, async (idRecord, _) =>
        {
            // Khai báo stored procedure
            string className = typeof(T).Name;
            string storedProcedureName = $"Proc_{className}_Delete{className}ByID";

            using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mysqlConnection.Open();
                var transaction = mysqlConnection.BeginTransaction();

                // Chuẩn bị tham số cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add($"v_{className}ID", idRecord);
                var numberDeleted = await mysqlConnection.ExecuteAsync(
                    storedProcedureName,
                    parameters,
                    transaction: transaction,
                    commandType: System.Data.CommandType.StoredProcedure);

                transaction.Commit();
                numberOfAffectedRow += numberDeleted;
            }
        });
        return numberOfAffectedRow;
    }

    public async Task<bool> CheckDuplicateCode(string code)
    {
        string className = typeof(T).Name;
        // Chuẩn bị tên Stored procedure
        string storedProcedureName = $"Proc_{className}_CheckCodeExisted";

        // Chuẩn bị tham số cho stored procedure
        var parameters = new DynamicParameters();
        parameters.Add($"v_{className}Code", code);

        using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
        {
            // Thực hiện gọi stored procedure vào Db với tham số ở trên
            var result = await mysqlConnection.QueryFirstOrDefault(
                storedProcedureName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);

            if(result != null) {
                return true; 
	        }
        }

        return false;
    }
}