using Dapper;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.Common.Entities.DTO;
using MISA.QLTH.Common.Utils;
using MISA.QLTH.DL.BaseDL;
using MySqlConnector;

namespace MISA.QLTH.DL.EmployeeDL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Lấy mã cán bộ, giáo viên tự động
        /// </summary>
        /// <returns>Mã cán bộ, giáo viên tự sinh</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public async Task<string> GetNextEmployeeCode()
        {
            var storedProcedureName = "Proc_Employee_GetLastCode";

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var lastCode = await sqlConnection.QueryFirstOrDefaultAsync<string>(
                    storedProcedureName,
                    commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã nhân viên tự động tăng
                // Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                // Mã nhân viên mới = "MS" + Giá trị cắt chuỗi ở  trên + 1
                var newCode = "MS" + (Int64.Parse(lastCode.Substring(2) + 1)).ToString();
                return newCode;
            }
        }

        /// <summary>
        /// Chỉnh sửa cán bộ, giáo viên
        /// </summary>
        /// <param name="employeeID">ID cán bộ, giáo viên</param>
        /// <param name="employee">Dữ liệu cán bộ, giáo viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public async Task<int> UpdateEmployee(Guid employeeID, Employee employee)
        {
            var storedProcedureName = "Proc_Employee_UpdateEmployee";
            var parameters = Utils.EntityAllDynamicParams(employee);

            string localDate = DateTime.UtcNow.ToString("ddTHH-MM-yyyy\\:mm\\:ss.fffffffzzz");
            employee.ModifiedDate = DateTime.Now;

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var numberOfAffectedRow = await sqlConnection.ExecuteAsync(storedProcedureName, parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return numberOfAffectedRow;
            }
        }

        /// <summary>
        /// Lấy dữ liệu một cán bộ, giáo viên
        /// </summary>
        /// <param name="id">ID cán bộ, giáo viên</param>
        /// <returns>Dữ liệu cán bộ, giáo viên</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public async Task<Employee> GetEmployeeByID(Guid id)
        {
            var storedProcedureName = "Proc_Employee_GetByID";
            var parameters = new DynamicParameters();
            parameters.Add("v_EmployeeID", id);

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                using (var mutil = await sqlConnection.QueryMultipleAsync(
                           storedProcedureName,
                           parameters,
                           commandType: System.Data.CommandType.StoredProcedure))
                {
                    var subject = mutil.Read<Subject>().ToList();
                    var room = mutil.Read<Room>().ToList();
                    var employee = mutil.Read<Employee>().FirstOrDefault();
                    employee.Subjects = subject;
                    employee.Rooms = room;

                    return employee;
                }
            }
        }

        /// <summary>
        /// Lấy danh sách cán bộ, giáo viên theo filter nhiều field và sắp xếp theo nhiều field + phân trang
        /// </summary>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="pageSize">Số bản ghi muốn lấy mỗi trang</param>
        /// <param name="filters">Dictionary chứa thông tin muốn lọc vd: {"EmployeeName": "Võ Minh Anh", ......} </param>
        /// <param name="sorts">Dictionary chứa thông tin muốn sắp xếp vd: {"EmployeeName": "DESC", "CreatedDate": "ASC", .......}</param>
        /// <returns></returns>
        /// AUTHOR: SONTB (06/10/2022)
        public async Task<PagingData<Employee>> FilterEmployeeFilterEmployee(
            int pageNumber,
            int pageSize,
            Dictionary<string, string> filters,
            Dictionary<string, string> sorts)
        {
            var orderClause = Utils.MappingOrder(sorts, "e");
            var whereClause = Utils.MappingWhere(filters, "e");
            var parameters = new DynamicParameters();
            var offset = (pageNumber - 1) * pageSize;
            parameters.Add("v_OffsetData", offset);
            parameters.Add("v_LimitData", pageSize);
            parameters.Add("v_WhereClause", whereClause);
            parameters.Add("v_OrderClause", orderClause);

            const string storedProcedureName = "Proc_Employee_GetPaging";

            // Khai báo data paging dành cho việc return
            var result = new PagingData<Employee>();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            await using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                using (var mutil = await sqlConnection.QueryMultipleAsync(
                           storedProcedureName,
                           parameters,
                           commandType: System.Data.CommandType.StoredProcedure))
                {
                    // Lấy danh sách tất cả cán bộ, giáo viên
                    var empl = (await mutil.ReadAsync<Employee>()).ToList();

                    // lấy dánh sách tất cả môn học
                    var subjects = (await mutil.ReadAsync<Subject>()).ToList();
                    // Mapping dữ liệu môn phù hợp với cán bộ, giáo viên
                    empl.ForEach(e => e.Subjects = subjects.Where(s => s.EmployeeID.Equals(e.EmployeeID)).ToList());

                    // Lấy danh sách tất cả kho/phòng
                    var rooms = (await mutil.ReadAsync<Room>()).ToList();
                    // Mapping dữ liệu kho/phòng phù hợp với cán bộ, giáo viên
                    empl.ForEach(e => e.Rooms = rooms.Where(r => r.EmployeeID.Equals(e.EmployeeID)).ToList());

                    // Gán data danh sách cán bộ, giáo viên vào data paging
                    result.Data = empl;

                    // Lấy tổng số cán bộ, giáo viên có trong database
                    result.Total = await mutil.ReadFirstAsync<long>();

                    return result;
                }
            }
        }

        /// <summary>
        /// Kiểm tra các trường dữ liệu unique có bị dupliadte hay không
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="email"></param>
        /// <param name="employeeCode"></param>
        /// AUTHOR: SONTB (06/10/2022)
        public async Task<bool> CheckDuplicate(Guid employeeID, string email, string employeeCode)
        {
            var storedProcedureName = "Proc_Employee_CheckDuplicate";
            var parameters = new DynamicParameters();
            parameters.Add("v_EmployeeCode", employeeCode);
            parameters.Add("v_Email", email);
            parameters.Add("v_EmployeeID", employeeID);
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var isDuplicated = await sqlConnection.QueryFirstOrDefaultAsync<Employee>(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return isDuplicated != null;
            }
        }

        /// <summary>
        /// Kiểm tra cán bộ, giáo viên có tồn tại không.
        /// </summary>
        /// <param name="employeeID"></param>
        /// AUTHOR: SONTB (06/10/2022)
        public async Task<bool> CheckExistedByID(Guid employeeID)
        {
            var storedProcedureName = "Proc_Employee_CheckExistByID";
            var parameters = new DynamicParameters();
            parameters.Add("v_EmployeeID", employeeID);

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var isExisted = await sqlConnection.QueryFirstOrDefaultAsync<Employee>(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return isExisted != null;
            }
        }
    }
}