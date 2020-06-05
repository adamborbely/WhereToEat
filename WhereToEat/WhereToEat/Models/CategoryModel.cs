using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToEat.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public CategoryModel() { }
        public CategoryModel(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public CategoryModel(CategoryModel category)
        {
            Id = category.Id;
            Text = category.Text;
        }
    }
}
