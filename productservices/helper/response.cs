public class DbResponse
{
    public string Message { get; set; }
    public bool HasError { get; set; }
    public object Response { get; set; }
    public string Key { get; set; }
}

public class GeneralResponse
{
    public string Message { get; set; }
    public bool HasError { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public bool HasError { get; set; } = true;
}