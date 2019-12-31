using MiPaladar.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MiPaladar.Entities;

namespace MiPaladarTestProject
{
    
    
    /// <summary>
    ///This is a test class for SaleViewModelTest and is intended
    ///to contain all SaleViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SaleViewModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for NewLineItem
        ///</summary>
        [TestMethod()]
        public void NewLineItemTest()
        {
            MainWindowViewModel appvm = null; // TODO: Initialize to an appropriate value
            Sale salesOrder = null; // TODO: Initialize to an appropriate value
            Action<Sale> onRemoved = null; // TODO: Initialize to an appropriate value
            SaleViewModel target = new SaleViewModel(appvm, salesOrder, onRemoved); // TODO: Initialize to an appropriate value
            double qtty_to_add = 0F; // TODO: Initialize to an appropriate value
            Product product_to_add = null; // TODO: Initialize to an appropriate value
            target.NewLineItem(qtty_to_add, product_to_add);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
