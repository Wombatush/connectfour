using System;
using System.Text;

using ConnectFour.Model;
using ConnectFour.Wrappers;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace ConnectFour.Tests.Model
{
    [TestFixture]
    internal sealed class BoardPrinterFixture
    {
        private Mock<IOutputService> outputService;

        [SetUp]
        public void SetUp()
        {
            outputService = new Mock<IOutputService>(MockBehavior.Strict);
        }

        [Test]
        public void ShouldCreate()
        {

            // Given
            var instance = default(BoardPrinter);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        
        [Test]
        public void ShouldNotCreateWithNullOutputService()
        {

            // Given
            var instance = default(BoardPrinter);

            // When
            Action action = () => instance = new BoardPrinter(null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("outputService");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldNotPrintNullBoard()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Print(null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("board");
        }

        [Test]
        public void ShouldPrint([Values(3, 4)] int columns, [Values(3, 4)] int rows)
        {
            // Given
            var playerOne = new Player("One", '1');
            var playerTwo = new Player("Two", '2');
            var board = new Mock<IConnectFourBoard>(MockBehavior.Strict);
            board.Setup(x => x.GetCell(It.IsAny<int>(), It.IsAny<int>())).Returns(default(Player?));
            board.Setup(x => x.GetCell(It.IsAny<int>(), 0)).Returns(playerOne);
            board.Setup(x => x.GetCell(It.IsAny<int>(), 1)).Returns(playerTwo);
            board.SetupGet(x => x.Columns).Returns(columns);
            board.SetupGet(x => x.Rows).Returns(rows);
            var actualBuilder = new StringBuilder();
            var expectedBuilder = new StringBuilder();
            expectedBuilder.AppendLine(new string('1', columns));
            expectedBuilder.AppendLine(new string('2', columns));
            for (int i = 2; i < rows; i++)
            {
                expectedBuilder.AppendLine(new string('o', columns));
            }
            var instance = CreateInstance();
            outputService
                .Setup(x => x.Write(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((format, args) => actualBuilder.Append(string.Format(format, args)));
            outputService
                .Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((format, args) => actualBuilder.AppendLine(string.Format(format, args)));
            var expected = expectedBuilder.ToString();

            // When
            instance.Print(board.Object);
            var actual = actualBuilder.ToString();

            // Then
            board.VerifyGet(x => x.Columns, Times.AtLeastOnce);
            board.VerifyGet(x => x.Rows, Times.AtLeastOnce);
            actual.Should().Be(expected);
        }

        private BoardPrinter CreateInstance()
        {
            return new BoardPrinter(outputService.Object);
        }
    }
}
