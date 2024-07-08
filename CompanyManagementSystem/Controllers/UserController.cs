using AutoMapper;
using CompanyManagementSystem.DAL.Models;
using CompanyManagementSystem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        //fetch roles with user (multiple Active results) can cause performance issues
        /*
        public async Task<IActionResult> Index(string SearchValue)
        {
                var users = await _userManager.Users.Select(  u => new UserViewModel
                {
                    Id = u.Id,
                    Fname = u.FName,
                    Lname = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToListAsync();
            if(!string.IsNullOrEmpty(SearchValue))
            {
                // search by email and make suere u make the email to lower case
                var FilteredUsers = users.Where(u => u.Email.ToLower().Contains(SearchValue.ToLower()));
                return View(FilteredUsers);
            }
            return View(users);
        }
        */

        //fetch roles with user (single Active result)
        // best practice to use this method
        public async Task<IActionResult> Index(string? SearchValue)
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            foreach (var user in userViewModels)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = await _userManager.GetRolesAsync(appUser);
            }
            if (!string.IsNullOrEmpty(SearchValue))
            {
                // Search by email and ensure the email is in lower case
                var filteredUsers = userViewModels.Where(u => u.Email.ToLower().Contains(SearchValue.ToLower()));
                return View(filteredUsers);
            }
            return View(userViewModels);
        }


        public async Task<IActionResult> Details(string id)
        {

            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _userManager.GetRolesAsync(user);

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userViewModel = _mapper.Map<UserViewModel>(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userViewModel.AvailableRoles[role] = true;
        }

            var allRoles = await _roleManager.Roles.ToListAsync();
            foreach (var role in allRoles)
            {
                if (!userViewModel.AvailableRoles.ContainsKey(role.Name))
                {
                    userViewModel.AvailableRoles[role.Name] = false;
                }
            }

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user == null) return NotFound();

            _mapper.Map(userViewModel, user);
            user.UserName = userViewModel.Email.Split('@')[0];
            
            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRoles = userViewModel.AvailableRoles.Where(r => r.Value).Select(r => r.Key).ToList();
            var rolesToAdd = newRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(newRoles);

            var addRoleResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addRoleResult.Succeeded)
            {
                foreach (var error in addRoleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userViewModel);
            }

            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeRoleResult.Succeeded)
            {
                foreach (var error in removeRoleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userViewModel);
            }
            
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userViewModel);
        }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _userManager.GetRolesAsync(user);
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel model ,[FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return NotFound();
            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return RedirectToAction("Index");
           
        }







    }
}
