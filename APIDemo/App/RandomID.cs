using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace APIDemo.App
{
    public class RandomID
    {
        /// <summary>
        /// 取得隨機身分證字號List
        /// </summary>
        /// <param name="size">List大小</param>
        /// <returns>身分證字號List</returns>
        public List<string> getRandomIDs(int size)
        {
            var randomIDs = new List<string>();

            for (int i = 0; i < size; i++)
            {
                randomIDs.Add(getRandomID());
            }

            return randomIDs;
        }

        /// <summary>
        /// 取得隨機身分證字號
        /// </summary>
        /// <returns>身分證字號</returns>
        public string getRandomID()
        {
            string ID = "";

            try
            {
                char firstChar = getFirstChar();
                int gender = getGender();
                string num_3rdTo9th = getRandomNum(7);

                var temp = new int[10];
                temp[0] = convertD0(firstChar);
                temp[1] = gender;
                for (int i = 0; i < 7; i++)
                {
                    temp[i + 2] = Convert.ToInt32(num_3rdTo9th.Substring(i, 1));
                }
                temp[9] = (10 - (getCheckSum(temp) % 10)) % 10;

                ID = firstChar.ToString() + gender.ToString() + num_3rdTo9th + temp[9];
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(Const.AP_ID, Util.getDebugMsg(MethodBase.GetCurrentMethod(), e.ToString()), EventLogEntryType.Error);
            }

            return ID;
        }

        private List<string> address = new List<string> { "A.台北市","B.台中市","C.基隆市","D.台南市",
            "E.高雄市","F.新北市","G.宜蘭縣","H.桃園縣","I.嘉義市","J.新竹縣","K.苗栗縣","L.台中縣",
            "M.南投縣","N.彰化縣","O.新竹市","P.雲林縣","Q.嘉義縣","R.台南縣","S.高雄縣","T.屏東縣",
            "U.花蓮縣","V.台東縣","W.金門縣","X.澎湖縣","Y.陽明山管理局","Z.連江縣" };

        /// <summary>
        /// 隨機產生第一個英文字元
        /// </summary>
        /// <returns>英文字元</returns>
        private char getFirstChar()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            int index = random.Next(address.Count);
            return Convert.ToChar(address[index].Substring(0, 1));
        }

        /// <summary>
        /// 隨機產生性別
        /// </summary>
        /// <returns>1: 男, 2: 女</returns>
        private int getGender()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return (random.Next(1, 100) % 2) == 0 ? 2 : 1;
        }

        /// <summary>
        /// 隨機產生指定長度的數字字串
        /// </summary>
        /// <param name="length">長度</param>
        /// <returns>數字字串</returns>
        private string getRandomNum(int length)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            string num = "";

            for (int i = 0; i < length; i++)
            {
                num += random.Next(9);
            }

            return num;
        }

        /// <summary>
        /// 轉換英文字元為對應的數字
        /// </summary>
        /// <param name="c">英文字元</param>
        /// <returns>數字</returns>
        private int convertD0(char c)
        {
            int D0 = 0;
            int temp = c;

            if (72 >= temp && temp >= 65)
            {
                D0 = temp - 55;
            }
            else if (78 >= temp && temp >= 74)
            {
                D0 = temp - 56;
            }
            else if (86 >= temp && temp >= 80)
            {
                D0 = temp - 57;
            }
            else if (89 >= temp && temp >= 88)
            {
                D0 = temp - 58;
            }

            switch (temp)
            {
                case 73:
                    D0 = temp - 39;
                    break;
                case 79:
                    D0 = temp - 44;
                    break;
                case 87:
                    D0 = temp - 55;
                    break;
                case 90:
                    D0 = temp - 57;
                    break;
                default:
                    break;
            }

            return D0;
        }

        /// <summary>
        /// 取得檢查碼
        /// </summary>
        /// <param name="temp">暫存數字陣列</param>
        /// <returns>檢查碼</returns>
        private int getCheckSum(int[] temp)
        {
            int y = temp[0] / 10 + temp[0] % 10 * 9;

            for (int i = 1; i <= 8; i++)
            {
                y += (9 - i) * temp[i];
            }

            return y;
        }
    }
}