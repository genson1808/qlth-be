using MISA.QLTH.BL.BaseBL;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.DL.SubjectDL;

namespace MISA.QLTH.BL.SubjectBL;

public class SubjectBL : BaseBL<Subject>, ISubjectBL
{
    private readonly ISubjectDL _subjectDL;
    public SubjectBL(ISubjectDL subjectDL) : base(subjectDL)
    {
        _subjectDL = subjectDL;
    }
}