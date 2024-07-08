using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.BLL.Repositories;
using CompanyManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagementSystem.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            ViewBag.message = TempData["Message"];  // notification message comes from Create action
            return View(departments);
        }
        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if(ModelState.IsValid)
            {
                await _UnitOfWork.DepartmentRepository.AddAsync(department);
                int result = await _UnitOfWork.CommitAsync();
                _UnitOfWork.Dispose();
                if (result > 0)
                {
                    TempData["Message"] = "Department added successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();    // status code 400

            var department = await _UnitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            return View(department);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _UnitOfWork.DepartmentRepository.Update(department);
                    await _UnitOfWork.CommitAsync();
                    _UnitOfWork.Dispose();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await _UnitOfWork.DepartmentRepository.GetByIdAsync(id);
            _UnitOfWork.DepartmentRepository.Delete(department);
            await _UnitOfWork.CommitAsync();
            _UnitOfWork.Dispose();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            return await Edit(id);
        }
    }
}
