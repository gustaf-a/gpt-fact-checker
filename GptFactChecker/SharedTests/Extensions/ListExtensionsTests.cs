using Shared.Extensions;

namespace Shared.Tests.Extensions
{
    public class ListExtensionsTests
    {
        [Fact]
        public void IsNullOrEmpty_ShouldReturnTrue_WhenListIsNull()
        {
            List<int> list = null;

            bool result = list.IsNullOrEmpty();

            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_ShouldReturnTrue_WhenListIsEmpty()
        {
            var list = new List<int>();

            bool result = list.IsNullOrEmpty();

            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_ShouldReturnFalse_WhenListIsNotEmpty()
        {
            var list = new List<int> { 1, 2, 3 };

            bool result = list.IsNullOrEmpty();

            Assert.False(result);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 3, 2)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 3, 3)]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, 3, 2)]
        [InlineData(new int[] { 1, 2, 3 }, 3, 1)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 2, 3)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 1, 6)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 6, 1)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 10, 1)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 0, 1)]
        public void SplitByMaxCount_ShouldReturnCorrectlySplitLists(int[] initialArray, int maxCount, int expectedCount)
        {
            var list = new List<int>(initialArray);

            var result = list.SplitByMaxCount(maxCount);

            Assert.Equal(expectedCount, result.Count);

            Assert.True(result.All(sublist => sublist.Count > 0));

            if (maxCount > 0)
                Assert.True(result.All(sublist => sublist.Count <= maxCount));
        }


        [Fact]
        public void SplitIntoParts_ShouldThrowArgumentException_WhenPartsIsZero()
        {
            var list = new List<int> { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => list.SplitIntoParts(0));
        }

        [Fact]
        public void SplitIntoParts_ShouldThrowArgumentException_WhenPartsIsGreaterThanListCount()
        {
            var list = new List<int> { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => list.SplitIntoParts(4));
        }

        [Fact]
        public void SplitIntoParts_ShouldReturnCorrectlySplitLists()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6 };

            var result = list.SplitIntoParts(3);

            Assert.Equal(3, result.Count);
            Assert.True(result.All(sublist => sublist.Count <= 2));
        }
    }
}
