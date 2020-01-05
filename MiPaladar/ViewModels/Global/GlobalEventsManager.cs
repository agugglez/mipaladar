using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class GlobalEventsManager
    {
        #region Category Events

        public event EventHandler<CategoryInfoEventArgs> CategoryCreated;
        public event EventHandler<CategoryInfoEventArgs> CategoryRemoved;
        public event EventHandler<CategoryInfoEventArgs> CategoryModified;

        public void FireCategoryCreated(int id)
        {
            EventHandler<CategoryInfoEventArgs> handler = this.CategoryCreated;
            if (handler != null)
            {
                handler(this, new CategoryInfoEventArgs(id));
            }
        }

        public void FireCategoryRemoved(int id)
        {
            EventHandler<CategoryInfoEventArgs> handler = this.CategoryRemoved;
            if (handler != null)
            {
                handler(this, new CategoryInfoEventArgs(id));
            }
        }

        public void FireCategoryModified(int id)
        {
            EventHandler<CategoryInfoEventArgs> handler = this.CategoryModified;
            if (handler != null)
            {
                handler(this, new CategoryInfoEventArgs(id));
            }
        }

        #endregion

        #region Tag Events

        public event EventHandler<TagInfoEventArgs> TagCreated;
        public event EventHandler<TagInfoEventArgs> TagRemoved;
        public event EventHandler<TagInfoEventArgs> TagModified;

        public void FireTagCreated(int tagId)
        {
            EventHandler<TagInfoEventArgs> handler = this.TagCreated;
            if (handler != null)
            {
                handler(this, new TagInfoEventArgs(tagId));
            }
        }

        public void FireTagRemoved(int tagId)
        {
            EventHandler<TagInfoEventArgs> handler = this.TagRemoved;
            if (handler != null)
            {
                handler(this, new TagInfoEventArgs(tagId));
            }
        }

        public void FireTagModified(int tagId)
        {
            EventHandler<TagInfoEventArgs> handler = this.TagModified;
            if (handler != null)
            {
                handler(this, new TagInfoEventArgs(tagId));
            }
        }

        #endregion

        #region Product Events

        public event EventHandler<ProductInfoEventArgs> ProductCreated;
        public event EventHandler<ProductInfoEventArgs> ProductRemoved;
        public event EventHandler<ProductInfoEventArgs> ProductModified;

        public void FireProductCreated(int prodId)
        {
            EventHandler<ProductInfoEventArgs> handler = this.ProductCreated;
            if (handler != null)
            {
                handler(this, new ProductInfoEventArgs(prodId));
            }
        }

        public void FireProductRemoved(int prodId)
        {
            EventHandler<ProductInfoEventArgs> handler = this.ProductRemoved;
            if (handler != null)
            {
                handler(this, new ProductInfoEventArgs(prodId));
            }
        }

        public void FireProductModified(int prodId)
        {
            EventHandler<ProductInfoEventArgs> handler = this.ProductModified;
            if (handler != null)
            {
                handler(this, new ProductInfoEventArgs(prodId));
            }
        }

        #endregion
    }

    public class CategoryInfoEventArgs : EventArgs
    {
        public CategoryInfoEventArgs(int category_id)
        {
            CategoryId = category_id;
        }

        public int CategoryId { get; private set; }
    }

    public class TagInfoEventArgs : EventArgs
    {
        public TagInfoEventArgs(int tagId)
        {
            TagId = tagId;
        }

        public int TagId { get; private set; }
    }

    public class ProductInfoEventArgs : EventArgs
    {
        public ProductInfoEventArgs(int prodId)
        {
            ProductId = prodId;
        }

        public int ProductId { get; set; }
    }
}
