using MISA.QLTH.Common.Entities;
using QLTH.BL.BaseBL;
using QLTH.DL.SubjectDL;

namespace QLTH.BL.SubjectBL;

public class SubjectBL : BaseBL<Subject>, ISubjectBL
{
    private readonly ISubjectDL _subjectDL;
    public SubjectBL(ISubjectDL subjectDL) : base(subjectDL)
    {
        _subjectDL = subjectDL;
    }
}