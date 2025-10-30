using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class BorrowRequestController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BorrowRequestController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var requests = await _context.BorrowRequests
            .Include(r => r.Book)
            .Include(r => r.Member)
            .ToListAsync();

        return View(requests);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        var request = await _context.BorrowRequests
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null || request.IsApproved || request.Book.AvailableCopies < 1)
        {
            TempData["Error"] = "Request invalid or book unavailable.";
            return RedirectToAction("Index");
        }

        request.IsApproved = true;
        request.DueDate = DateTime.Now.AddDays(14);
        request.Book.AvailableCopies--; // ✅ Decrease available copies
        await _context.SaveChangesAsync();
        TempData["Success"] = "Request approved.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _context.BorrowRequests.FindAsync(id);
        if (request == null)
        {
            TempData["Error"] = "Request not found.";
            return RedirectToAction("Index");
        }

        _context.BorrowRequests.Remove(request);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Request rejected.";
        return RedirectToAction("Index");
    }
}