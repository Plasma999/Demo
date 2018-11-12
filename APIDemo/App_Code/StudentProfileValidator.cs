using APIDemo.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace APIDemo.App_Code
{
    public class StudentProfileValidator
    {
        StudentProfile studentProfile { get; set; }

        public StudentProfileValidator(StudentProfile studentProfile)
        {
            this.studentProfile = studentProfile;
        }

        public bool ValidateAll(ref string errMsg)
        {
            bool isValid = true;
            List<IValidator> validators = getValidators();
            var errMsgs = new List<string>();
            foreach (var validator in validators)
            {
                if (!validator.Validate())
                {
                    isValid = false;
                    errMsgs.Add(validator.ErrMsg);
                }
            }

            if (errMsgs.Count > 0)
            {
                errMsg = string.Join(",", errMsgs.ToArray());
            }
            
            return isValid;
        }

        private List<IValidator> getValidators()
        {
            var result = new List<IValidator>();
            //TODO result.Add(new StudentProfileIdValidator(studentProfile));
            result.Add(new StudentProfileNameValidator(studentProfile));
            result.Add(new StudentProfileGenderValidator(studentProfile));
            result.Add(new StudentProfileBloodValidator(studentProfile));
            result.Add(new StudentProfileHeightValidator(studentProfile));
            result.Add(new StudentProfileWeightValidator(studentProfile));
            result.Add(new StudentProfileCouponValidator(studentProfile));

            return result;
        }
    }

    internal class StudentProfileIdValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        string Id { get; set; }

        public StudentProfileIdValidator(StudentProfile studentProfile)
        {
            Id = studentProfile.Id;
        }

        public bool Validate()
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(Id))
            {
                ErrMsg = "Id cant be empty or whitespace!";
            }
            else if (IsForeigner() || IsValidId())
            {
                result = true;
            }

            return result;
        }

        private bool IsForeigner()
        {
            bool result = false;

            if (Id.Length == 10)
            {
                char[] arr = Id.ToCharArray();

                //身分證前兩碼 or 後兩碼為英文
                if ((Util.isEnglishChar(arr[0]) && Util.isEnglishChar(arr[1]) && int.TryParse(Id.Substring(2), out int m)) 
                    || (Util.isEnglishChar(arr[8]) && Util.isEnglishChar(arr[9]) && int.TryParse(Id.Substring(0, 7), out int n)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool IsValidId()
        {
            bool result = false;

            //TODO 身分證檢查

            return result;
        }
    }

    internal class StudentProfileNameValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        string Name { get; set; }

        public StudentProfileNameValidator(StudentProfile studentProfile)
        {
            Name = studentProfile.Name;
        }

        public bool Validate()
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrMsg = "Name cant be empty or whitespace!";
            }
            else if (checkLength() && checkSpecialChar() && checkSpace())
            {
                result = true;
            }

            return result;
        }

        private bool checkLength()
        {
            bool result = false;

            int maxSize = 50;  //資料庫欄位長度
            if (Name.Length > maxSize)
            {
                ErrMsg += "Name cant be more than " + maxSize + " characters!";
            }
            
            if (string.IsNullOrEmpty(ErrMsg))
            {
                result = true;
            }

            return result;
        }

        private bool checkSpecialChar()
        {
            bool result = false;

            //特殊字元檢查，可允許空白和.
            string s = Name.Replace(" ", "").Replace(".", "");
            if (Regex.IsMatch(s, @"[\W_]+"))
            {
                ErrMsg += "Name cant have illegal special characters!";
            }

            if (string.IsNullOrEmpty(ErrMsg))
            {
                result = true;
            }

            return result;
        }

        private bool checkSpace()
        {
            bool result = false;

            //開頭不能有空白
            if (Name.Substring(0, 1) == " ")
            {
                ErrMsg += "the first character of Name cant be whitespace!";
            }
            //結尾不能有空白
            if (Name.Substring(Name.Length - 1) == " ")
            {
                ErrMsg += "the last character of Name cant be whitespace!";
            }
            //不能出現兩個以上的空白相連
            if (Name.Contains("  "))
            {
                ErrMsg += "Name cant have double whitespace!";
            }

            if (string.IsNullOrEmpty(ErrMsg))
            {
                result = true;
            }

            return result;
        }
    }

    internal class StudentProfileGenderValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        string Gender { get; set; }

        public StudentProfileGenderValidator(StudentProfile studentProfile)
        {
            Gender = studentProfile.Gender;
        }

        public bool Validate()
        {
            return checkGender();
        }

        private bool checkGender()
        {
            bool result = false;

            if (Const.Gender.Contains(Gender))
            {
                result = true;
            }
            else
            {
                ErrMsg = "please choose a correct Gender!";
            }

            return result;
        }
    }

    internal class StudentProfileBloodValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        string Blood { get; set; }

        public StudentProfileBloodValidator(StudentProfile studentProfile)
        {
            Blood = studentProfile.Blood;
        }

        public bool Validate()
        {
            return checkBlood();
        }

        private bool checkBlood()
        {
            bool result = false;

            if (Const.Blood.Contains(Blood))
            {
                result = true;
            }
            else
            {
                ErrMsg = "please choose a correct Blood!";
            }

            return result;
        }
    }

    internal class StudentProfileHeightValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        decimal? Height { get; set; }

        public StudentProfileHeightValidator(StudentProfile studentProfile)
        {
            Height = studentProfile.Height;
        }

        public bool Validate()
        {
            return checkHeight();
        }

        private bool checkHeight()
        {
            bool result = false;

            if (Util.checkMoreThanZero(Height))
            {
                result = true;
            }
            else
            {
                ErrMsg = "please input a Height more than 0!";
            }

            return result;
        }
    }

    internal class StudentProfileWeightValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        decimal? Weight { get; set; }

        public StudentProfileWeightValidator(StudentProfile studentProfile)
        {
            Weight = studentProfile.Weight;
        }

        public bool Validate()
        {
            return checkWeight();
        }

        private bool checkWeight()
        {
            bool result = false;

            if (Util.checkMoreThanZero(Weight))
            {
                result = true;
            }
            else
            {
                ErrMsg = "please input a Weight more than 0!";
            }

            return result;
        }
    }

    internal class StudentProfileCouponValidator : IValidator
    {
        public bool IsValid { get; set; }
        public string ErrMsg { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string Coupon { get; set; }

        public StudentProfileCouponValidator(StudentProfile studentProfile)
        {
            Id = studentProfile.Id;
            Name = studentProfile.Name;
            Coupon = studentProfile.Coupon;
        }

        public bool Validate()
        {
            return checkCoupon();
        }

        private bool checkCoupon()
        {
            bool result = false;
            string correctCoupon = Util.truncateString(Util.getMD5(Id + Name), 15);

            if (string.IsNullOrWhiteSpace(Coupon) || Coupon == correctCoupon)
            {
                result = true;
            }
            else
            {
                ErrMsg = "wrong Coupon! correct one is " + correctCoupon;
            }

            return result;
        }
    }
}