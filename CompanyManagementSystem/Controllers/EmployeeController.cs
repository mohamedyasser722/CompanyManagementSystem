using AutoMapper;
using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.BLL.Repositories;
using CompanyManagementSystem.DAL.Models;
using CompanyManagementSystem.PL.Utilities;
using CompanyManagementSystem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace CompanyManagementSystem.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }
            
        public async Task<IActionResult> Index()
        {
            var employees = await _UnitOfWork.EmployeeRepository.GetAllAsync(e => e.Department);
            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            ViewBag.Message = TempData["Message"];



            return View(mappedEmployees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                string ImagePath = ImageHelper.UploadFile(employeeVM.ImageFile, "Images");

                if(! string.IsNullOrEmpty(ImagePath))
                {
                    employeeVM.ImagePath = ImagePath;
                }
                var Mappedemployee = _mapper.Map<EmployeeViewModel , Employee>(employeeVM);
                await _UnitOfWork.EmployeeRepository.AddAsync(Mappedemployee);
                int result = await _UnitOfWork.CommitAsync(); 
                _UnitOfWork.Dispose();
                if(result > 0)
                    TempData["Message"] = "Employee added successfully";

                return RedirectToAction("Index");
            }
            ViewBag.Departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            return View(employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _UnitOfWork.EmployeeRepository.GetByIdAsync(id);
            if(employee == null)
               return NotFound();
           
            ViewBag.Departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            //mappedEmployee.ImageFile = ImageHelper.GetImageAsFormFile(mappedEmployee.ImagePath);

            return View(mappedEmployee);
        }
        //[HttpPost]
        //public async Task<IActionResult> Edit(EmployeeViewModel employeeVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var oldEmployee = await _UnitOfWork.EmployeeRepository.GetByIdAsync(employeeVM.Id);

        //        if (oldEmployee != null)
        //        {
        //            if (oldEmployee.ImagePath != null && employeeVM.ImageFile != null)
        //            {
        //                ImageHelper.DeleteFile(oldEmployee.ImagePath,"Images");
        //            }

        //            // Detach the old employee from the context
        //            _UnitOfWork.EmployeeRepository.Detach(oldEmployee);

        //            string NewImagePath = ImageHelper.UploadFile(employeeVM.ImageFile, "Images");

        //            if (!string.IsNullOrEmpty(NewImagePath))
        //            {
        //                employeeVM.ImagePath = NewImagePath;
        //            }

        //            var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
        //            _UnitOfWork.EmployeeRepository.Update(mappedEmployee);
        //            _UnitOfWork.CommitAsync();
        //            _UnitOfWork.Dispose();

        //            return RedirectToAction("Index");
        //        }
        //    }

        //    ViewBag.Departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
        //    return View(employeeVM);
        //}
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var oldEmployee = await _UnitOfWork.EmployeeRepository.GetByIdAsync(employeeVM.Id);

                if (oldEmployee != null)
                {
                    if (employeeVM.ImageFile != null)   // if you update the image
                    {
                        if (oldEmployee.ImagePath != null)  // if there was an old image, delete it
                        {
                            ImageHelper.DeleteFile(oldEmployee.ImagePath, "Images");
                        }

                        string newImagePath = ImageHelper.UploadFile(employeeVM.ImageFile, "Images");
                        if (!string.IsNullOrEmpty(newImagePath))
                        {
                            employeeVM.ImagePath = newImagePath;
                        }
                    }
                    else
                    {
                        // Preserve the existing image path if a new image is not uploaded
                        employeeVM.ImagePath = oldEmployee.ImagePath;
                    }

                    // Detach the old employee from the context
                    _UnitOfWork.EmployeeRepository.Detach(oldEmployee);

                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _UnitOfWork.EmployeeRepository.Update(mappedEmployee);
                    await _UnitOfWork.CommitAsync();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.Departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            return View(employeeVM);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _UnitOfWork.EmployeeRepository.GetByIdAsync(id);
            if(employee == null)
               return NotFound();

            // delete the employee from the database
            _UnitOfWork.EmployeeRepository.Delete(employee);
            var deleted = await _UnitOfWork.CommitAsync();
            _UnitOfWork.Dispose();

            // delete the image from the server
            if(employee.ImagePath != null && deleted > 0)
            {
                ImageHelper.DeleteFile(employee.ImagePath, "Images");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _UnitOfWork.EmployeeRepository.GetByIdAsync(id, e => e.Department);
            if(employee == null)
               return NotFound();
            
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(mappedEmployee);
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var employees = await _UnitOfWork.EmployeeRepository.GetAllAsync(e => e.Department);
            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            if (!string.IsNullOrEmpty(searchString))
            {
                mappedEmployees = mappedEmployees.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EmployeeSearchTableBody", mappedEmployees);
            }

            return View(nameof(Index), mappedEmployees);
        }

    }
}
