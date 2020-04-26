namespace Mahamudra.Result.Core.Patterns
{
    public class Success<TSuccess, TMessage> : Result<TSuccess, TMessage>
    {
        public Success(TSuccess input) : base (input)
        {

        } 
    }
}
