/*using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests
{
    internal class StringCalculator_UnitTests
    {
        [Test]
        public void Add_Empty_String_Returns_0()
        {
            StringCalculator calc = new StringCalculator();
            int expectedResult = 0;
            int result = calc.Add("");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Add_ResultIsAPrimeNumber_ResultsAreSaved()
        {
            Mock<IStore> mockStore = new Mock<IStore>();
            var calc = new StringCalculator(mockStore.Object);
            var result = calc.Add("3,4");
            mockStore.Verify(m => m.Save(), Times.Once);

        }

        public interface IStore
        {
            void Save(int result);
        }

        [TestCase("4", 4)]
        public void Add_SingleNumbers_Returns_Number(string input, int expectedResult)
        {
            StringCalculator calc = new StringCalculator();
            int result = calc.Add(input);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Add_MultipleNumbers_Returns_Sum()
        {
            StringCalculator calc = new StringCalculator();
            int result = calc.Add("1,2,5");
            int expectedResult = 8;
            Assert.AreEqual(result, expectedResult);
        }

        public class StringCalculator
        {
            private readonly IStore _store;

            public StringCalculator()
            {
                
            }

            public StringCalculator(IStore store)
            {
                _store = store;
            }

            public int Add(string s)
            {
                if (string.IsNullOrEmpty(s)) return 0;
                var total = 0;
                var numbers = s.Split(',');
                foreach(var number in numbers)
                {
                    total += int.Parse(number);
                }

                if (_store != null)
                {
                    if(IsPrime(total))
                }

                return total;
            }
        }
    }
}
*/