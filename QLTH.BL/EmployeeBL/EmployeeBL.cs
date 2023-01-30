using MISA.QLTH.Common.Entities;
using MISA.QLTH.Common.Entities.DTO;
using QLTH.BL.BaseBL;
using QLTH.DL.EmployeeDL;

namespace QLTH.BL.EmployeeBL;

public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
{
    private readonly IEmployeeDL _employeeDL;

    public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
    {
        _employeeDL = employeeDL;
    }

    /// <summary>
    /// Chỉnh sửa cán bộ, giáo viên
    /// </summary>
    /// <param name="employeeID">ID cán bộ, giáo viên</param>
    /// <param name="employee">Dữ liệu cán bộ, giáo viên</param>
    /// <returns>Số bản ghi bị ảnh hưởng</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public async Task<int> UpdateEmployee(Guid employeeID, Employee employee)
    {
        return await _employeeDL.UpdateEmployee(employeeID, employee);
    }

    /// <summary>
    /// Lấy mã cán bộ, giáo viên tự động
    /// </summary>
    /// <returns>Mã cán bộ, giáo viên tự sinh</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public async Task<string> GetNextEmployeeCode()
    {
        return await _employeeDL.GetNextEmployeeCode();
    }

    /// <summary>
    /// Lấy dữ liệu một cán bộ, giáo viên
    /// </summary>
    /// <param name="id">ID cán bộ, giáo viên</param>
    /// <returns>Dữ liệu cán bộ, giáo viên</returns>
    /// AUTHOR: SONTB (06/10/2022)
    public async Task<Employee> GetEmployeeByID(Guid id)
    {
        return await _employeeDL.GetEmployeeByID(id);
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
    public async Task<PagingData<Employee>> FilterEmployeeFilterEmployee(int pageNumber, int pageSize,
        Dictionary<string, string> filters, Dictionary<string, string> sorts)
    {
        return await _employeeDL.FilterEmployeeFilterEmployee(pageNumber, pageSize, filters, sorts);
    }

    // <summary>
    /// Kiểm tra các trường dữ liệu unique có bị dupliadte hay không
    /// </summary>
    /// <param name="employeeID"></param>
    /// <param name="email"></param>
    /// <param name="employeeCode"></param>
    /// AUTHOR: SONTB (06/10/2022)
    public async Task<bool> CheckDuplicate(Guid employeeID, string email, string employeeCode)
    {
        return await _employeeDL.CheckDuplicate(employeeID, email, employeeCode);
    }

    /// <summary>
    /// Kiểm tra cán bộ, giáo viên có tồn tại không.
    /// </summary>
    /// <param name="employeeID"></param>
    /// AUTHOR: SONTB (06/10/2022)
    public async Task<bool> CheckExistedByID(Guid employeeID)
    {
        return await _employeeDL.CheckExistedByID(employeeID);
    }

    public async Task<Guid> CreateEmployee(CreateEmployee employee)
    {
        return await _employeeDL.CreateEmployee(employee);
    }
}