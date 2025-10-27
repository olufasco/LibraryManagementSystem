using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IBorrowService _borrowService;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(IBorrowService borrowService, UserManager<IdentityUser> userManager)
    {
        _borrowService = borrowService;
        _userManager = userManager;
    }

    public IActionResult BorrowRequests()
    {
        var requests = _borrowService.GetPendingRequests();
        return View(requests);
    }

    public IActionResult Approve(int id)
    {
        _borrowService.ApproveRequest(id);
        return RedirectToAction("BorrowRequests");
    }

    public IActionResult Members()
    {
        var members = _userManager.Users.ToList();
        return View(members);
    }
}