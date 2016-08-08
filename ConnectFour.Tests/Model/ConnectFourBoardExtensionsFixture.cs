namespace ConnectFour.Tests.Model
{
    using System;
    using ConnectFour.Model;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class ConnectFourBoardExtensionsFixture
    {
        [Test]
        public void ShouldThrowOnGetCellCharWhenNullPointer()
        {
            // Given
            var instance = default(IConnectFourBoard);

            // When
            Action action = () => instance.GetCellChar(1, 1);

            // Then
            action.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ShouldGetCellCharWithPlayerChar([Values(-1, 0, 1)] int columnIdx, [Values(-1, 0, 1)] int rowIdx, [Values('o', 'O')] char playerChar)
        {
            // Given
            var player = new Player("Generic Player", playerChar);
            var instanceMock = new Mock<IConnectFourBoard>(MockBehavior.Strict);
            instanceMock.Setup(x => x.GetCell(columnIdx, rowIdx)).Returns(player);
            var instance = instanceMock.Object;

            // When
            var result = instance.GetCellChar(columnIdx, rowIdx);

            // Then
            result.Should().Be(playerChar);
            instanceMock.Verify(x => x.GetCell(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            instanceMock.Verify(x => x.GetCell(columnIdx, rowIdx), Times.Once);
        }

        [Test]
        public void ShouldGetCellCharWithEmptyChar([Values(-1, 0, 1)] int columnIdx, [Values(-1, 0, 1)] int rowIdx, [Values('o', 'O')] char emptyChar)
        {
            // Given
            var instanceMock = new Mock<IConnectFourBoard>(MockBehavior.Strict);
            instanceMock.Setup(x => x.GetCell(columnIdx, rowIdx)).Returns(default(Player?));
            var instance = instanceMock.Object;
            
            // When
            var result = instance.GetCellChar(columnIdx, rowIdx, emptyChar);

            // Then
            result.Should().Be(emptyChar);
            instanceMock.Verify(x => x.GetCell(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            instanceMock.Verify(x => x.GetCell(columnIdx, rowIdx), Times.Once);
        }
    }
}
