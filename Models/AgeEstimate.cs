namespace tests_cache_dotnet.Models;

public class AgeEstimate
{
    public int Count { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }

}

public class AgeEstimateWithMemoryBomb
{
    public AgeEstimateWithMemoryBomb(AgeEstimate? estimate, int bombLength, int bombDepth)
    {
        Estimate = estimate;
        MemoryBomb = new MemoryBomb(bombLength, bombDepth);
    }
    
    public AgeEstimate? Estimate { get; set; }

    public MemoryBomb? MemoryBomb { get; set; }
}

public class MemoryBomb
{
    public MemoryBomb(int bombLength, int bombDepth)
    {
        TextBomb = new string('a', bombLength);

        if(bombDepth > 1)
        {
            InnerBomb = new MemoryBomb(bombLength, bombDepth - 1);
            return;
        }

        InnerBomb = new MemoryBomb(bombLength);
    }

    public MemoryBomb(int bombLength)
    {
        TextBomb = new string('a', bombLength);
    }

    public string? TextBomb { get; set; }

    public MemoryBomb? InnerBomb { get; set; }
}