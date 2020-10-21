using System;
using Xunit;

namespace LibraryApiIntegrationTests
{
    
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int x = 10, y = 20;

            int answer = x + y;

            Assert.Equal(30, answer);
        }
        [Theory]
        [InlineData(2,2,4)]
        [InlineData(10,2, 12)]
        [InlineData(8,8, 16)]
        public void CanAdd(int x, int y, int expected)
        {
            var answer = x + y;
            Assert.Equal(expected, answer);
        }
    }
}
