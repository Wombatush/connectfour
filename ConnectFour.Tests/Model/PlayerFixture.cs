namespace ConnectFour.Tests.Model
{
    using System;
    using ConnectFour.Model;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class PlayerFixture
    {
        [Test]
        public void ShouldConstructorThrowWhenNullPlayerName()
        {
            // When
            Action action = () => new Player(null, 'O');

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("playerName");
        }

        [Test]
        public void ShouldConstructProperly([Values("", " ", "Reds")] string playerName, [Values(char.MinValue, char.MaxValue, 'a', 'A')] char playerChar)
        {
            // Given
            var instance = default(Player);

            // When
            Action action = () => instance = new Player(playerName, playerChar);

            // Then
            action.ShouldNotThrow();
            instance.PlayerName.Should().Be(playerName);
            instance.PlayerChar.Should().Be(playerChar);
        }

        [Test]
        public void ShouldUsePlayerNameWhenConvertedToString([Values("", " ", "Reds")] string playerName)
        {
            // Given
            var instance = new Player(playerName, 'x');

            // When
            var result = instance.ToString();

            // Then
            result.Should().Be(playerName);
        }
    }
}
