using TerraProc.Core.Noise;

namespace TerraProc.Core.Tests.TestDoubles;

public sealed class FakeNoiseProvider(Func<double, double, double> fn) : INoiseProvider
{
    public double Sample(double x, double y) => fn(x, y);
}