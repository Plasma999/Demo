namespace APIDemo.App
{
    interface IValidator
    {
        bool IsValid { get; set; }
        string ErrMsg { get; set; }

        /// <summary>
        /// 驗證的邏輯
        /// </summary>
        /// <returns>true: 成功, false: 失敗</returns>
        bool Validate();
    }
}