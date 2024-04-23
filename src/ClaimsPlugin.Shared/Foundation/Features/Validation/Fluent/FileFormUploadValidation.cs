//using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
//using FluentValidation;

//namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Fluent.Models;

//public static class FileFormUploadValidation
//{
//    public static IRuleBuilderOptions<T, TProperty> File<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, long size, params FileUtility.FileFormat[] formats) where TProperty : FileUploadForm?
//    {
//        return ruleBuilder.ChildRules(
//            x =>
//            {
//                x.RuleFor(t => t!.File.Length)
//                    .InclusiveBetween(0, FileUtility.GetFileSize(size))
//                    .WithMessage($"File length should be greater than 0 and less than {size} MB");

//                x.RuleFor(t => t!.File.ContentType)
//                    .NotEmpty()
//                    .Must(t => FileUtility.IsContentTypeMatched(t, formats));

//                x.RuleFor(t => t!.File.FileName)
//                    .Must(t => FileUtility.IsExtensionMatched(t, formats));

//            }).When(x => x.IsNotNull());
//    }
//}
