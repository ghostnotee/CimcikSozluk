using AutoMapper;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Events.Entry;
using MediatR;

namespace CimcikSozluk.Api.Application.Features.Commands.EntryComment.Create;

public class CreateEntryCommentCommandHandler : IRequestHandler<CreateEntryCommentCommand, Guid>
{
    private readonly IEntryCommentRepository _entryCommentRepository;
    private readonly IMapper _mapper;

    public CreateEntryCommentCommandHandler(IEntryCommentRepository entryCommentRepository, IMapper mapper)
    {
        _entryCommentRepository = entryCommentRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateEntryCommentCommand request, CancellationToken cancellationToken)
    {
        var dbEntryComment = _mapper.Map<Domain.Models.EntryComment>(request);
        await _entryCommentRepository.AddAsync(dbEntryComment);

        return dbEntryComment.Id;
    }
}