using EPAM.IntegrationTests.Util;

namespace EPAM.IntegrationTests.Abstraction
{
    public abstract class IntegrationTest : IDisposable
    {
        protected readonly BaseTestFixture App;

        protected IntegrationTest()
        {
            App = new BaseTestFixture();
        }

        public void Dispose()
        {
            App.Dispose();
        }
    }
}
