namespace EasySoccer.BLL.Infra.DTO
{
    public class TransactionResponse
    {
        public bool IsAuthorized { get; set; }
        public string TransactionJson { get; set; }
        public int Status { get; set; }
    }
}
