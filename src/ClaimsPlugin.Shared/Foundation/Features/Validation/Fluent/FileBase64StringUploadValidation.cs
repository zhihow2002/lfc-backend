using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using ClaimsPlugin.Shared.Foundation.Features.FileStorage.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using FluentValidation;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Fluent.Models;

public static class FileBase64StringUploadValidation
{
    public static IRuleBuilderOptions<T, TProperty> File<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        long size,
        params FileUtility.FileFormat[] formats
    )
        where TProperty : FileUploadBase64String?
    {
        return ruleBuilder
            .ChildRules(x =>
            {
                x.RuleFor(t => t!.Name)
                    .NotEmpty()
                    .WithMessage("File name cannot be empty.")
                    .MaximumLength(150);

                x.RuleFor(t => t!.Extension)
                    .NotEmpty()
                    .Must(t => FileUtility.IsExtensionMatched(t, formats));

                x.RuleFor(t => t!.Data)
                    .NotEmpty()
                    .WithMessage("File data cannot be empty.")
                    .Must(t => t.IsBase64String())
                    .Must(t => FileUtility.GetFileSize(t) <= size);
            })
            .When(x => x.IsNotNull());
    }
}
