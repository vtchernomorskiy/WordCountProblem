using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountLibrary.Interfaces;
using Moq;
using NUnit;
using NUnit.Framework;
using WordCountUtilities;
using Assert = NUnit.Framework.Assert;
using CollectionAssert = NUnit.Framework.CollectionAssert;

namespace WordCountLibrary.Unit
{
    [TestClass]
    public class WordCounterTests
    {
        private WordCount[] wc1 = { new WordCount { Word = "HELLO", Count = 5 } };
        private WordCount[] wc2 = { new WordCount { Word = "HELLO,", Count = 4 }, new WordCount { Word = "HELLO!", Count = 1 } };

        [TestCase(null, ExpectedException=typeof(InputStatementValidationException))]
        [TestCase("", ExpectedException=typeof(InputStatementValidationException))]
        [TestCase("     ", ExpectedException = typeof(InputStatementValidationException))]
        [TestCase("  valid  ")]
        public void ValidateString(string str)
        {
            var res = str.Validate();
            Assert.IsTrue(res.Equals("valid"));
        }

        [TestCase(false, new [] { "Hello,", "I", "am", "Slava!"})]
        [TestCase(true, new[] { "Hello", "I", "am", "Slava" })]
        public void GivenStatementAndWordDictionaryCheckStatementWordSplit(bool removePunctuation, string[] wordDictionary)
        {
            var res = "Hello, I am Slava!".ToWordArray(removePunctuation);
            CollectionAssert.AllItemsAreInstancesOfType(res, typeof(string));
            CollectionAssert.AllItemsAreNotNull(res);
            CollectionAssert.AreEquivalent(res, wordDictionary);
        }

        [TestCase(null, ExpectedException = typeof(InputStatementValidationException))]
        public void ValidateWords(string[] words)
        {
            var res = words.Validate();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void GivenStatementAndWordCountDictionaryCheckCountedWordsInStatement(bool removePunctuation)
        {
            var wc = removePunctuation ? wc1 : wc2;
            var res = "HELLO, HELLO, HELLO, HELLO, HELLO!"
                .ToWordArray(removePunctuation)
                .CountWordOccurences();
            CollectionAssert.AllItemsAreInstancesOfType(res, typeof(WordCount));
            CollectionAssert.AllItemsAreNotNull(res);
            CollectionAssert.AreEquivalent(res, wc);
        }

        [TestMethod]
        public void CountWordsWithoutRemovalOfPunctuation()
        {
            var wc = new WordCounter(new Mock<ILogger>().Object, new Mock<IConfig>().Object);
            var res = wc.CountWordsInStatement("Hello, I... am... Slava!", false);
            Assert.IsNotNullOrEmpty(res.Statement);
            Assert.IsFalse(res.HasError);
            Assert.IsNull(res.Error);
            Assert.IsTrue("Hello, I... am... Slava!".Equals(res.Statement));
            Assert.IsTrue(res.ToString().Contains("HELLO, - 1"));
            Assert.IsTrue(res.ToString().Contains("I... - 1"));
            Assert.IsTrue(res.ToString().Contains("AM... - 1"));
            Assert.IsTrue(res.ToString().Contains("SLAVA! - 1"));
        }

        [TestMethod]
        public void CountWordsWithRemovalOfPunctuation()
        {
            var wc = new WordCounter(new Mock<ILogger>().Object, new Mock<IConfig>().Object);
            var res = wc.CountWordsInStatement("Hello, I am Slava!", true);
            Assert.IsNotNullOrEmpty(res.Statement);
            Assert.IsFalse(res.HasError);
            Assert.IsNull(res.Error);
            Assert.IsTrue("Hello, I am Slava!".Equals(res.Statement));
            Assert.IsTrue(res.ToString().Contains("HELLO - 1"));
            Assert.IsTrue(res.ToString().Contains("I - 1"));
            Assert.IsTrue(res.ToString().Contains("AM - 1"));
            Assert.IsTrue(res.ToString().Contains("SLAVA - 1"));
        }
    }
}
