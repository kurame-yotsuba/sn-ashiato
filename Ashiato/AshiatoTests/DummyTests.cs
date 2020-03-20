using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass()]
	public class DummyTests
	{
		[TestMethod()]
		public void SampleTest()
		{
			Assert.AreEqual("Welcome, Ashiato!", new Dummy().Welcome());
		}
	}
}