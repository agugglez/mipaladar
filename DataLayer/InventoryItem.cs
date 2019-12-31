using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects.DataClasses;

namespace MiPaladar.Entities
{
    public partial class InventoryItem : EntityObject
    {
        Category category;
        bool mainCategoryFound;
        public Category Category
        {
            get
            {
                if (!mainCategoryFound)
                {
                    foreach (var item in Product.RelatedCategories)
                    {
                        if (item.IsMain) category = item.Category;
                    }

                    mainCategoryFound = true;
                }
                return category;
            }
        }
    }
}
