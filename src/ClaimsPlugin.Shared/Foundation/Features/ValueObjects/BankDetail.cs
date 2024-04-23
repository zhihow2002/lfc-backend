//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class BankDetail : BaseValueObject
//{
//    protected BankDetail()
//    {
//    }

//    private BankDetail(string bankName, string bankCode, string bankAccountNumber, string bankSwiftCode, string bankBranchCode, string? bankRoutingNumber = null)
//    {
//        BankName = bankName;
//        BankCode = bankCode;
//        BankAccountNumber = bankAccountNumber;
//        BankSwiftCode = bankSwiftCode;
//        BankBranchCode = bankBranchCode;
//        BankRoutingNumber = bankRoutingNumber;
//    }
    
//    public string BankName { get; private set; } = default!;
//    public string BankCode { get; private set; } = default!;
//    public string BankAccountNumber { get; private set; } = default!;
//    public string BankSwiftCode { get; private set; } = default!;
//    public string? BankRoutingNumber { get; private set; } = default!;
//    public string BankBranchCode { get; private set; } = default!;

//    public BankDetail Duplicate()
//    {
//        return Create(BankName, BankCode, BankAccountNumber, BankSwiftCode, BankBranchCode, BankRoutingNumber);
//    }

//    public static BankDetail Create(string bankName, string bankCode, string bankAccountNumber, string bankSwiftCode, string bankBranchCode, string? bankRoutingNumber = null)
//    {
//        // TODO: Based on bank name to validate the code?

//        if (bankName.IsNullOrWhiteSpace(out string? bankNameNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(bankNameNullOrWhiteSpaceErrorMessage);
//        }

//        if (bankName.HasLengthMoreThan(100, out string? bankNameLengthErrorMessage))
//        {
//            throw new DomainException(bankNameLengthErrorMessage);
//        }

//        if (bankCode.IsNullOrWhiteSpace(out string? bankCodeNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(bankCodeNullOrWhiteSpaceErrorMessage);
//        }

//        if (bankCode.HasLengthMoreThan(20, out string? bankCodeLengthErrorMessage))
//        {
//            throw new DomainException(bankCodeLengthErrorMessage);
//        }

//        if (bankAccountNumber.IsNullOrWhiteSpace(out string? bankAccountNumberNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(bankAccountNumberNullOrWhiteSpaceErrorMessage);
//        }

//        if (bankAccountNumber.HasLengthMoreThan(50, out string? bankAccountNumberLengthErrorMessage))
//        {
//            throw new DomainException(bankAccountNumberLengthErrorMessage);
//        }

//        if (bankSwiftCode.IsNullOrWhiteSpace(out string? bankSwiftCodeNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(bankSwiftCodeNullOrWhiteSpaceErrorMessage);
//        }

//        if (bankSwiftCode.HasLengthMoreThan(20, out string? bankSwiftCodeLengthErrorMessage))
//        {
//            throw new DomainException(bankSwiftCodeLengthErrorMessage);
//        }

//        if (bankBranchCode.IsNullOrWhiteSpace(out string? bankBranchCodeNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(bankBranchCodeNullOrWhiteSpaceErrorMessage);
//        }

//        if (bankBranchCode.HasLengthMoreThan(20, out string? bankBranchCodeLengthErrorMessage))
//        {
//            throw new DomainException(bankBranchCodeLengthErrorMessage);
//        }
        
//        return new BankDetail(bankName, bankCode, bankAccountNumber, bankSwiftCode, bankBranchCode, bankRoutingNumber);
//    }
    
//    public static implicit operator string(BankDetail bankDetail)
//    {
//        return bankDetail.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{BankCode}, {BankName}, {BankSwiftCode}, {BankBranchCode}, {BankAccountNumber}, {BankRoutingNumber}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return BankName;
//        yield return BankCode;
//        yield return BankAccountNumber;
//        yield return BankSwiftCode;
//        yield return BankRoutingNumber;
//        yield return BankBranchCode;
//    }
//}
