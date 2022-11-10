using FluentValidation;
using MISA.QLTH.Common.Entities;

namespace MISA.QLTH.BL.Validations;

public class CreateEmployeeValidator : AbstractValidator<Employee>
{
   public CreateEmployeeValidator()
   {
      RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Mã cán bộ, giáo viên là bắt buộc.");
      RuleFor(x => x.EmployeeName).NotEmpty().WithMessage("Tên cán bộ, giáo viên là bắt buộc.");
      RuleFor(x => x.Email).Empty().EmailAddress().WithMessage("Email không hợp lệ.");
   }
}