using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTH.BL.SubjectBL;

namespace QLTH.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectBL _subjectBL;

    public SubjectsController(ISubjectBL subjectBl)
    {
        _subjectBL = subjectBl;
    }

    [HttpGet]
    public async Task<ActionResult> GetSubjects()
    {
        var subjects = await _subjectBL.GetAllRecords();
        return Ok(subjects);
    }
}