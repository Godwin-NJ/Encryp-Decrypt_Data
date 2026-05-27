namespace EncryptorDecryptor.Impementation.Model
{
    public static class MockData
    {
        public static string SamplePayload = "This is a sample payload to be encrypted and decrypted.";
        public static dynamic peopleData()
        {
             return new[]
            {
                new { Id = 1, Name = "Alice", Age = 30 },
                new { Id = 2, Name = "Bob", Age = 25 },
                new { Id = 3, Name = "Charlie", Age = 35 }
            };
        }
    }
}
