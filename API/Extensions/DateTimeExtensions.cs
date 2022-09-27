namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - date.Year;

            if(date.Date > today.AddYears(-age)) 
                age--;

            return age;
        }
    }
}