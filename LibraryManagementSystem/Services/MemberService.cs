public class MemberService : IMemberService
{
    private readonly List<Member> _members = new();
    private int _nextId = 1;

    public List<Member> GetAllMembers() => _members;
    public Member GetMemberById(int id) => _members.FirstOrDefault(m => m.Id == id);
    public Member GetByUserId(string userId) => _members.FirstOrDefault(m => m.UserId == userId);
    public void AddMember(Member member)
    {
        member.Id = _nextId++;
        _members.Add(member);
    }
}