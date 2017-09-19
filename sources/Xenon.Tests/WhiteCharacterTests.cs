using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class WhiteCharacterTests
    {
        [Test]
        public async Task ShouldPreserveWhiteCharactersByDefault()
        {
            const string data = @"<data> 123 </data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToString(), Is.EqualTo(" 123 "));
                });
            }
        }

        [Test]
        public async Task ShouldOmitWhiteCharacters()
        {
            const string data = @"<data> 123 </data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");
                XenonWhiteCharacter mode = XenonWhiteCharacter.Omit;

                await reader.Process(document =>
                {
                    Assert.That(document.ToString(mode), Is.EqualTo("123"));
                });
            }
        }
    }
}
