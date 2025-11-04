namespace Application.Interfaces
{
    public interface IPinHasher
    {
        string Hash(string pin);
        bool Verify(string hash, string providedPin);
    }
}