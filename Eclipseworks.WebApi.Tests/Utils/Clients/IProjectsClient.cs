using MediatR;

namespace Eclipseworks.WebApi.Tests.Utils.Clients;

public interface IProjectsClient
{
    [Post("/api/projects")]
    Task<ApiResponse<Guid>> CreateProjectAsync([Body] CreateProjectCommand command);
    
    [Get("/api/projects")]
    Task<ApiResponse<IEnumerable<ListProjectsViewModel>>> ListProjectAsync();
    
    [Post("/api/projects/{projectId}/tasks")]
    Task<ApiResponse<Guid>> CreateProjectTaskAsync(Guid projectId, [Body] CreateProjectTaskPayload payload);
    
    [Delete("/api/projects/{projectId}")]
    Task<ApiResponse<Unit>> DeleteProjectAsync(Guid projectId);

    [Get("/api/projects/{projectId}/tasks")]
    Task<ApiResponse<IEnumerable<ListProjectTasksViewModel>>> ListProjectTasksAsync(Guid projectId);
    
    [Put("/api/projects/{projectId}/tasks/{taskId}")]
    Task<ApiResponse<Unit>> UpdateProjectTaskAsync(Guid projectId, Guid taskId, [Body] UpdateProjectTaskPayload payload);
    
    [Delete("/api/projects/{projectId}/tasks/{taskId}")]
    Task<ApiResponse<Unit>> DeleteProjectTaskAsync(Guid projectId, Guid taskId);
    
    [Post("/api/projects/{projectId}/tasks/{taskId}/comments")]
    Task<ApiResponse<Guid>> CreateProjectTaskCommentAsync(Guid projectId, Guid taskId, [Body] CreateProjectTaskCommentPayload payload);

    
        
}