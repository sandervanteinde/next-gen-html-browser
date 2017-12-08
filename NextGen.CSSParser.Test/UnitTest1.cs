using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextGen.CSSParser.Test
{
    [TestClass]
    public class AwesomeCssParserTest
    {
        [TestMethod]
        public void SimpleCase()
        {
            // Arrange
            var css = "   button{ color:   white;background-color: black; border-radius: 6px; }   ";

            // Act
            var style = new AwesomeCssParser().ParseFromString(css);

            // Assert - Number of blocks
            Assert.IsNotNull(style);
            Assert.AreEqual(1, style.Blocks.Count());

            // Assert - (block 1) Selector
            var block1 = style.Blocks.First();
            Assert.AreEqual("button", block1.Selector.TagName);
            Assert.IsNull(block1.Selector.Id);
            Assert.AreEqual(0, block1.Selector.Classes.Count());

            // Assert - (block 1) Rules
            Assert.AreEqual(3, block1.Rules.Count());
            var rule1 = block1.Rules.FirstOrDefault(r => r.Name == "color");
            var rule2 = block1.Rules.FirstOrDefault(r => r.Name == "background-color");
            var rule3 = block1.Rules.FirstOrDefault(r => r.Name == "border-radius");
            Assert.IsNotNull(rule1);
            Assert.IsNotNull(rule2);
            Assert.IsNotNull(rule3);
            Assert.AreEqual("white", rule1.Value);
            Assert.AreEqual("black", rule2.Value);
            Assert.AreEqual("6px", rule3.Value);
        }

        [TestMethod]
        public void MultiCase()
        {
            // Arrange
            var css = "div,span#block1{display:block;}";

            // Act
            var style = new AwesomeCssParser().ParseFromString(css);

            // Assert - Number of blocks
            Assert.IsNotNull(style);
            Assert.AreEqual(2, style.Blocks.Count());

            // Assert - (block 1) Selector
            var block1 = style.Blocks.First();
            Assert.AreEqual("div", block1.Selector.TagName);
            Assert.IsNull(block1.Selector.Id);
            Assert.AreEqual(0, block1.Selector.Classes.Count());

            // Assert - (block 1) Rules
            Assert.AreEqual(1, block1.Rules.Count());
            var rule1_1 = block1.Rules.FirstOrDefault(r => r.Name == "display");
            Assert.IsNotNull(rule1_1);
            Assert.AreEqual("block", rule1_1.Value);

            // Assert - (block 2) Selector
            var block2 = style.Blocks.Skip(1).First();
            Assert.AreEqual("span", block2.Selector.TagName);
            Assert.AreEqual("block1", block2.Selector.Id);
            Assert.AreEqual(0, block2.Selector.Classes.Count());

            // Assert - (block 2) Rules
            Assert.AreEqual(1, block2.Rules.Count());
            var rule2_1 = block2.Rules.FirstOrDefault(r => r.Name == "display");
            Assert.IsNotNull(rule2_1);
            Assert.AreEqual("block", rule2_1.Value);
        }

        [TestMethod]
        public void HarderCase()
        {
            // Arrange
            var css = " .large-link.display-always#link-number-1 { color:   white;background-color: black; } *{display:block;}  ";

            // Act
            var style = new AwesomeCssParser().ParseFromString(css);

            // Assert - Number of blocks
            Assert.IsNotNull(style);
            Assert.AreEqual(2, style.Blocks.Count());

            // Assert - (block 1) Selector
            var block1 = style.Blocks.First();
            Assert.IsFalse(block1.Selector.SelectAll);
            Assert.IsNull(block1.Selector.TagName);
            Assert.AreEqual("link-number-1", block1.Selector.Id);
            CollectionAssert.AreEqual(new[] { "large-link", "display-always"}, block1.Selector.Classes.ToList());

            // Assert - (block 1) Rules
            Assert.AreEqual(2, block1.Rules.Count());
            var rule1_1 = block1.Rules.FirstOrDefault(r => r.Name == "color");
            var rule1_2 = block1.Rules.FirstOrDefault(r => r.Name == "background-color");
            Assert.IsNotNull(rule1_1);
            Assert.IsNotNull(rule1_2);
            Assert.AreEqual("white", rule1_1.Value);
            Assert.AreEqual("black", rule1_2.Value);

            // Assert - (block 2) Selector
            var block2 = style.Blocks.Skip(1).First();
            Assert.IsTrue(block2.Selector.SelectAll);
            Assert.IsNull(block2.Selector.TagName);
            Assert.IsNull(block2.Selector.Id);
            CollectionAssert.AreEqual(new string[0], block2.Selector.Classes.ToList());

            // Assert - (block 2) Rules
            Assert.AreEqual(1, block2.Rules.Count());
            var rule2_1 = block2.Rules.FirstOrDefault(r => r.Name == "display");
            Assert.IsNotNull(rule2_1);
            Assert.AreEqual("block", rule2_1.Value);
        }
    }
}
