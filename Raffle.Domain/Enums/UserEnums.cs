namespace Raffle.Domain.Enums
{
    public enum DocumentType
    {
        CPF,
        CNPJ
    }

    public enum DocumentVerificationStatus
    {
        PENDING,
        VERIFIED,
        REJECTED,
        UNDER_REVIEW
    }

    public enum UserType
    {
        INDIVIDUAL, // Pessoa Física
        COMPANY     // Pessoa Jurídica
    }
}