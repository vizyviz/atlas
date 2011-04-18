using System;
using System.IO;
using System.Text;
using Atlas.Console;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ConsoleWrapperTests
    {
        private IWrapConsole _console;
        private TextWriter _writer;
        private StringBuilder _builder;
        private TextWriter _oldOut;

        [SetUp]
        public void Setup()
        {
            _console = new ConsoleWrapper();

            _oldOut = Console.Out;
            _builder = new StringBuilder();
            _writer = new StringWriter(_builder);
            Console.SetOut(_writer);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(_oldOut);
        }

        [Test]
        public void WriteLineWritesToConsole()
        {
            _console.WriteLine("blah");

            Assert.AreEqual("blah\r\n", _builder.ToString());
        }

        [Test]
        public void WriteLineWithAFormatStringAndNoArgsWritesCorrectly()
        {
            _console.WriteLine("blah {0} blah");

            Assert.AreEqual("blah {0} blah\r\n", _builder.ToString());
        }

        [Test]
        public void WriteLineWithAFormatStringAndNullArgumentWritesCorrectly()
        {
            _console.WriteLine("blah {0} blah", null);

            Assert.AreEqual("blah  blah\r\n", _builder.ToString());
        }

        [Test]
        public void WriteLineWithAFormatStringAndSingleArgumentWritesCorrectly()
        {
            _console.WriteLine("blah {0} blah", 1);

            Assert.AreEqual("blah 1 blah\r\n", _builder.ToString());
        }

        [Test]
        public void WriteLineWithAFormatStringAndMultipleArgsWritesFormatCorrectly()
        {
            _console.WriteLine("blah {0}, {2}, {1}, blah", 1, 2, 3);

            Assert.AreEqual("blah 1, 3, 2, blah\r\n", _builder.ToString());
        }
    }
}
