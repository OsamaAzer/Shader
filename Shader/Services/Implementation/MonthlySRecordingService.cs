using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MonthlySalaryRecording;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MonthlySRecordingService(ShaderContext context) : IMonthlySRecordingService
    {
        public async Task<PagedResponse<RMonthlySRecordingDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var monthlySRecordings = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            return monthlySRecordings.ToRMonthlySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlySRecordingDto>> GetAllByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var monthlySRecordings = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .Where(m => m.Date >= startDate && m.Date <= endDate && !m.IsDeleted)
                .ToListAsync();

            return monthlySRecordings.ToRMonthlySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlySRecordingDto>> GetAllByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            var monthlySRecordings = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .Where(m => m.EmployeeId == employeeId && !m.IsDeleted)
                .ToListAsync();

            return monthlySRecordings.ToRMonthlySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlySRecordingDto>> GetAllByEmployeeIdAndDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var monthlySRecordings = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .Where(m => m.EmployeeId == employeeId && m.Date >= startDate && m.Date <= endDate && !m.IsDeleted)
                .ToListAsync();

            return monthlySRecordings.ToRMonthlySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RMonthlySRecordingDto> GetByIdAsync(int id)
        {
            var monthlySRecording = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted) ??
                throw new Exception($"Monthly Salary Recording with ID {id} not found.");

            return monthlySRecording.ToRMonthlySRecordingDto();
        }

        public async Task<IEnumerable<RMonthlySRecordingDto>> AddRangeAsync(List<int> employees, int month)
        {
            if (employees == null || !employees.Any())
                throw new ArgumentException("Employee list cannot be null or empty.", nameof(employees));

            var monthlySRecordings = new List<MonthlySalaryRecording>();
            foreach (var employeeId in employees)
            {
                var employee = await context.MonthlyEmployees
                    .FirstOrDefaultAsync(m => m.Id == employeeId && !m.IsDeleted) ??
                    throw new Exception($"Employee with ID {employeeId} not found.");

                bool isAlreadyRecorded = await context.MonthlyEmpSalaryRecordings
                    .AnyAsync(m => m.EmployeeId == employeeId && m.Date.Month == month && m.Date.Year == DateTime.Now.Year && !m.IsDeleted);

                if (isAlreadyRecorded)
                    throw new Exception($"Monthly Salary Recording for Employee  {employee.Name} for month {month} already exists.");

                var daySalaryInCurrentMonth = employee.BaseSalary / DateTime.DaysInMonth(DateTime.Now.Year, month);

                var numberOfAbsentDaysInCurrentMonth = await context.DailyEmpAbsences
                    .Where(a => a.EmployeeId == employeeId && a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && !a.IsDeleted)
                    .CountAsync();

                var monthlySRecordingDto = new WMonthlySRecordingDto();
                monthlySRecordingDto.EmployeeId = employeeId;
                var monthlySRecording = monthlySRecordingDto.ToMonthlySRecording();
                monthlySRecording.DeductionAmount = numberOfAbsentDaysInCurrentMonth * daySalaryInCurrentMonth; // Calculate deduction based on absent days
                monthlySRecording.Employee = employee;
                monthlySRecording.Employee.BorrowedAmount = 0; // Reset borrowed amount of the employee
                monthlySRecordings.Add(monthlySRecording);
            }
            await context.MonthlyEmpSalaryRecordings.AddRangeAsync(monthlySRecordings);
            await context.SaveChangesAsync();
            return monthlySRecordings.ToRMonthlySRecordingDtos();

        }

        public async Task<RMonthlySRecordingDto> UpdateAsync(int id, WMonthlySRecordingDto monthlySRecordingDto)
        {
            var monthlySRecording = await context.MonthlyEmpSalaryRecordings
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted) ??
                throw new Exception($"Monthly Salary Recording with ID {id} not found.");

            var newEmployee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(m => m.Id == monthlySRecordingDto.EmployeeId && !m.IsDeleted) ??
                throw new Exception($"Employee with ID {monthlySRecordingDto.EmployeeId} not found.");

            var removedEmployee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(m => m.Id == monthlySRecording.EmployeeId && !m.IsDeleted);

            if (removedEmployee != null)
            {
                removedEmployee.BorrowedAmount += monthlySRecording.BorrowedAmount; // Adjust borrowed amount of the removed employee
                context.MonthlyEmployees.Update(removedEmployee);
            }
            newEmployee.BorrowedAmount = 0; // Reset borrowed amount of the new employee
            context.MonthlyEmployees.Update(newEmployee);
            monthlySRecording = monthlySRecordingDto.ToMonthlySRecording(monthlySRecording);
            context.MonthlyEmpSalaryRecordings.Update(monthlySRecording);
            await context.SaveChangesAsync();
            return monthlySRecording.ToRMonthlySRecordingDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var monthlySRecording = await context.MonthlyEmpSalaryRecordings.FindAsync(id) ??
                throw new Exception($"Monthly Salary Recording with ID {id} not found.");

            var removedEmployee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(m => m.Id == monthlySRecording.EmployeeId && !m.IsDeleted);

            if (removedEmployee != null)
            {
                removedEmployee.BorrowedAmount += monthlySRecording.BorrowedAmount; // Adjust borrowed amount of the removed employee
                context.MonthlyEmployees.Update(removedEmployee);
            }

            monthlySRecording.IsDeleted = true;
            context.MonthlyEmpSalaryRecordings.Update(monthlySRecording);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
