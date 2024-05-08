namespace System
{
    public static class Calculations
    {
        public static int GetAge(this DateTime birthday)
        {
            if (birthday.Month >= DateTime.Today.Month && birthday.Day > DateTime.Today.Day)
                return (DateTime.Today.Year - birthday.Year) - 1;
            else
                return DateTime.Today.Year - birthday.Year;
        }

        public static int GetAge(this DateTime birthday, DateTime actualDate)
        {
            if (birthday.Month >= actualDate.Month && birthday.Day > actualDate.Day)
                return (actualDate.Year - birthday.Year) - 1;
            else
                return actualDate.Year - birthday.Year;
        }
    }
}