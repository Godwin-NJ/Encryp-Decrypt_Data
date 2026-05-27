namespace EncryptorDecryptor.Impementation.Dto
{
    public class Encrypt_DecryptConfig
    {
        public bool IsActive { get; set; }
        public string Aes_Key { get; set; }
        public string Aes_IV { get; set; }
        public string Rsa_PublicKey { get; set; }
        public string Rsa_PrivateKey { get; set; }

    }
}
