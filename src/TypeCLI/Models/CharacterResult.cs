namespace TypeCLI.Models;

public enum CharacterStatus
{
    Pending,
    Correct,
    Incorrect,
    Corrected
}

public class CharacterResult
{
    public char Expected { get; }
    public char? Typed { get; private set; }
    public CharacterStatus Status { get; private set; }
    public int Attempts { get; private set; }

    public CharacterResult(char expected)
    {
        Expected = expected;
        Status = CharacterStatus.Pending;
        Attempts = 0;
    }

    public void SetTyped(char typed)
    {
        Typed = typed;
        Attempts++;

        if (typed == Expected)
        {
            Status = Attempts == 1 ? CharacterStatus.Correct : CharacterStatus.Corrected;
        }
        else
        {
            Status = CharacterStatus.Incorrect;
        }
    }

    public void Reset()
    {
        Typed = null;
        Status = CharacterStatus.Pending;
    }

    public bool IsCorrect => Status == CharacterStatus.Correct || Status == CharacterStatus.Corrected;
    public bool IsTyped => Status != CharacterStatus.Pending;
}
