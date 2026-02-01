namespace TypeCLI.Models;

public class GameState
{
    private readonly List<CharacterResult> _characters;
    private int _cursorPosition;
    private int _correctCount;
    private int _incorrectCount;

    public GameMode Mode { get; }
    public string TargetText { get; }
    public IReadOnlyList<CharacterResult> Characters => _characters;
    public int CursorPosition => _cursorPosition;
    public int TotalCharacters { get; }
    public DateTime? StartTime { get; private set; }
    public TimeSpan? TimeLimit { get; }
    public bool HasStarted => StartTime.HasValue;
    public bool IsComplete { get; private set; }

    public GameState(GameMode mode, string targetText)
    {
        Mode = mode;
        TargetText = targetText;
        _characters = targetText.Select(c => new CharacterResult(c)).ToList();
        TotalCharacters = _characters.Count;
        _cursorPosition = 0;
        _correctCount = 0;
        _incorrectCount = 0;

        if (mode.IsTimed())
        {
            TimeLimit = TimeSpan.FromSeconds(mode.GetDurationSeconds());
        }
    }

    public void Start()
    {
        if (!HasStarted)
        {
            StartTime = DateTime.Now;
        }
    }

    public TimeSpan GetElapsedTime()
    {
        if (!HasStarted)
        {
            return TimeSpan.Zero;
        }

        return DateTime.Now - StartTime!.Value;
    }

    public TimeSpan GetRemainingTime()
    {
        if (!TimeLimit.HasValue || !HasStarted)
        {
            return TimeLimit ?? TimeSpan.Zero;
        }

        TimeSpan remaining = TimeLimit.Value - GetElapsedTime();
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public bool ProcessCharacter(char typed)
    {
        if (IsComplete || _cursorPosition >= _characters.Count)
        {
            return false;
        }

        if (!HasStarted)
        {
            Start();
        }

        CharacterResult current = _characters[_cursorPosition];
        current.SetTyped(typed);

        if (current.IsCorrect)
        {
            _correctCount++;
        }
        else
        {
            _incorrectCount++;
        }

        _cursorPosition++;
        CheckCompletion();
        return true;
    }

    public bool ProcessBackspace()
    {
        if (_cursorPosition <= 0)
        {
            return false;
        }

        _cursorPosition--;
        CharacterResult current = _characters[_cursorPosition];

        if (current.IsCorrect)
        {
            _correctCount--;
        }
        else if (current.Status == CharacterStatus.Incorrect)
        {
            _incorrectCount--;
        }

        current.Reset();
        return true;
    }

    public void ForceComplete()
    {
        IsComplete = true;
    }

    private void CheckCompletion()
    {
        if (_cursorPosition >= _characters.Count)
        {
            IsComplete = true;
        }
    }

    public void CheckTimeExpired()
    {
        if (TimeLimit.HasValue && HasStarted && GetRemainingTime() <= TimeSpan.Zero)
        {
            IsComplete = true;
        }
    }

    public int GetCorrectCount() => _correctCount;
    public int GetIncorrectCount() => _incorrectCount;
    public int GetTypedCount() => _cursorPosition;

    public double GetCurrentWpm()
    {
        TimeSpan elapsed = GetElapsedTime();
        if (elapsed.TotalMinutes <= 0)
        {
            return 0;
        }

        return (_cursorPosition / 5.0) / elapsed.TotalMinutes;
    }

    public double GetCurrentAccuracy()
    {
        if (_cursorPosition == 0)
        {
            return 100;
        }

        return (_correctCount / (double)_cursorPosition) * 100;
    }
}
