using CimcikSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CimcikSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public EntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateEntry")]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            command.CreatedById ??= UserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntryComment")]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommentCommand command)
        {
            command.CreatedById = command.CreatedById ?? UserId;
            
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}