namespace LibraryManagementSystem.Interfaces
{
    public interface IBorrowService
    {
        void SubmitRequest(BorrowRequest request);
        List<BorrowRequest> GetPendingRequests();
        void ApproveRequest(int requestId);
    }
}
