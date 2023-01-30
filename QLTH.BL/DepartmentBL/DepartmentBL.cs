using MISA.QLTH.Common.Entities;
using QLTH.BL.BaseBL;
using QLTH.DL.BaseDL;
using QLTH.DL.DepartmentDL;

namespace QLTH.BL.DepartmentBL;

public class DepartmentBL : BaseBL<Department>, IDepartmentBL
{
    private readonly IDepartmentDL _departmentDL;
    public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
    {
        _departmentDL = departmentDL;
    }
}