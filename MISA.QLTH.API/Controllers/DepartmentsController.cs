using Microsoft.AspNetCore.Mvc;
using MISA.QLTH.BL.DepartmentBL;

[Route("/api/v1/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentBL _departmentBL;

    public DepartmentsController(IDepartmentBL departmentBL)
    {
        _departmentBL = departmentBL;
    }

    [HttpGet]
    public async Task<ActionResult> GetDepartments()
    {
        var departments = await _departmentBL.GetAllRecords();
        return Ok(departments);
    }
}