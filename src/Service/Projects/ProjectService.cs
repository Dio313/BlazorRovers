using Common.ViewModels;
using Common.Wrapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Service.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepo;
       
        public ProjectService(IRepository<Project> projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public Task<PagedResult<ProjectVm>> GetProjects(string name, int page)
        {
            int pageSize = 10;

            if (name is not null)
            {
                return Task.FromResult(_projectRepo.TableNoTracking.Include(p =>p.Employee)
                   .Where(p => p.Name.Contains(name))
                    .Select(b => new ProjectVm()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Amount = b.Amount,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        EmployeeName = b.Employee.FirstName + new String('\n', 10) +  b.Employee.LastName +
                                       new String('\n', 10) + b.Employee.EmployeeNumber
                        

                    }).OrderBy(e => e.Name).GetPaged(page, pageSize));
            }
            return Task.FromResult(_projectRepo.TableNoTracking
                .Select(b => new ProjectVm()
                {
                        
                    Id = b.Id,
                    Name = b.Name,
                    Amount = b.Amount,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    EmployeeName = b.Employee.FirstName + new String('\n', 10) +  b.Employee.LastName +
                                   new String('\n', 10) + b.Employee.EmployeeNumber
                    
                }).OrderBy(e => e.Name).GetPaged(page, pageSize));
        }


        public async Task<ApiResultResponse<ProjectVm>> GetByIdAsync(string id)
        {
            try
            {
                var project = await (from p in _projectRepo.TableNoTracking
                    where p.Id == id
                    select new ProjectVm()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        Amount = p.Amount,
                        EmployeeName = p.Employee.FirstName + new String('\n', 10) +  p.Employee.LastName +
                                       new String('\n', 10) + p.Employee.EmployeeNumber
                    }).FirstOrDefaultAsync();
                return new ApiResultResponse<ProjectVm>()
                {
                    Data = project
                };
            }
            catch (Exception ex)
            {
                return new ApiResultResponse<ProjectVm>()
                {
                    IsSuccess = false,
                    Errors = new List<string> {ex.Message}
                };
            }
           
        }

        public async Task< ApiResultResponse<ProjectVm>> CreateAsync(ProjectVm model)
        {
            try
            {
                if (_projectRepo.Table.Any(d => d.Name == model.Name))
                {
                    return new ApiResultResponse<ProjectVm>()
                    {
                        Message =  "A Project with the name already exists, please choose another name",
                        IsSuccess = false
                    };
                }

                var project = new Project()
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Amount = model.Amount,
                    EmployeeId = model.EmployeeId
                };

                await _projectRepo.InsertAsync(project);
          
                return new ApiResultResponse<ProjectVm>() { Data = model };
            }
            catch (Exception ex)
            {
                return new ApiResultResponse<ProjectVm>()
                {
                    IsSuccess = false,
                    Errors = new List<string> {ex.Message}
                };
            }
           
        }

        public async Task<ApiResultResponse<ProjectVm>> UpdateAsync(ProjectVm model)
        {
            try
            {
                var projectToUpdate = await _projectRepo.GetByIdAsync(model.Id);

                if (projectToUpdate is null)
                {
                    return new ApiResultResponse<ProjectVm>()
                    {
                        Errors = new List<string> {"Bad Request"}
                    };
                }

                projectToUpdate.Name = model.Name;
                projectToUpdate.StartDate = model.StartDate;
                projectToUpdate.EndDate =model.EndDate;
                projectToUpdate.Amount = model.Amount;
                projectToUpdate.EmployeeId = model.EmployeeId;
           
                await _projectRepo.UpdateAsync(projectToUpdate);

                return new ApiResultResponse<ProjectVm>() { Data = model };
            }
            catch (Exception ex)
            {
                return new ApiResultResponse<ProjectVm>()
                {
                    IsSuccess = false,
                    Errors = new List<string> {ex.Message}
                };
            }
           
        }



        public async Task DeleteAsync(string id)
        {
            var project = await _projectRepo.GetByIdAsync(id);

            if (project is null)
            {
                throw new Exception("Bad Request");
            }
            await _projectRepo.DeleteAsync(project);
            
        }
    }
}