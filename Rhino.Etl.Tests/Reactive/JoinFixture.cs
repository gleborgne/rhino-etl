using System.Collections.Generic;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Reactive;
using Rhino.Etl.Tests.Joins;
using Xunit;

namespace Rhino.Etl.Tests.Reactive
{
    public class JoinFixture : BaseJoinFixture
    {
        [Fact]
        public void InnerJoin()
        {
            var result = Input.From(left).InnerJoin(Input.From(right), "email", MergeRows).Execute();

            List<Row> items = (List<Row>)result.Data;

            Assert.Equal(1, items.Count);
            Assert.Equal(3, items[0]["person_id"]);
        }

        [Fact]
        public void RightJoin()
        {
            var result = Input.From(left).RightJoin(Input.From(right), "email", MergeRows).Execute();

            List<Row> items = (List<Row>)result.Data;

            Assert.Equal(2, items.Count);
            Assert.Equal(3, items[0]["person_id"]);
            Assert.Null(items[1]["name"]);
            Assert.Equal(5, items[1]["person_id"]);
        }

        [Fact]
        public void LeftJoin()
        {
            var result = Input.From(left).LeftJoin(Input.From(right), "email", MergeRows).Execute();
            List<Row> items = (List<Row>)result.Data;

            Assert.Equal(2, items.Count);
            Assert.Equal(3, items[0]["person_id"]);
            Assert.Null(items[1]["person_id"]);
            Assert.Equal("bar", items[1]["name"]);
        }

        [Fact]
        public void FullJoin()
        {
            var result = Input.From(left).FullJoin(Input.From(right), "email", MergeRows).Execute();
            List<Row> items = (List<Row>)result.Data;

            Assert.Equal(3, items.Count);

            Assert.Equal(3, items[0]["person_id"]);

            Assert.Null(items[1]["person_id"]);
            Assert.Equal("bar", items[1]["name"]);

            Assert.Null(items[2]["name"]);
            Assert.Equal(5, items[2]["person_id"]);
        }

        [Fact]
        public void CanJoinOnEnumerable()
        {
            var result = Input.From(left).Join(right, new RowJoinHelper("email").InnerJoinMatch, MergeRows).Execute();
            Assert.Equal(1, result.Count);
            Assert.Equal(3, ((List<Row>)result.Data)[0]["person_id"]);
        }

        [Fact]
        public void CanUseComplexJoinInProcesses()
        {
            var result =
                Input.From(left)
                .Transform(RowColumnToUpperCase)
                .RightJoin(Input.From(right).Transform(RowColumnToUpperCase), "email", MergeRows)
                .Execute();
            Assert.Equal(2, result.Count);
            Assert.Equal("FOO", ((List<Row>)result.Data)[0]["name"]);
            Assert.Equal("FOO@EXAMPLE.ORG", ((List<Row>)result.Data)[0]["email"]);
            Assert.Equal(5, ((List<Row>)result.Data)[1]["person_id"]);

        }

        private Row RowColumnToUpperCase(Row row)
        {
            foreach (string column in row.Columns)
            {
                string item = row[column] as string;
                if (item != null)
                    row[column] = item.ToUpper();
            }

            return row;
        }

        private Row MergeRows(Row leftRow, Row rightRow)
        {
            Row row = new Row();
            row.Copy(leftRow);
            if (rightRow != null)
                row["person_id"] = rightRow["id"];
            return row;
        }
    }
}
