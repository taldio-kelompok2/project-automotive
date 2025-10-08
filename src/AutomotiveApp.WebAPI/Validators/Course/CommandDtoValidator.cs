// RuleFor(x => x.Id)
//                 .NotEmpty().WithMessage("Course Id is required.")
//                 .MustAsync(async (dto, id, ct) => await IdExists(id))
//                 .WithMessage(dto => $"Course Id {dto.Id} dosent exist");

//             RuleFor(x => x.Name)
//                 .NotEmpty().WithMessage("Name is required.");

//             RuleFor(x => x.Price)
//             .NotEmpty().WithMessage("Price is required.")
//             .GreaterThan(0).WithMessage("Price must be greater than 0");

//             // nunggu category Endpoint jadi kalo ga ga bisa fetch :)
//             RuleFor(x => x.Category)
//                 .NotEmpty().WithMessage("Category is required.");
//             // .MustAsync(CategoryExists).WithMessage("Category does not exist.");