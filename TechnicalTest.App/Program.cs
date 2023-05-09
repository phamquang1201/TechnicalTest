public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            int[] nums =
            {
                    -2,1,-3,4,-1,2,1,-5,4
                };
            Console.WriteLine($"Output: {MaxSubArray(nums)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
        Console.ReadKey();
    }

    private static int MaxSubArray(int[] nums)
    {
        var maxSubArray = 0;
        var sumCurrentSubArray = 0;
        foreach (int num in nums)
        {
            sumCurrentSubArray += num;
            if (maxSubArray < sumCurrentSubArray)
            {
                maxSubArray = sumCurrentSubArray;
            }
            if (sumCurrentSubArray < 0)
            {
                sumCurrentSubArray = 0;
            }
        }
        return maxSubArray;
    }
}