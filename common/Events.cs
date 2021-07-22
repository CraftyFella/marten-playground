namespace common
{
    public abstract record StreamName;
    public record AnEvent(string Description) : StreamName;
}