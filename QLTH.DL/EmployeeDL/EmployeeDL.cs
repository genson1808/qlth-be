using Dapper;
using QLTH.Common.Entities;
using QLTH.Common.Entities.DTO;
using QLTH.Common.Utils;
using MySqlConnector;
using QLTH.DL.BaseDL;

namespace QLTH.DL.EmployeeDL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        public async Task<Guid> CreateEmployee(CreateEmployee employee)
        {
            var storedProcedureName = "Proc_Employee_Create";

            string localDate = DateTime.UtcNow.ToString("ddTHH-MM-yyyy\\:mm\\:ss.fffffffzzz");
            employee.ModifiedDate = DateTime.Now;
            employee.CreatedDate = DateTime.Now;
            employee.EmployeeID = Guid.NewGuid();

            var parameters = new DynamicParameters();

            string subjects;
            string insertSubjects = "";
            if (employee.Subjects != null && employee.Subjects.Count > 0)
            {
                subjects = Utils.BuildValuesFromGuidList(employee.Subjects, employee.EmployeeID);
                insertSubjects = $"INSERT SubjectEmployee (SubjectID, EmployeeID) VALUES {subjects};";
            }

            string rooms;
            string insertRooms = "";
            if (employee.Rooms != null && employee.Rooms.Count > 0)
            {
                rooms = Utils.BuildValuesFromGuidList(employee.Rooms, employee.EmployeeID);
                insertRooms = $"INSERT RoomEmployee (RoomID, EmployeeID) VALUES {rooms};";
            }

            parameters.Add("v_EmpolyeeID", employee.EmployeeID);
            parameters.Add("v_EmployeeCode", employee.EmployeeCode);
            parameters.Add("v_EmployeeName", employee.EmployeeName);
            parameters.Add("v_PhoneNumber", employee.PhoneNumber);
            parameters.Add("v_Email", employee.Email);
            parameters.Add("v_DepartmentID", employee.DepartmentID);
            parameters.Add("v_IsEquipmentManagement", employee.IsEquipmentManagement);
            parameters.Add("v_IsWorking", employee.IsWorking);
            parameters.Add("v_DayOfResignation", employee.DayOfResignation);
            parameters.Add("v_CreatedDate", employee.CreatedDate);
            parameters.Add("v_CreatedBy", employee.CreatedBy);
            parameters.Add("v_ModifiedDate", employee.ModifiedDate);
            parameters.Add("v_ModifiedBy", employee.ModifiedBy);

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var numberOfAffectedRow = 0;
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    numberOfAffectedRow = await sqlConnection.ExecuteAsync(storedProcedureName, parameters,
                        commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                    if (employee.Subjects != null && employee.Subjects.Count > 0)
                    {
                        await sqlConnection.ExecuteAsync(insertSubjects, transaction: transaction);
                    }

                    if (employee.Rooms != null && employee.Rooms.Count > 0)
                    {
                        await sqlConnection.ExecuteAsync(insertRooms, transaction: transaction);
                    }

                    transaction.Commit();
                }

                if (numberOfAffectedRow > 0)
                {
                    return employee.EmployeeID;
                }

                return Guid.Empty;
            }
        }

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
                // Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
                var number = Int64.Parse(lastCode.Substring(2)) + 1;
                var newCode = $"NV{number}";
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
                var isDuplicated = await sqlConnection.QueryFirstOrDefaultAsync<bool>(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return isDuplicated;
            }
        }

        public async Task<bool> CheckDuplicateEmail(string email)
        {
            var storedProcedureName = "Proc_Employee_CheckEmailExisted";
            var parameters = new DynamicParameters();
            parameters.Add("v_Email", email);

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var isExisted = await sqlConnection.QueryFirstOrDefaultAsync<bool>(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return isExisted;
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