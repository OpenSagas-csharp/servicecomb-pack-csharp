using System;
using System.Collections.Generic;
using System.Text;
using Servicecomb.Saga.Omega.Core.Serializing;
using Xunit;

namespace Servicecomb.Saga.Omega.Core.Tests.Serializing
{
    public class MessagePackMessageFormatTest
    {
        [Fact]
        public void CanFormat()
        {
            var messageFormat=new MessagePackMessageFormat();
            Assert.NotEmpty(messageFormat.Serialize(new object[] { 123 }));
            Assert.NotEmpty( messageFormat.Deserialize<object[]>(messageFormat.Serialize(new object[] { 123 })));

        }
    }
}
