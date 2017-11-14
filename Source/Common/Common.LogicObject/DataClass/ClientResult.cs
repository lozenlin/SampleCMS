
namespace Common.LogicObject
{
    /// <summary>
    /// result data of JsonService to send to client
    /// </summary>
    public class ClientResult
    {
        /// <summary>
        /// boolean of result
        /// </summary>
        public bool b { get; set; }

        /// <summary>
        /// error message
        /// </summary>
        public string err { get; set; }

        /// <summary>
        /// object of data
        /// </summary>
        public object o { get; set; }
    }
}
