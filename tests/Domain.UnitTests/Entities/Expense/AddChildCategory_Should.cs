using Firewatch.Domain.Entities;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.UnitTests.Entities.Expense
{
    public class AddChildCategory_Should
    {
        [Test]
        public void AddChildToChildrenCategories()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent");

            parent.AddChildCategory("child", 10m);

            parent.ChildrenCategories.Count.ShouldBe(1);
            parent.ChildrenCategories.Any(c => c.Label == "child").ShouldBeTrue();
        }

        [Test]
        public void IgnoreDuplicateChildrenCategories()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent");

            parent.AddChildCategory("child", 10m);
            parent.AddChildCategory("child", 10m);
            parent.AddChildCategory("child", 10m);
            parent.AddChildCategory("child", 10m);

            parent.ChildrenCategories.Count.ShouldBe(1);
        }

        [Test]
        public void NotSetAnIconForChild()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent");

            parent.AddChildCategory("child", 10m);

            parent.ChildrenCategories.First(c => c.Label == "child").Icon.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void SetChildColourTheSameAsParent()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent", 0.00m, "#FFFFFF");

            parent.AddChildCategory("child", 10m);

            parent.ChildrenCategories.First(c => c.Label == "child").Color.ShouldBe(ColorTranslator.FromHtml("#FFFFFF"));
        }

        [Theory]
        [TestCase(10.00)]
        public void SetChildBudget(decimal expectedBudget)
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent", 0.00m, "#FFFFFF");

            parent.AddChildCategory("child", expectedBudget);

            parent.ChildrenCategories.First(c => c.Label == "child").MonthlyBudget.ShouldBe(expectedBudget);
        }

        [Test]
        public void NotChangeParentBudgetWhenChildrenSumToLess()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent", 30.00m, "#FFFFFF");

            parent.AddChildCategory("child", 10m);

            parent.MonthlyBudget.ShouldBe(30.00m);
        }

        [Test]
        public void AdjustParentBudgetWhenChildrenSumToMore()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            var parent = new ExpenseCategory(person, "parent", 10.00m, "#FFFFFF");

            parent.AddChildCategory("child", 30m);

            parent.MonthlyBudget.ShouldBe(30.00m);
        }

    }
}
