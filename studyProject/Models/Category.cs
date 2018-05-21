using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TreeUtility;

namespace studyProject.Models
{
    public class Category : ITreeNode<Category>
    {
        
        [Column("CategoryId")]
        public int Id { get; set; }
        private int? _ParentCategoryId;
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId {
            get { return _ParentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");
                _ParentCategoryId = value;
            }
        }

        [Display(Name = "Category")]
        [StringLength(20, ErrorMessage = "Category names must be 20 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a category name.")]
        [Index("AK_Category_CategoryName", IsUnique = true)]
        public string CategoryName { get; set; }

        public IList<Category> Children { get; set; }
          

        

        public virtual Category Parent { get; set; }
        
    }
}