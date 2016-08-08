using System;
using System.Collections.Generic;

using ConnectFour.Model;
using ConnectFour.Wrappers;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace ConnectFour.Tests.Model
{
    [TestFixture]
    internal sealed class DimensionReaderFixture
    {
        private Mock<IInputService> inputService;
        private Mock<IOutputService> outputService;

        [SetUp]
        public void SetUp()
        {
            inputService = new Mock<IInputService>(MockBehavior.Strict);
            outputService = new Mock<IOutputService>(MockBehavior.Strict);   
        }

        [Test]
        public void ShouldCreate()
        {

            // Given
            var instance = default(DimensionReader);

            // When
            Action action = () => instance = CreateInstance();

            // Then
            action.ShouldNotThrow();
            instance.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotCreateWithNullInputService()
        {

            // Given
            var instance = default(DimensionReader);

            // When
            Action action = () => instance = new DimensionReader(null, outputService.Object);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("inputService");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldNotCreateWithNullOutputService()
        {

            // Given
            var instance = default(DimensionReader);

            // When
            Action action = () => instance = new DimensionReader(inputService.Object, null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("outputService");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldParseExactly([Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var instance = CreateInstance();
            var value = $"{rowValue}{separator}{columnValue}";
            inputService.Setup(x => x.ReadLine()).Returns(value);

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Once);
        }
        
        [Test]
        public void ShouldParseWithSilentRetry([Values(null, "", " ")] string probe, [Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var value = $"{rowValue}{separator}{columnValue}";
            var values = new Queue<string>(new[] { probe, value });
            var instance = CreateInstance();
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
        }

        [Test]
        public void ShouldParseWithLoudRetryWhenRowNumberIsIncorrect([Values("WRONG")] string probe, [Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var wrong = $"{probe}{separator}{columnValue}";
            var value = $"{rowValue}{separator}{columnValue}";
            var values = new Queue<string>(new[] { wrong, value });
            var instance = CreateInstance();
            outputService.Setup(x => x.WriteLine(It.IsAny<string>()));
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
            outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            outputService.Verify(x => x.WriteLine("Invalid entry: the 'number of rows' cannot be parsed to number"), Times.Once);
        }

        [Test]
        public void ShouldParseWithLoudRetryWhenColumnNumberIsIncorrect([Values("WRONG")] string probe, [Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var wrong = $"{rowValue}{separator}{probe}";
            var value = $"{rowValue}{separator}{columnValue}";
            var values = new Queue<string>(new[] { wrong, value });
            var instance = CreateInstance();
            outputService.Setup(x => x.WriteLine(It.IsAny<string>()));
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
            outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            outputService.Verify(x => x.WriteLine("Invalid entry: the 'number of columns' cannot be parsed to number"), Times.Once);
        }

        [Test]
        public void ShouldParseWithLoudRetryWhenOnlyOneWordWasGiven([Values("WRONG", "1")] string probe, [Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var value = $"{rowValue}{separator}{columnValue}";
            var values = new Queue<string>(new[] { probe, value });
            var instance = CreateInstance();
            outputService.Setup(x => x.WriteLine(It.IsAny<string>()));
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
            outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            outputService.Verify(x => x.WriteLine("Invalid entry: please enter two numbers separated by a white space"), Times.Once);
        }

        [Test]
        public void ShouldParseWithLoudRetryWhenMoreThanTwoWordsWasGiven([Values("1 1 1", "W R O N G")] string probe, [Values(-1, 0, 1)] int columnValue, [Values(-1, 0, 1)] int rowValue, [Values(" ", "\t", "  ")] string separator)
        {
            // Given
            var value = $"{rowValue}{separator}{columnValue}";
            var values = new Queue<string>(new[] { probe, value });
            var instance = CreateInstance();
            outputService.Setup(x => x.WriteLine(It.IsAny<string>()));
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int rows;
            int columns;
            instance.Read(out columns, out rows);

            // Then
            rows.Should().Be(rowValue);
            columns.Should().Be(columnValue);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
            outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            outputService.Verify(x => x.WriteLine("Invalid entry: please enter two numbers separated by a white space"), Times.Once);
        }

        private DimensionReader CreateInstance()
        {
            return new DimensionReader(
                inputService.Object, 
                outputService.Object);
        }
    }
}
