namespace VaporStore.Common
{
    public class ValidationConstants
    {
        // Game
        public const int GamePriceMinValue = 0;
        public const int GamePriceMaxValue = 2147483647;

        // User
        public const int UserUsernameMinLenght = 3;
        public const int UserUsernameMaxLenght = 20;

        public const string UserFullNameRegEx = @"[A-Z]{1}[a-z]+\s[A-Z]{1}[a-z]+";

        public const int UserAgeMinValue = 3;
        public const int UserAgeMaxValue = 103;

        public const string UserEmailRegEx = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        // Card
        public const string CardNumberRegEx = @"\d{4}\s\d{4}\s\d{4}\s\d{4}";
        public const string CardCvcRegEx = @"\d{3}";

        // Purchase
        public const string PurchaseProductKeyRegEx = @"[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}";
    }
}
