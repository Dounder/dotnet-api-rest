using API.UnitOfWork;
using AutoMapper;
using Core.Interfaces.Common;
using Infrastructure.Services.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Common;

[ApiController, Route("api/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class ApiController<T, TID, TLIST, TCREATE, TUPDATE> : ControllerBase where T : class, IBaseEntity
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unit;
    private readonly IRepository<T> repository;

    protected ApiController(IMapper mapper, IUnitOfWork unit)
    {
        this.mapper = mapper;
        this.unit = unit;
        repository = new Repository<T>(unit.DbContext, mapper);
    }

    [HttpGet]
    public virtual async Task<ActionResult> Get()
    {
        return Ok(await repository.Find<TLIST>());
    }


    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult> GetById(Guid id)
    {
        var data = await repository.FindOne<TID>(id);

        if (data is null)
        {
            return new NotFoundObjectResult(new
            {
                code = 404,
                message = $"Data with id '{id}' not found",
                error = "Not Found"
            });
        }

        return Ok(data);
    }

    [HttpPost]
    public virtual async Task<ActionResult> Post(TCREATE createDto)
    {
        try
        {
            var data = mapper.Map<T>(createDto);

            await repository.Add(data);
            await unit.CompleteAsync();

            return Ok(new { message = "Created", data.Id, data.Iid });
        }
        catch (Exception e)
        {
            return HandleExceptions.OnError(e);
        }
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult> Put(Guid id, TUPDATE updateDto)
    {
        try
        {
            var data = await repository.FindOne(id);

            if (data is null)
            {
                return new NotFoundObjectResult(new
                {
                    code = 404,
                    message = $"Data with id '{id}' not found",
                    error = "Not Found"
                });
            }

            data = mapper.Map(updateDto, data);

            repository.Update(data);
            await unit.CompleteAsync();

            return Ok(new { message = "Created", data.Id, data.Iid });
        }
        catch (Exception e)
        {
            return HandleExceptions.OnError(e);
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        return await repository.Delete(id)
            ? Ok(new { message = "Deleted" })
            : new NotFoundObjectResult(new
            {
                code = 404,
                message = $"Data with id '{id}' not found",
                error = "Not Found"
            });
    }
}