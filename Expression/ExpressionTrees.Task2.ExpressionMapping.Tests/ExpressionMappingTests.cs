using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        private readonly Foo _foo = new Foo
        {
            Zero = "Zero",
            One = "One",
            Two = "Two",
            Three = 3
        };

        [TestMethod]
        public void MapSamePropertyToProperty()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator.GenerateMapperFunc();
            var result = mapper.Map(_foo);
            Assert.AreEqual(3, result.Three);
        }

        [TestMethod]
        public void MapSameFieldToField()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator.GenerateMapperFunc();
            var result = mapper.Map(_foo);
            Assert.AreEqual("Zero", result.Zero);
        }

        [TestMethod]
        public void MapSameTypeWithCustom()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator
                .MapMember(b => b.Five, f => $"{f.Zero} {f.One}")
                .GenerateMapperFunc();
            var result = mapper.Map(new Foo
            {
                Zero = "Zero",
                One = "One"
            });
            Assert.AreEqual("Zero One", result.Five);
        }

        [TestMethod]
        public void MapSamePropertyToFiel()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator.GenerateMapperFunc();
            var result = mapper.Map(_foo);
            Assert.AreEqual("Two", result.Two);
        }

        [TestMethod]
        public void MapSameFieldToProperty()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator.GenerateMapperFunc();
            var result = mapper.Map(_foo);
            Assert.AreEqual("One", result.One);
        }

        [TestMethod]
        public void MapDifferentTypeWithCustom()
        {
            var mappingGenerator = new MappingGenerator<Foo, Bar>();
            var mapper = mappingGenerator
                .MapMember(b => b.Six, f => f.Three >= 1)
                .GenerateMapperFunc();
            var result = mapper.Map(_foo);
            Assert.AreEqual(true, result.Six);
        }
    }
}