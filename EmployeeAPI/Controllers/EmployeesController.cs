using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            return await _context.Employees.ToListAsync();
        }


        // GET: api/Employee/GetEmployeesPaginated
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEmployeesPaginated(int? pageNum, int? pageSize)
        {

            int currPage = pageNum ?? 1;
            int currSize = pageSize ?? 5;
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employees = await _context.Employees.ToListAsync();

            return Ok(employees.Skip((currPage - 1) * currSize).Take(currSize));
        }

        // GET: api/Employee/<id>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // GET: api/Employee/order
        [HttpGet("[action]")]
        public async Task<IActionResult> Order()
        {
            var employees = await (from employee in _context.Employees
                               orderby employee.Name ascending
                               select new
                               {
                                   Id = employee.Id,
                                   Name = employee.Name,
                                   Address = employee.Address
                               }).Take(10).ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employee/searchemployee?query=<query>
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchEmployee(string query)
        {
            var employees = await (from employee in _context.Employees
                               where employee.Address.StartsWith(query)
                               select new
                               {
                                   Id = employee.Id,
                                   Name = employee.Name,
                                   Address = employee.Address
                               }).Take(10).ToListAsync();

            return Ok(employees);
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'EmployeeContext.Employees'  is null.");
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound("Database is empty");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
