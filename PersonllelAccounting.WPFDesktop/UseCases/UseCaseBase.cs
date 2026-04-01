using WPF_Desktop.Utils;

namespace WPF_Desktop.UseCases;

public abstract class UseCaseBase
{
    protected async Task<Resource<T>> SafeCallAsync<T>(Func<Task<T>> action)
    {
        try
        {
            var result = await action();
            return Resource<T>.Success(result);
        }
        catch (Exception e)
        {
            return Resource<T>.Failure("", e);
        }
    }

    protected async Task<Resource> SafeCallAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Resource.Success();
        }
        catch (Exception e)
        {
            return Resource.Failure("", e);
        }
    }
}