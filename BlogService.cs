using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog
{
	public static class BlogService
	{
		public static IEnumerable<object> NumberOfCommentsPerUser(MyDbContext context)
		{
			var comments = context.BlogComments
					.GroupBy(c => c.UserName)
					.Select(g => new { UserName = g.Key, Count = g.Count() })
					.ToList();

			return comments;
		}

		public static IEnumerable<object> NumberOfLastCommentsLeftByUser(MyDbContext context)
		{
			var lastComments = context.BlogPosts
				.Select(b => b.Comments.OrderByDescending(c => c.CreatedDate).First())
				.GroupBy(c => c.UserName)
				.Select(g => new { UserName = g.Key, Count = g.Count() })
				.ToList();

			return lastComments;
		}

		public static IEnumerable<object> PostsOrderedByLastCommentDate(MyDbContext context)
		{
			var postsOrdered = context.BlogComments
					.Include(c => c.BlogPost)
					.OrderByDescending(c => c.CreatedDate)
					.Select(g => new { g.BlogPost.Title, g.CreatedDate, g.Text })
					.ToList()
					.DistinctBy(g => g.Title);

			return postsOrdered;
		}
	}
}
