using Bogus;
using System.Runtime.Serialization;

namespace OrderService.IntegrationTests.TestUtils
{
    public static class BogusExtensions
    {
        public static Faker<T> WithRecord<T>(this Faker<T> faker) where T : class
        {
#pragma warning disable
            faker.CustomInstantiator(_ =>
                FormatterServices.GetUninitializedObject(typeof(T)) as T
                );
#pragma warning restore 
            return faker;
        }
    }
}