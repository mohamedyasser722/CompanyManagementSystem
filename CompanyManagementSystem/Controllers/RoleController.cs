using AutoMapper;
using CompanyManagementSystem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (!string.IsNullOrEmpty(SearchValue))
            {
                var FilteredRoles = roles.Where(r => r.NormalizedName.Contains(SearchValue.ToUpper()));
                var FilteredRoleViewModels = _mapper.Map<List<RoleViewModel>>(FilteredRoles);
                return View(FilteredRoleViewModels);
            }
            
            var roleViewModels = _mapper.Map<List<RoleViewModel>>(roles);
            return View(roleViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = _mapper.Map<IdentityRole>(roleViewModel);
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(roleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            
            var roleViewModel = _mapper.Map<RoleViewModel>(role);
            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel, [FromRoute] string id)
        {
           if (id != roleViewModel.Id)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
            if (role == null)
                return NotFound();
            _mapper.Map(roleViewModel, role);

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(roleViewModel);

        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(); // Return a success response
            }

            return BadRequest("Failed to delete role."); // Return an error response
        }


    }
}
