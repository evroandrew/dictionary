using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System.Collections.Generic;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(100, 50);
            dictionary.Add(10, 99);
            Assert.AreEqual(dictionary.Count, 2);
        }

        [TestMethod]
        public void AddPairTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(new KeyValuePair<int, int>(10, 25));
            dictionary.Add(new KeyValuePair<int, int>(100, 50));
            dictionary.Add(new KeyValuePair<int, int>(10, 99));
            Assert.AreEqual(dictionary.Count, 2);
        }

        [TestMethod]
        public void ClearTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(100, 50);
            dictionary.Add(10, 99);
            Assert.AreEqual(dictionary.Count, 2);
            dictionary.Clear();
            Assert.AreEqual(dictionary.Count, 0);
        }

        [TestMethod]
        public void IndexTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            Assert.AreEqual(dictionary[10], 25);

            dictionary.Add(15, 50);
            dictionary.Add(100, 99);
            Assert.AreEqual(dictionary[15], 50);
            Assert.AreEqual(dictionary[100], 99);

            dictionary[15] = 150;
            Assert.AreEqual(dictionary[15], 150);

            dictionary[250] = 35;
            Assert.AreEqual(dictionary[250], 35);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IndexExceptionTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            int res = dictionary[0];
        }

        [TestMethod]
        public void AllKeysTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(100, 50);
            dictionary.Add(10, 99);

            ICollection<int> keys = dictionary.Keys;
            Assert.IsTrue(keys.Contains(10));
            Assert.IsTrue(keys.Contains(100));
            Assert.IsFalse(keys.Contains(0));
        }

        [TestMethod]
        public void AllValuesTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(100, 50);
            dictionary.Add(10, 99);

            ICollection<int> values = dictionary.Values;
            Assert.IsTrue(values.Contains(50));
            Assert.IsTrue(values.Contains(99));
            Assert.IsFalse(values.Contains(25));
        }

        [TestMethod]
        public void ContanceTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(15, 50);
            dictionary.Add(20, 99);
            Assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(10, 25)));
            Assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(15, 50)));
            Assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(20, 99)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(10, 30)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(15, 90)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(20, 79)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(0, 79)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(1, 79)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(100, 79)));
        }
        [TestMethod]
        public void ContanceKeyTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(15, 50);
            dictionary.Add(20, 99);
            Assert.IsTrue(dictionary.ContainsKey(10));
            Assert.IsTrue(dictionary.ContainsKey(15));
            Assert.IsTrue(dictionary.ContainsKey(20));
            Assert.IsFalse(dictionary.ContainsKey(50));
            Assert.IsFalse(dictionary.ContainsKey(0));
        }
        [TestMethod]
        public void CopyToTest()
        {
            KeyValuePair<int, int>[] array = new KeyValuePair<int, int>[10];

            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(15, 50);
            dictionary.Add(20, 99);
            dictionary.CopyTo(array, 5);
            Assert.AreEqual(array[5], new KeyValuePair<int, int>(10, 25));
            Assert.AreEqual(array[6], new KeyValuePair<int, int>(15, 50));
            Assert.AreEqual(array[7], new KeyValuePair<int, int>(20, 99));
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(15, 50);
            dictionary.Add(20, 99);
            int value;
            Assert.IsTrue(dictionary.TryGetValue(10, out value));
            Assert.AreEqual(value, 25);
            Assert.IsTrue(dictionary.TryGetValue(15, out value));
            Assert.AreEqual(value, 50);
            Assert.IsTrue(dictionary.TryGetValue(20, out value));
            Assert.AreEqual(value, 99);
            Assert.IsFalse(dictionary.TryGetValue(0, out value));
            Assert.IsFalse(dictionary.TryGetValue(11, out value));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void RemoveKeyTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary.Add(10, 25);
            dictionary.Add(15, 50);
            dictionary.Add(20, 99);
            dictionary.Add(5, 25);
            dictionary.Add(7, 50);
            dictionary.Add(2, 99);
            Assert.AreEqual(dictionary[15], 50);
            Assert.AreEqual(dictionary[10], 25);
            Assert.AreEqual(dictionary[20], 99);
            Assert.IsTrue(dictionary.Remove(15));
            Assert.IsFalse(dictionary.Remove(99));
            Assert.IsTrue(dictionary.Remove(2));
            Assert.IsTrue(dictionary.Remove(5));
            Assert.IsTrue(dictionary.Remove(10));

            dictionary.Add(10, 25);
            dictionary.Add(20, 99);
            dictionary.Add(5, 25);
            dictionary.Add(7, 50);
            dictionary.Add(2, 99);

            Assert.IsTrue(dictionary.Remove(5));
            Assert.IsTrue(dictionary.Remove(10));

            Assert.AreEqual(dictionary[10], 25);
            Assert.AreEqual(dictionary[20], 99);
            int val = dictionary[15];
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            MyDictionary<int, int> dictionary = new MyDictionary<int, int>();
            dictionary[100] = 100;
            dictionary[55] = 55;
            dictionary[200] = 200;
            dictionary[41] = 41;
            dictionary[35] = 35;
            dictionary[45] = 45;
            dictionary[110] = 110;
            dictionary[210] = 210;

            int tmp = 0;
            foreach (var item in dictionary)
            {
                Assert.IsTrue(tmp < item.Key && tmp < item.Value && item.Key == item.Value);
                tmp = item.Key;
            }
        }
    }
}
