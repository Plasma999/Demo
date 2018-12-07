namespace APIDemo.App
{
    interface ILogger
    {
        /// <summary>
        /// 寫log
        /// </summary>
        /// <param name="msg">訊息</param>
        void Log(string msg);
    }
}