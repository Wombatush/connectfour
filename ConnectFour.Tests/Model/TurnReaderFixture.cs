namespace ConnectFour.Tests.Model
{
    using System;
    using System.Collections.Generic;
    using ConnectFour.Model;
    using ConnectFour.Wrappers;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class TurnReaderFixture
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
            var instance = default(TurnReader);

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
            var instance = default(TurnReader);

            // When
            Action action = () => instance = new TurnReader(null, outputService.Object);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("inputService");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldNotCreateWithNullOutputService()
        {

            // Given
            var instance = default(TurnReader);

            // When
            Action action = () => instance = new TurnReader(inputService.Object, null);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("outputService");
            instance.Should().BeNull();
        }

        [Test]
        public void ShouldParseExactly([Values(-1, 0, 1)] int number)
        {
            // Given
            var instance = CreateInstance();
            inputService.Setup(x => x.ReadLine()).Returns(number.ToString);

            // When
            int result;
            instance.Read(out result);

            // Then
            result.Should().Be(number);
            inputService.Verify(x => x.ReadLine(), Times.Once);
        }

        [Test]
        public void ShouldParseWithSilentRetry([Values(null, "", " ")] string probe, [Values(-1, 0, 1)] int number)
        {
            // Given
            var values = new Queue<string>(new[] { probe, number.ToString() });
            var instance = CreateInstance();
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());
                
            // When
            int result;
            instance.Read(out result);

            // Then
            result.Should().Be(number);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
        }

        [Test]
        public void ShouldParseWithLoudRetry([Values("I am not a number")] string probe, [Values(-1, 0, 1)] int number)
        {
            // Given
            var values = new Queue<string>(new[] { probe, number.ToString() });
            var instance = CreateInstance();
            outputService.Setup(x => x.WriteLine(It.IsAny<string>()));
            inputService.Setup(x => x.ReadLine()).Returns(() => values.Dequeue());

            // When
            int result;
            instance.Read(out result);

            // Then
            result.Should().Be(number);
            inputService.Verify(x => x.ReadLine(), Times.Exactly(2));
            outputService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            outputService.Verify(x => x.WriteLine("Invalid entry: the 'turn' cannot be parsed to number"), Times.Once);
        }

        private TurnReader CreateInstance()
        {
            return new TurnReader(
                inputService.Object, 
                outputService.Object);
        }
    }
}
