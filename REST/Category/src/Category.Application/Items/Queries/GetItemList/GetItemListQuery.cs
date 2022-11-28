using System;
using MediatR;

namespace Categories.Application.Items.Queries.GetItemList;

public class GetItemListQuery : IRequest<ItemListVm>
{
}
