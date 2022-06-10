using Microsoft.VisualStudio.TestTools.UnitTesting;
using AGLChallenge;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetStringFromBadUri()
        {
            // pass bad URI into the method; should result in a 404 and therefore null string
            var result = await Program.GetJsonString("http://agl-developer-test.azurewebsites.net/hello.json");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public async Task GetStringFromGoodUri()
        {
            // pass correct URI into the method; should return a non-null string
            var result = await Program.GetJsonString("http://agl-developer-test.azurewebsites.net/people.json");

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void ConvertBadJsonString()
        {
            // Pass an invalid Json string to the method; should return null object
            string badString = "[aaabbbxxx";

            var result = Program.ConvertJsonToList(badString);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ConvertGoodJsonString()
        {
            // Pass a valid Json string to the method; should return a List<Owner>
            string goodString = "[ { \"name\" : \"Name 1\", \"gender\" : \"male\" , \"age\" : 10 , \"pets\" : [ { \"name\" : \"Petname\" , \"type\" : \"Horse\" } ]  } ]";

            var actualResult = Program.ConvertJsonToList(goodString);

            Assert.IsInstanceOfType(actualResult, typeof(List<Owner>));
        }
    }
}
