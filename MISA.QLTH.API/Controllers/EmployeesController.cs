using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTH.API.Models;
using MISA.QLTH.BL.EmployeeBL;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.Common.Entities.DTO;
using MISA.QLTH.Common.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace MISA.QLTH.API.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeBL _employeeBL;

    public EmployeesController(IEmployeeBL employeeBl)
    {
        _employeeBL = employeeBl;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployee request)
    {
        var result = await _employeeBL.CreateEmployee(request);

        return StatusCode((int)HttpStatusCode.Created, result.ToString());
    }

    [HttpGet]
    [Route("{EmployeeID}")]
    public async Task<IActionResult> GetEmployeeById(Guid EmployeeID)
    {
        var isExisted = await _employeeBL.CheckExistedByID(EmployeeID);
        if (!isExisted)
        {
            throw new NotFoundException("Cán bộ, giáo viên không tồn tại.");
        }

        var result = await _employeeBL.GetEmployeeByID(EmployeeID);

        return StatusCode((int)HttpStatusCode.OK, result);
    }
    
    
    [HttpGet("code")]
    public async Task<IActionResult> GetNextCode()
    {
        var result = await _employeeBL.GetNextEmployeeCode();

        return StatusCode((int)HttpStatusCode.OK, result);
    }

    [HttpPost("paging")]
    public async Task<IActionResult> GetEmployees([FromQuery] int pageNumber, [FromQuery] int pageSize,
        [FromBody] FilterRequest filterRequest)
    {
        var result =
            await _employeeBL.FilterEmployeeFilterEmployee(pageNumber, pageSize, filterRequest.Filters,
                filterRequest.Sorts);
        return StatusCode((int)HttpStatusCode.OK, result);
    }

    [HttpPut]
    [Route("{EmployeeID}")]
    public async Task<IActionResult> UpdateEmployee(Guid EmployeeID, [FromBody] Employee request,
        [FromServices] IValidator<Employee> validator)
    {
        ValidationResult validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var isDuplicated = await _employeeBL.CheckDuplicate(EmployeeID, request.Email, request.EmployeeCode);
        if (isDuplicated)
        {
            throw new DuplicateException("Thông tin email hoặc mã cán bộ giáo viên đã tồn tại.");
        }

        var result = await _employeeBL.UpdateEmployee(EmployeeID, request);

        return StatusCode((int)HttpStatusCode.OK, result.ToString());
    }

    [HttpDelete]
    [Route("{EmployeeID}")]
    public async Task<IActionResult> DeleteEmployee(Guid EmployeeID)
    {
        var isExist = await _employeeBL.GetEmployeeByID(EmployeeID);
        if (isExist == null)
        {
            throw new NotFoundException("Cán bộ, giáo viên không tồn tại.");
        }

        var deleted = await _employeeBL.DeleteRecordByID(EmployeeID);
        return Ok(deleted.ToString());
    }

    [HttpDelete("multiple")]
    public async Task<IActionResult> DeleteEmployees([FromBody] List<Guid> recordIDList)
    {
        var numberOfAffected = await _employeeBL.DeleteMultipleRecord(recordIDList);
        if (numberOfAffected <= 0)
        {
            throw new BadRequestException("Yêu cầu tồi tệ .");
        }

        return Ok(numberOfAffected.ToString());
    }
}