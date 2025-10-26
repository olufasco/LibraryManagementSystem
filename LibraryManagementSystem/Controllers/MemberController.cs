using Microsoft.AspNetCore.Mvc;

public class MemberController : Controller
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public IActionResult Index() => View(_memberService.GetAllMembers());

    public IActionResult Create() => View();

    [HttpPost]
    [HttpPost]
    public IActionResult Create(Member member)
    {
        if (ModelState.IsValid)
        {
            member.UserId = Guid.NewGuid().ToString(); // Auto-generate unique UserId
            _memberService.AddMember(member);
            return RedirectToAction("Index");
        }
        return View(member);
    }

    public IActionResult Details(int id) => View(_memberService.GetMemberById(id));
}