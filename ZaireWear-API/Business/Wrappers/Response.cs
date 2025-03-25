namespace Business.Wrappers
{
    public class Response
    {
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }

    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
