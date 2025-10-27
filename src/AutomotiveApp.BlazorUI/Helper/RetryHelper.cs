namespace AutomotiveApp.BlazorUI.Helper
{
    public static class RetryHelper
    {
        public static async Task ExecuteWithRetryAsync(
            Func<Task> action,
            Action<bool> onLoadingChanged,
            int maxAttempts = 5,
            int delayMs = 100)
        {
            onLoadingChanged(true);

            try
            {
                for (int i = 0; i < maxAttempts; i++)
                {
                    try
                    {
                        await action();
                        return;
                    }
                    catch
                    {
                        if (i == maxAttempts - 1)
                            break;

                        await Task.Delay(delayMs);
                    }
                }
            }
            finally
            {
                onLoadingChanged(false);
            }
        }
    }
}