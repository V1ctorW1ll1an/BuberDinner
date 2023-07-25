using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.Menu;
using BuberDinner.Domain.Menu.Entities;

using ErrorOr;

using MediatR;

namespace BuberDinner.Application.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandle : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{

    public async Task<ErrorOr<Menu>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        // create menu
        var menu = Menu.Create(
            HostId.Create(request.HostId),
            request.Name,
            request.Description,
            request.Sections.ConvertAll(section => MenuSection.Create(
                section.Name,
                section.Description,
                section.Items.ConvertAll(item => MenuItem.Create(
                    item.Name,
                    item.Description
                ))
            ))
        );
        // persist menu
        // return menu
        return default!;
    }
}
