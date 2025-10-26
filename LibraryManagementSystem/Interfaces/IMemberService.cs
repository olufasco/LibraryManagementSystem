public interface IMemberService
{
    List<Member> GetAllMembers();
    Member GetMemberById(int id);
    Member GetByUserId(string userId);
    void AddMember(Member member);
}