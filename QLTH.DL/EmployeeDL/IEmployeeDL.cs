using QLTH.Common.Entities;
using QLTH.Common.Entities.DTO;
using QLTH.DL.BaseDL;

namespace QLTH.DL.EmployeeDL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Chỉnh sửa cán bộ, giáo viên
        /// </summary>
        /// <param name="employeeID">ID cán bộ, giáo viên</param>
        /// <param name="employee">Dữ liệu cán bộ, giáo viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public Task<int> UpdateEmployee(Guid employeeID, Employee employee);
        
        /// <summary>
        /// Thêm cán bộ, giáo viên
        /// </summary>
        /// <param name="employee">Dữ liệu cán bộ, giáo viên</param>
        /// <returns>ID</returns>
        /// AUTHOR: SONTB (10/10/2022)
        public Task<Guid> CreateEmployee(CreateEmployee employee);

        /// <summary>
        /// Lấy mã cán bộ, giáo viên tự động
        /// </summary>
        /// <returns>Mã cán bộ, giáo viên tự sinh</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public Task<string> GetNextEmployeeCode();

        /// <summary>
        /// Lấy dữ liệu một cán bộ, giáo viên
        /// </summary>
        /// <param name="id">ID cán bộ, giáo viên</param>
        /// <returns>Dữ liệu cán bộ, giáo viên</returns>
        /// AUTHOR: SONTB (05/10/2022)
        public Task<Employee> GetEmployeeByID(Guid id);

        /// <summary>
        /// Lấy danh sách cán bộ, giáo viên theo filter nhiều field và sắp xếp theo nhiều field + phân trang
        /// </summary>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="pageSize">Số bản ghi muốn lấy mỗi trang</param>
        /// <param name="filters">Dictionary chứa thông tin muốn lọc vd: {"EmployeeName": "Võ Minh Anh", ......} </param>
        /// <param name="sorts">Dictionary chứa thông tin muốn sắp xếp vd: {"EmployeeName": "DESC", "CreatedDate": "ASC", .......}</param>
        /// <returns></returns>
        /// AUTHOR: SONTB (06/10/2022)
        public Task<PagingData<Employee>> FilterEmployeeFilterEmployee(
            int pageNumber,
            int pageSize,
            Dictionary<string, string> filters,
            Dictionary<string, string> sorts);
        
        
        /// <summary>
        /// Kiểm tra các trường dữ liệu unique có bị dupliadte hay không
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="email"></param>
        /// <param name="employeeCode"></param>
        /// AUTHOR: SONTB (10/10/2022)
        public Task<bool> CheckDuplicate(Guid employeeID, string email, string employeeCode);
        
        /// <summary>
        /// Kiểm tra các trường email có bị dupliadte hay không
        /// </summary>
        /// <param name="email"></param>
        /// AUTHOR: SONTB (10/10/2022)
        public Task<bool> CheckDuplicateEmail(string email);
        
        
        /// <summary>
        /// Kiểm tra cán bộ, giáo viên có tồn tại không.
        /// </summary>
        /// <param name="employeeID"></param>
        /// AUTHOR: SONTB (06/10/2022)
        public Task<bool> CheckExistedByID(Guid employeeID);
    }
}