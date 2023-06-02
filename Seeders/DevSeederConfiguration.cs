namespace Seeders;

public abstract class DevSeederConfiguration : BaseSeeder
{
    private List<ISeeder> _seeders = new List<ISeeder>();

    protected DevSeederConfiguration AddSeeder(ISeeder seeder)
    {
        _seeders.Add(seeder);

        return this;
    }

    protected DevSeederConfiguration AddSeeders(List<ISeeder> seeders)
    {
        _seeders.AddRange(seeders);

        return this;
    }

    protected override void Run()
    {
        foreach (var seeder in _seeders)
        {
            seeder.Seed();
        }
    }

    protected override bool ShouldRun()
    {
        return true;
    }
}