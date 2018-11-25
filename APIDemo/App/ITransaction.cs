namespace APIDemo.App
{
    public interface ITransaction
    {
        string ErrMsg { get; set; }

        /// <summary>
        /// 執行的邏輯
        /// </summary>
        /// <returns>true: 成功, false: 失敗</returns>
        bool Execute();
    }
}