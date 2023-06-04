namespace Footballers.Common
{
    public class ValidationConstraints
    {
        // Footballer
        public const int FootballerNameMinLenght = 2;
        public const int FootballerNameMaxLenght = 40;


        // Team
        public const int TeamNameMinLenght = 3;
        public const int TeamNameMaxLenght = 40;
        public const string TeamNameRegEx = @"^[a-zA-Z0-9 .-]*$";
        public const int TeamNationalityMinLenght = 2;
        public const int TeamNationalityMaxLenght = 40;


        // Coach
        public const int CoachNameMinLenght = 2;
        public const int CoachNameMaxLenght = 40;

        


    }
}
