using AutoMapper;
using AutoMapper.QueryableExtensions;
using Categories.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Items.Queries.GetItemList;

public class GetItemListQueryHandler
    : IRequestHandler<GetItemListQuery, ItemListVm>
{
    private readonly ICategoriesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetItemListQueryHandler(ICategoriesDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ItemListVm> Handle(GetItemListQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _dbContext.Items
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ItemListVm { Items = items };
    }
}
