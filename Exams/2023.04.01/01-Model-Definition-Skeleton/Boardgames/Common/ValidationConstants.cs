namespace Boardgames.Common
{
    public class ValidationConstants
    {
        // Boardgame
        public const int BoardgameNameMinLenght = 10;
        public const int BoardgameNameMaxLenght = 20;

        public const double BoardgameRatingMinValue = 1.0;
        public const double BoardgameRatingMaxValue = 10.0;

        public const int BoardgameYearPublishedMinValue = 2018;
        public const int BoardgameYearPublishedMaxValue = 2023;

        // Seller
        public const int SellerNameMinLenght = 5;
        public const int SellerNameMaxLenght = 20;

        public const int SellerAddressMinLenght = 2;
        public const int SellerAddressMaxLenght = 30;

        public const string SellerWebsiteRegEx = @"(?:www\.)[A-Za-z0-9-]+(?:\.com)";


        // Creator
        public const int CreatorFirstNameMinLenght = 2;
        public const int CreatorFirstNameMaxLenght = 7;

        public const int CreatorLastNameMinLenght = 2;
        public const int CreatorLastNameMaxLenght = 7;


    }
}
