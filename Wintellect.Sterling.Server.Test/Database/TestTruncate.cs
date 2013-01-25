using System.Linq;
#if SILVERLIGHT
using Microsoft.Phone.Testing;
#endif
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Wintellect.Sterling.Test.Helpers;
using Wintellect.Sterling.Core;

namespace Wintellect.Sterling.Test.Database
{
#if SILVERLIGHT
    [Tag("Truncate")]
#endif
    [TestClass]
    public class TestTruncate
    {
        private SterlingEngine _engine;
        private ISterlingDatabaseInstance _databaseInstance;

        [TestInitialize]
        public void TestInit()
        {
            _engine = Factory.NewEngine();
            _engine.Activate();
            _databaseInstance = _engine.SterlingDatabase.RegisterDatabase<TestDatabaseInstance>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseInstance.Purge();
            _engine.Dispose();
            _databaseInstance = null;            
        }        

        [TestMethod]
        public void TestTruncateAction()
        {
            // save a few objects
            var sample = TestModel.MakeTestModel();
            _databaseInstance.Save(sample);
            _databaseInstance.Save(TestModel.MakeTestModel());
            _databaseInstance.Save(TestModel.MakeTestModel());

            _databaseInstance.Truncate(typeof(TestModel));

            // query should be empty
            Assert.IsFalse(_databaseInstance.Query<TestModel,int>().Any(), "Truncate failed: key list still exists.");

            // load should be empty
            var actual = _databaseInstance.Load<TestModel>(sample.Key);

            Assert.IsNull(actual, "Truncate failed: was able to load item.");            
        }       
    }
}