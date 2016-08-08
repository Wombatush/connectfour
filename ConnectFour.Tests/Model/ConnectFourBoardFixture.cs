namespace ConnectFour.Tests.Model
{
    using System;
    using System.Linq;
    using ConnectFour.Model;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class ConnectFourBoardFixture
    {
        [Test]
        public void ShouldHaveProperStaticFieldValues()
        {
            // Then
            ConnectFourBoard.MinRows.Should().Be(4);
            ConnectFourBoard.MinColumns.Should().Be(4);
            ConnectFourBoard.MaxRows.Should().Be(10);
            ConnectFourBoard.MaxColumns.Should().Be(10);
            ConnectFourBoard.CountToWin.Should().Be(4);

            ConnectFourBoard.MinRows.Should().BeGreaterOrEqualTo(ConnectFourBoard.CountToWin);
            ConnectFourBoard.MinColumns.Should().BeGreaterOrEqualTo(ConnectFourBoard.CountToWin);
        }

        [Test]
        public void ShouldCreate()
        {
            // When
            var instance = CreateInstance();

            // Then
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldResize()
        {
            // Given
            var instance = CreateInstance();

            // When
            instance.Reset(ConnectFourBoard.MinColumns + 1, ConnectFourBoard.MinRows + 2);

            // Then
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows + 2);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns + 1);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenMinColumnsConstraintViolated()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Reset(ConnectFourBoard.MinColumns - 1, instance.Rows);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnCount");
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenMaxColumnsConstraintViolated()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Reset(ConnectFourBoard.MaxColumns + 1, instance.Rows);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnCount");
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenMinRowsConstraintViolated()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Reset(instance.Columns, ConnectFourBoard.MinRows - 1);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowCount");
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenMaxRowsConstraintViolated()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Reset(instance.Columns, ConnectFourBoard.MaxRows + 1);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowCount");
            instance.Should().NotBeNull();
            instance.Rows.Should().Be(ConnectFourBoard.MinRows);
            instance.Columns.Should().Be(ConnectFourBoard.MinColumns);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();
        }

        [Test]
        public void ShouldGetCellThrowWhenColumnIndexLessThanZero()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.GetCell(-1, 0);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIdx");
        }

        [Test]
        public void ShouldGetCellThrowWhenRowIndexLessThanZero()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.GetCell(0, -1);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIdx");
        }

        [Test]
        public void ShouldGetCellThrowWhenColumnIndexIsEqualToNumberOfColumns()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.GetCell(instance.Columns, 0);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIdx");
        }

        [Test]
        public void ShouldGetCellThrowWhenRowIndexIsEqualToNumberOfRows()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.GetCell(0, instance.Rows);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("rowIdx");
        }

        [Test]
        public void ShouldTurnThrowWhenColumnIndexLessThanZero()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Turn(default(Player), -1);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIdx");
        }

        [Test]
        public void ShouldTurnThrowWhenColumnIndexisEqualToNumberOfColmns()
        {
            // Given
            var instance = CreateInstance();

            // When
            Action action = () => instance.Turn(default(Player), instance.Columns);

            // Then
            action.ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("columnIdx");
        }

        [Test]
        public void ShouldDoSingleTurn()
        {
            // Given
            var player = new Player("Generic Player", 'g');
            var instance = CreateInstance();

            // When
            var result = instance.Turn(player, 3);

            // Then
            result.Should().Be(TurnResult.Success);

            var cells =
                from y in Enumerable.Range(0, instance.Rows)
                from x in Enumerable.Range(0, instance.Columns)
                where x != 3 && y != instance.Rows - 1
                select instance.GetCell(x, y);

            cells.All(x => !x.HasValue).Should().BeTrue();

            instance.GetCell(3, instance.Rows - 1).HasValue.Should().BeTrue();
        }

        [Test]
        public void ShouldDetectWinByRow()
        {
            //// oooo
            //// oooo
            //// oooo
            //// gggg

            // Given
            var player = new Player("Generic Player", 'g');
            var instance = CreateInstance();

            // Then
            instance.Turn(player, 0).Should().Be(TurnResult.Success);
            instance.Turn(player, 1).Should().Be(TurnResult.Success);
            instance.Turn(player, 2).Should().Be(TurnResult.Success);
            instance.Turn(player, 3).Should().Be(TurnResult.Win);
        }

        [Test]
        public void ShouldDetectWinByColumn()
        {
            //// gooo
            //// gooo
            //// gooo
            //// gooo

            // Given
            var player = new Player("Generic Player", 'g');
            var instance = CreateInstance();

            // Then
            instance.Turn(player, 0).Should().Be(TurnResult.Success); 
            instance.Turn(player, 0).Should().Be(TurnResult.Success); 
            instance.Turn(player, 0).Should().Be(TurnResult.Success); 
            instance.Turn(player, 0).Should().Be(TurnResult.Win);
        }

        [Test]
        public void ShouldDetectWinByForwardDiagonal()
        {
            //// ooog
            //// ooga
            //// ogaa
            //// gaaa

            // Given
            var player1 = new Player("Generic Player", 'g');
            var player2 = new Player("Another Player", 'a');
            var instance = CreateInstance();

            // Then
            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Win);
        }

        [Test]
        public void ShouldDetectWinByBackwardDiagonal()
        {
            //// gooo
            //// agoo
            //// aago
            //// aaag

            // Given
            var player1 = new Player("Generic Player", 'g');
            var player2 = new Player("Another Player", 'a');
            var instance = CreateInstance();

            // Then
            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Success);
            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 0).Should().Be(TurnResult.Win);
        }

        [Test]
        public void ShouldDetectDraw()
        {
            //// agag
            //// agag
            //// gaga
            //// gaga

            // Given
            var player1 = new Player("Generic Player", 'g');
            var player2 = new Player("Another Player", 'a');
            var instance = CreateInstance();

            // Then
            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);

            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);

            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Success);

            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Draw);
        }

        [Test]
        public void ShouldDetectColumnOverflow()
        {
            //// aooo
            //// gooo
            //// aooo
            //// gooo

            // Given
            var player1 = new Player("Generic Player", 'g');
            var player2 = new Player("Another Player", 'a');
            var instance = CreateInstance();

            // Then
            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 0).Should().Be(TurnResult.Invalid);
        }

        [Test]
        public void ShouldDetectDrawOverflow()
        {
            //// agag
            //// agag
            //// gaga
            //// gaga

            // Given
            var player1 = new Player("Generic Player", 'g');
            var player2 = new Player("Another Player", 'a');
            var instance = CreateInstance();

            // Then
            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);

            instance.Turn(player1, 0).Should().Be(TurnResult.Success);
            instance.Turn(player2, 1).Should().Be(TurnResult.Success);
            instance.Turn(player1, 2).Should().Be(TurnResult.Success);
            instance.Turn(player2, 3).Should().Be(TurnResult.Success);

            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Success);

            instance.Turn(player2, 0).Should().Be(TurnResult.Success);
            instance.Turn(player1, 1).Should().Be(TurnResult.Success);
            instance.Turn(player2, 2).Should().Be(TurnResult.Success);
            instance.Turn(player1, 3).Should().Be(TurnResult.Draw);

            instance.Turn(player1, 3).Should().Be(TurnResult.Invalid);
        }

        private static ConnectFourBoard CreateInstance()
        {
            return new ConnectFourBoard();
        }
    }
}
