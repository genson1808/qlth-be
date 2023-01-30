using FluentValidation;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.Common.Entities.DTO;
using QLTH.BL.EmployeeBL;

namespace QLTH.BL.Validations;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployee>
{
    private readonly IEmployeeBL _employeeBL;

    public CreateEmployeeValidator(IEmployeeBL employeeBL)
    {
        _employeeBL = employeeBL;

        RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Mã cán bộ, giáo viên là bắt buộc.");
        RuleFor(x => x.EmployeeName).NotEmpty().WithMessage("Tên cán bộ, giáo viên là bắt buộc.");
        RuleFor(x => x.Email).EmailAddress()
            .When(x => x.Email != null && !x.Email.Equals(""))
            .WithMessage("Email không hợp lệ.");
    }
}