using System.Collections.Generic;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public interface ICommentService
    {
        public List<CommentModel> GetAllCommentsForUser(int userId);
    }
}