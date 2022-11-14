using Complex.LinqProvider.Provider;
using System.Linq.Expressions;
using Complex.LinqProvider.Entities;

namespace LinqProvider.Tests
{
    public class ProviderTranslatorTests
    {
        [Fact]
        public void TranslateSelect()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Country";
            var expression = Expression.Constant(new EntitySet<Country>(string.Empty));
            var translator = new ExpressionToSqlTranslator();
            
            //Act
            var result = translator.Translate(expression);
            
            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereEqualNumber()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE OfficeId = 1";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.OfficeId == 1);
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereEqualString()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE FirstName = 'Bill'";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.FirstName == "Bill");
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereNotEqual()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE FirstName <> 'Bill'";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.FirstName != "Bill");
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereGreaterThan()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE OfficeId > 1";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.OfficeId > 1);
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereGreaterOrEqualThan()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE OfficeId >= 1";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.OfficeId >= 1);
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }
        [Fact]
        public void TranslateWhereLowerThan()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE OfficeId < 1";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.OfficeId < 1);
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }
        [Fact]
        public void TranslateWhereLowerOrEqualThan()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE OfficeId <= 1";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.OfficeId <= 1);
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }

        [Fact]
        public void TranslateWhereEqualAnd()
        {
            //Arrange
            var expectedExpression = "SELECT * FROM Employee WHERE FirstName = 'Bill' AND LastName = 'Gates'";
            Expression<Func<IQueryable<Employee>, IQueryable<Employee>>> expression =
                materials => materials.Where(e => e.FirstName == "Bill" && e.LastName == "Gates");
            var translator = new ExpressionToSqlTranslator();

            //Act
            var result = translator.Translate(expression);

            //Assert
            Assert.Equal(expectedExpression, result);
        }
    }
}
