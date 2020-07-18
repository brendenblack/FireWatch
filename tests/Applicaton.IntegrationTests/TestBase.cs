using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Firewatch.Application.IntegrationTests
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }

        public string ReadLocalTestFile(string filename)
        {
            //var filename = "U3111111_20200316_20200501.tlg";
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(dirName, filename);

            return File.ReadAllText(file);
        }
    }
}
