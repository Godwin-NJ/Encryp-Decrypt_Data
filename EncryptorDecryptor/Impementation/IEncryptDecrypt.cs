using EncryptorDecryptor.Impementation.Dto;

namespace EncryptorDecryptor.Impementation
{
    public interface IEncryptDecrypt
    {
        EncryptDto Encrypt(string payload);
        dynamic Decrypt(DecryptDto dto);
        string SamplePayload();
        dynamic SamplePeopleData();
    }
}
