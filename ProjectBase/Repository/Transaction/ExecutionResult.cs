namespace ProjectBase.Repository.Transaction
{
    public class ExecutionResult : ExecutionResult<object>
    {
        public ExecutionResult() { }

        public ExecutionResult(bool isokay)
            : base(isokay) { }

        public ExecutionResult(bool isokay, string message)
            : base(isokay, message) { }

        public ExecutionResult(bool isOkay, string message, object result)
            : base(isOkay, message, result) { }

        public ExecutionResult(bool isOkay, string message, object result, string technicalMessage)
            : base(isOkay, message, result, technicalMessage) { }
    }
}
