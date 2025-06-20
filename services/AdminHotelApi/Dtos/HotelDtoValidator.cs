using FluentValidation;

namespace AdminHotelApi.Dtos;

public class HotelValidator : AbstractValidator<HotelDto>
{
    public HotelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Price).NotNull().GreaterThan(0);
    }
}

public class CreateHotelDtoValidator : AbstractValidator<HotelDto>
{
    public CreateHotelDtoValidator()
    {
        Include(new HotelValidator());
    }
}

public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
{
    public UpdateHotelDtoValidator()
    {
        Include(new HotelValidator());
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}
