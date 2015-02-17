using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace proactima.jsonobject.tests
{
    [TestClass]
    public class DescribeJsonObject
    {
        [TestMethod]
        public void ItCanInstantiate()
        {
            // g
            // w
            var json = new JsonObject();

            // t
            Assert.AreSame(json.Id, "0");
        }
    }
}