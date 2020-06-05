using System.Collections.Generic;
using WhereToEat.Models;

namespace WhereToEat.Controllers
{
    public interface ICommentService
    {
        public void AddComment(CommentModel comment, bool isApproved = false);
        public List<CommentModel> GetAllCommentsForRestaurant(int restaurantId);
        public List<CommentModel> GetAllCommentsForUser(int userId);
        public void DeleteComment(int commentId);
        public List<PendingCommentModel> GetPendingComments(int restaurantOwnerId);
        public void DismissPendingComment(int commentId);
        public void AcceptPendingComment(int commentId);
        public List<PendingCommentModel> GetUsersPendingComments(int userId);
        public void DeletePending(int commentId);
    }
}