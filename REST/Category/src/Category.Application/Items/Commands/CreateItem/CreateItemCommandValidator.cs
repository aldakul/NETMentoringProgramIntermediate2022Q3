﻿using FluentValidation;

namespace Categories.Application.Items.Commands.CreateItem;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x =>
            x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
    }
}
