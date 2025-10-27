using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;



namespace LibraryManagementSystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly ApplicationDbContext _context;

        public BorrowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ApproveRequest(int requestId)
        {
            throw new NotImplementedException();
        }

        public List<BorrowRequest> GetPendingRequests()
        {
            return _context.BorrowRequests
                .Include(r => r.Book)
                .Include(r => r.Member)
                .Where(r => !r.IsApproved)
                .ToList();
        }

        public void SubmitRequest(BorrowRequest request)
        {
            throw new NotImplementedException();
        }

        // Implement methods here...
    }
}
