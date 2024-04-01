namespace Refit;

public static class ApiResponseExtensions
{
    public static AndConstraint<ApiResponse<T>> Ensure<T>(this ApiResponse<T> response, HttpStatusCode statusCode)
    {
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(statusCode);
            response.Error?.Content.Should().BeNullOrEmpty();
        }

        return new AndConstraint<ApiResponse<T>>(response);
    }

    public static AndConstraint<ApiResponse<T>> Ensure<T>(this ApiResponse<T> response, HttpStatusCode statusCode, string message)  
    {  
        using (new AssertionScope())  
        {  
            response.StatusCode.Should().Be(statusCode);  
            response.Error?.Content.Should().Contain(message);  
        }
        
        return new AndConstraint<ApiResponse<T>>(response);
    }  
}