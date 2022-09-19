using Domain.Enum;

namespace Domain.Responce
{
    public class BaseResponce<T> : IBaseResponce<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
    public interface IBaseResponce<T>
    {
        public T Data { get; set; }
        public StatusCode StatusCode { get; set; }
    }
}
