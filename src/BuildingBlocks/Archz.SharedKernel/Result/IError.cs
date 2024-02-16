namespace Archz.SharedKernel.Result;
internal interface IError
{
    public string Code { get; }

    public string Message { get; }
}
