namespace ProjectBase.Repository.Transaction
{
    public interface ITransactionResult<T>
    {
        bool IsOkay { get; set; }

        string Message { get; set; }

        T Result { get; set; }

        int ResultCode { get; set; }
    }
}
