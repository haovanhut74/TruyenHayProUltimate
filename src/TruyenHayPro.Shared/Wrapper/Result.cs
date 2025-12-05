namespace TruyenHayPro.Shared.Wrapper;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string[] Errors { get; }

    // Constructor được bảo vệ để bắt buộc dùng hàm Static
    protected Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    // Trả về thành công (không có dữ liệu kèm theo)
    public static Result Success()
        => new(true, []);

    // Trả về thất bại với 1 danh sách lỗi
    public static Result Failure(IEnumerable<string> errors)
        => new(false, errors.ToArray());

    // Trả về thất bại với 1 lỗi duy nhất
    public static Result Failure(string error)
        => new(false, [error]);
}

// Class Generic dùng khi cần trả về dữ liệu (ví dụ: UserId, Token...)
public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T? value, bool isSuccess, string[] errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(value, true, []);

    public new static Result<T> Failure(IEnumerable<string> errors)
        => new(default, false, errors.ToArray());

    public new static Result<T> Failure(string error)
        => new(default, false, [error]);
}