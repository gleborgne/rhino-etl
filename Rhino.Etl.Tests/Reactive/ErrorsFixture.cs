using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Rhino.Etl.Core.Reactive;
using Rhino.Etl.Tests.UsingDAL;
using System.IO;

namespace Rhino.Etl.Tests.Reactive
{
    public class ErrorsFixture
    {        
        public IEnumerable<User> ListUsers(int numusers)
        {
            for (int i=0 ; i < numusers; i++)
            {
                yield return new User() {Id = i, Email = "1@rhino.com", Name = "User" + i};
            }
        }

        [Fact]
        public void WillReportErrorsWhenThrown()
        {
            int maxElements = 1000;
            int throwAfter = 15;
            int rowCount = 0;

            var result = Input.From(ListUsers(maxElements))
                .Apply(row =>
                {
                    rowCount++;
                    if (rowCount > throwAfter)
                        throw new InvalidDataException("problem");
                })
                .Execute();

            Assert.Equal(1, result.CountExceptions);
        }
    }
}
