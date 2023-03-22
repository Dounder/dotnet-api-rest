using API.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Common;

[ApiController, Route("api/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GenericController : ControllerBase
{
    private readonly IUnitOfWork repository;
    private readonly IMapper mapper;

    public GenericController(IUnitOfWork repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult> Get() => Ok($"Get all");

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult> GetById(Guid id)
    {
        return new OkObjectResult($"Get {id}");
    }

    [HttpPost]
    public virtual async Task<ActionResult> Post()
    {
        try
        {
            return new OkObjectResult($"Create");
        }
        catch (Exception e)
        {
            return HandleExceptions.OnError(e);
        }
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult> Put(Guid id)
    {
        try
        {
            return new OkObjectResult($"Update {id}");
        }
        catch (Exception e)
        {
            return HandleExceptions.OnError(e);
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        return Ok(new { message = "Deleted" });
    }
}