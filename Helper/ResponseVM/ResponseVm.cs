namespace MonthlyExpenseTracker.Helper.ResponseVM
{
        public class ResponseVm<T>
        {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }       
        public T? Response { get; set; }           
        public List<string> Errors { get; set; }

        public ResponseVm()
            {
                Errors = new();
            }

            public ResponseVm(T response, string message = "Success", int statusCode = 200)
            {
                Success = true;
                StatusCode = statusCode;
                Message = message;
                Response = response;
                Errors = new();
            }

            public ResponseVm(List<string> errors, string message = "Failed", int statusCode = 400)
            {
                Success = false;
                StatusCode = statusCode;
                Message = message;
                Errors = errors ?? new List<string>();
                Response = default;
            }

            public ResponseVm(string error, string message = "Failed", int statusCode = 400)
            {
                Success = false;
                StatusCode = statusCode;
                Message = message;
                Errors = new List<string> { error };
                Response = default;
            }
        }
}
