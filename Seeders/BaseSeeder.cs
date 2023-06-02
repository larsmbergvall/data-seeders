namespace Seeders;

public abstract class BaseSeeder : ISeeder
{
    public void Seed()
    {
        if (ShouldRun())
        {
            Run();
        }
    }

    protected abstract bool ShouldRun();
    protected abstract void Run();
}