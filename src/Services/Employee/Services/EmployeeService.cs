namespace Archz.Employee.Api.Services
{
    public interface IEmployeeService
    {
        Task<bool> IsEmployeeExist(int employeeId);
    }

    public class EmployeeService : IEmployeeService
    {

        public async Task<bool> IsEmployeeExist(int employeeId)
        {
            await Task.Delay(1000);
            if (employeeId > 1000 && employeeId < 1500)
                return true;
            else
                return false;
        }
    }
}
