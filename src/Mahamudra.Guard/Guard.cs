namespace Mahamudra.Guard
{
    public class Guard : IGuard 
    { 
        public static IGuard Check { get; } = new Guard(); 
        private Guard() { }
    }
}
