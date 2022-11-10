using MISA.QLTH.BL.BaseBL;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.DL.BaseDL;
using MISA.QLTH.DL.DepartmentDL;

namespace MISA.QLTH.BL.DepartmentBL;

public class DepartmentBL : BaseBL<Department>, IDepartmentBL
{
    private readonly IDepartmentDL _departmentDL;
    public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
    {
        _departmentDL = departmentDL;
    }
}