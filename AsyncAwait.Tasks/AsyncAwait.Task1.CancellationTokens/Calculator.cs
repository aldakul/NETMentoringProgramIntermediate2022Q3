using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static async Task<long> Calculate(int n, CancellationToken token)
    {
        return await Task.Run((() => CalculateSum(n, token)));
    }
    private static long CalculateSum(int n, CancellationToken token)
    {
        long result = 0;
        for (int i = 0; i < n; i++)
        {
            if (token.IsCancellationRequested)
            {
                return result;
            }
            result = result + (i + 1);
            Thread.Sleep(10);
        }
        return result;
    }
}
