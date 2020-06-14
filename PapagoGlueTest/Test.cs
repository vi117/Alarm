using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapagoGlue;

namespace Test {
	[TestClass]
	public class Test {

		[TestMethod]
		public void TestTranslate() {
			var t = "안녕, 월드!";
			var p = new PapagoGlue.PapagoGlue();
			var s = p.Translate("Hello, World!").Result;

			Assert.AreEqual(s, t);
		}
	}
}
