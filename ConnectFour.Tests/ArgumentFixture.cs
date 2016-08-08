using System;

using FluentAssertions;

using NUnit.Framework;

namespace ConnectFour.Tests
{
    [TestFixture]
    internal sealed class ArgumentFixture
    {
        [Test]
        public void ShouldIsNotNullNotThrowWhenInstanceIsNotNull()
        {
            // When
            Action action = () => Argument.IsNotNull(new object(), "irrelevant");

            // Then
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldIsNotNullThrowWhenInstanceIsNull()
        {
            // Given
            const string paramName = "tell me who you are";

            // When
            Action action = () => Argument.IsNotNull(default(object), paramName);

            // Then
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be(paramName);
        }

        [Test]
        public void ShouldIsGreaterThanOrEqualToNotThrowWhenValueIsGreaterThanComparand()
        {
            // When
            Action action = () => Argument.IsGreaterThanOrEqualTo(4, 3, "irrelevant");

            // Then
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldIsGreaterThanOrEqualToNotThrowWhenValueIsEqualToComparand()
        {
            // When
            Action action = () => Argument.IsGreaterThanOrEqualTo(3, 3, "irrelevant");

            // Then
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldIsGreaterThanOrEqualToThrowWhenValueIsLessThanComparand()
        {
            // Given
            const string paramName = "tell me who you are";

            // When
            Action action = () => Argument.IsGreaterThanOrEqualTo(2, 3, paramName);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be(paramName);
        }

        [Test]
        public void ShouldIsLessThanNotThrowWhenValueIsLessThanComparand()
        {
            // When
            Action action = () => Argument.IsLessThan(2, 3, "irrelevant");

            // Then
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldIsLessThanThrowWhenValueIsEqualToComparand()
        {
            // Given
            const string paramName = "tell me who you are";

            // When
            Action action = () => Argument.IsLessThan(3, 3, paramName);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be(paramName);
        }

        [Test]
        public void ShouldIsLessThanThrowWhenValueIsGreaterThanComparand()
        {
            // Given
            const string paramName = "tell me who you are";

            // When
            Action action = () => Argument.IsLessThan(4, 3, paramName);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be(paramName);
        }
    }
}
