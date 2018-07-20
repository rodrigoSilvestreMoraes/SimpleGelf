using System;

namespace SimpleGelf.Extensions.Logging
{
    public class LogErrorModel
    {
        public LogErrorModel(string messagem, Exception exception, object data)
        {
            Message = messagem;
            _Exception = exception;
            Data = data;
        }

        public string Message { get; private set; }
        public Exception _Exception { get; private set; }
        public object Data { get; private set; }
    }
}
