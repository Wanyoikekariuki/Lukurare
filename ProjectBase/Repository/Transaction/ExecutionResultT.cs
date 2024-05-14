namespace ProjectBase.Repository.Transaction
{
    public class ExecutionResult<T> : ITransactionResult<T>
    {
        public System.Exception Exception { get; set; }

        public bool IsOkay { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }

        public int ResultCode { get; set; }

        public string TechnicalMessage { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public ExecutionResult() { }

        public ExecutionResult(bool isokay)
        {
            IsOkay = isokay;
        }

        public ExecutionResult(bool isokay, string message)
            : this(isokay)
        {
            Message = message;
        }

        public ExecutionResult(bool isOkay, string message, T result)
            : this(isOkay, message)
        {
            Result = result;
        }

        public ExecutionResult(bool isOkay, string message, T result, string technicalMessage)
            : this(isOkay, message, result)
        {
            TechnicalMessage = technicalMessage;
        }

        public ExecutionResult(bool isOkay, string message, System.Exception ex)
            : this(isOkay, message)
        {
            Exception = ex;
            TechnicalMessage = ex.ToString();
        }

        public ExecutionResult(
            bool isOkay,
            string message,
            System.Exception ex,
            string technicalMessage
        )
            : this(isOkay, message, ex)
        {
            TechnicalMessage = technicalMessage;
        }

        public ExecutionResult(bool isOkay, string message, string technicalMessage)
            : this(isOkay, message)
        {
            TechnicalMessage = technicalMessage;
        }

        public virtual void Clone(ExecutionResult<T> kaOther)
        {
            IsOkay = kaOther.IsOkay;
            Message = kaOther.Message;
            Result = kaOther.Result;
            TechnicalMessage = kaOther.TechnicalMessage;
        }

        public virtual ExecutionResult<NewT> CloneDiffrentType<NewT>()
        //where NewT : class
        {
            ExecutionResult<NewT> executionResult = new ExecutionResult<NewT>()
            {
                IsOkay = IsOkay,
                Message = Message,
                TechnicalMessage = TechnicalMessage,
                Exception = Exception,
                //Result = this.Result as NewT,
            };
            return executionResult;
        }

        public ExecutionResult<T> CopyFromException(System.Exception ex)
        {
            IsOkay = false;
            Message = ex.Message;
            TechnicalMessage = ex.ToString();
            return this;
        }

        public virtual ExecutionResult CloneOld()
        {
            ExecutionResult executionResult = new ExecutionResult()
            {
                IsOkay = IsOkay,
                Message = Message,
                TechnicalMessage = TechnicalMessage
            };
            return executionResult;
        }

        public virtual void LoadOldExecutionResult(ExecutionResult t_result)
        {
            IsOkay = t_result.IsOkay;
            Message = t_result.Message;
            TechnicalMessage = t_result.TechnicalMessage;
        }
    }
}
