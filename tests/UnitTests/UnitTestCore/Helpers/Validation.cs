using Mahamudra.Result.Core.Patterns;

namespace UnitTestsCore
{
    public static class Validation
    {
        public static Result<Person, string> CheckName(Result<Person, string> person)
        {
            if (string.IsNullOrWhiteSpace(person.Value.Name))
                return new Failure<Person, string>("Name should not be blank.");
            else
                return new Success<Person, string>(person.Value);
        }

        public static Result<Person, string> CheckEmail(Result<Person, string> person)
        {
            if (string.IsNullOrWhiteSpace(person.Value.Email))
                return new Failure<Person, string>("Email should not be blank.");
            else
                return new Success<Person, string>(person.Value);
        }
        public static Result<Person, string> CheckAge(Result<Person, string> person)
        {
            if (person.Value.Age < 18)
                return new Failure<Person, string>("The age should be not inferior than 18.");
            else
                return new Success<Person, string>(person.Value);
        }

        public static Result<Person, string> CheckName(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.Name))
                return new Failure<Person, string>("Name should not be blank.");
            else
                return new Success<Person, string>(person);
        }

        public static Result<Person, string> CheckEmail(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.Email))
                return new Failure<Person, string>("Email should not be blank.");
            else
                return new Success<Person, string>(person);
        }
        public static Result<Person, string> CheckAge(Person person)
        {
            if (person.Age < 18)
                return new Failure<Person, string>("The age should be not inferior than 18.");
            else
                return new Success<Person, string>(person);
        }
    }
}
