using EncryptorDecryptor.Impementation.Dto;
using EncryptorDecryptor.Impementation.Model;

namespace EncryptorDecryptor.Impementation
{
    public interface IEncryptDecrypt
    {
        EncryptDto Encrypt(string payload);
        dynamic Decrypt(DecryptDto dto);
        string SamplePayload();
        List<PeopleDto> SamplePeopleData();
        PeopleDto GetPeopleById(PeopleDataDto dto);
    }
}
