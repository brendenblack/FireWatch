using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Firewatch.Domain.Entities
{
    public class ExpenseCategory
    {
        public ExpenseCategory() { }

        public ExpenseCategory(Person owner, string label, decimal budget = 0.00m, string colorHexCode = "")
        {
            Owner = owner;
            OwnerId = owner?.Id;
            Label = label;
            MonthlyBudget = budget;
            Color = ColorTranslator.FromHtml(colorHexCode);
        }

        public int? ParentCategoryId { get; set; }
        public ExpenseCategory ParentCategory { get; set; }

        public IReadOnlyCollection<ExpenseCategory> ChildrenCategories { get; set; } = new List<ExpenseCategory>();

        public bool IsParent => !ParentCategoryId.HasValue;
        public bool HasChildren => ChildrenCategories.Count > 0;

        /// <summary>
        /// Adds the supplied label as a child category. If the label already exists as a child, it is ignored.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="budget"></param>
        public void AddChildCategory(string label, decimal budget = 0.00m)
        {
            if (ChildrenCategories.Any(c => c.Label.Equals(label, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var child = new ExpenseCategory(Owner, label, budget, ColorTranslator.ToHtml(Color));
            child.ParentCategory = this;
            child.ParentCategoryId = Id;

            ((List<ExpenseCategory>)ChildrenCategories).Add(child);

            var childrenBudget = ChildrenCategories.Select(c => c.MonthlyBudget).Sum();
            if (childrenBudget > MonthlyBudget)
            {
                MonthlyBudget = childrenBudget;
            }
        }

        public int Id { get; set; }

        public Person Owner { get; private set; }

        public string OwnerId { get; private set; }

        public string Label { get; set; }

        public Color Color { get; set; }

        public string Icon { get; set; }

        private decimal _monthlyBudget;
        /// <summary>
        /// The allotment for this category within a month's budget.
        /// </summary>
        /// <remarks>
        /// This value must be equal to or larger than the sum of the <see cref="ChildrenCategories"/>' budgets.
        /// </remarks>
        public decimal MonthlyBudget
        {
            get => _monthlyBudget;
            set
            {
                if (value >= ChildrenCategories.Select(c => c.MonthlyBudget).Sum())
                {
                    _monthlyBudget = value;
                }
            }
        }

        public override string ToString()
        {
            return $"${Label}: ${string.Format("{0:C}", MonthlyBudget)} [Owner: {OwnerId}]";
        }
    }
}
