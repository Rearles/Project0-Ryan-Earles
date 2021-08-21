﻿using System;
using System.Collections.Generic;

namespace DL.Entities
{
    public partial class Review
    {
        public Review()
        {
            Reviews = new HashSet<Review>();
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public decimal Rating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}