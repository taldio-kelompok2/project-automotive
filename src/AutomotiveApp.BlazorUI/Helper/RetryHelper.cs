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
            Console.WriteLine($"[RetryHelper] Starting with maxAttempts={maxAttempts}, delayMs={delayMs}ms");
            onLoadingChanged(true);

            try
            {
                for (int i = 0; i < maxAttempts; i++)
                {
                    try
                    {
                        Console.WriteLine($"[RetryHelper] Attempt {i + 1}/{maxAttempts} starting...");
                        await action();
                        Console.WriteLine($"[RetryHelper] Attempt {i + 1} succeeded ✅");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[RetryHelper] Attempt {i + 1} failed ❌: {ex.GetType().Name}: {ex.Message}");

                        if (i == maxAttempts - 1)
                        {
                            Console.WriteLine($"[RetryHelper] All {maxAttempts} attempts failed. Giving up. 🚫");
                            break;
                        }

                        Console.WriteLine($"[RetryHelper] Waiting {delayMs}ms before next attempt...");
                        await Task.Delay(delayMs);
                        Console.WriteLine($"[RetryHelper] Retrying now...");
                    }
                }
            }
            finally
            {
                Console.WriteLine("[RetryHelper] Execution finished");
                onLoadingChanged(false);
            }
        }
    }
}