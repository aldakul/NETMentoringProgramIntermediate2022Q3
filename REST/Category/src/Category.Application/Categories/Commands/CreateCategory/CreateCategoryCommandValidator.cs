﻿using FluentValidation;

namespace Categories.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(createCategoryCommand =>
            createCategoryCommand.Name).NotEmpty().MaximumLength(250);
    }
}
