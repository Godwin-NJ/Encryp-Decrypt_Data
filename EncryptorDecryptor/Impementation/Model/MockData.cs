namespace EncryptorDecryptor.Impementation.Model
{
    public static class MockData
    {
        public static string SamplePayload = "This is a sample payload to be encrypted and decrypted.";
        public static List<PeopleDto> peopleData()
        {
             return new List<PeopleDto>
            {
                new PeopleDto { Id = 1, Name = "Alice", Age = 30 },
                new PeopleDto { Id = 2, Name = "Bob", Age = 25 },
                new PeopleDto { Id = 3, Name = "Charlie", Age = 35 }
            };
        }
    }



    public class PeopleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
