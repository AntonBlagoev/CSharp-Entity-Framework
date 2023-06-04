namespace Theatre.Common
{
    public class ValidationConstants
    {
        // Theatre
        public const int TheatreNameMinLenght = 4;
        public const int TheatreNameMaxLenght = 30;
        public const int TheatreNumberOfHallsMinValue = 1;
        public const int TheatreNumberOfHallsMaxValue = 10;
        public const int TheatreDirectorMinLenght = 4;
        public const int TheatreDirectorMaxLenght = 30;


        // Play
        public const int PlayTitleMinLenght = 4;
        public const int PlayTitleMaxLenght = 50;

        public const double PlayRatingMinValue = 0.00;
        public const double PlayRatingMaxValue = 10.00;

        public const int PlayDescriptionMaxLenght = 700;
        public const int PlayScreenwriterMinLenght = 4;
        public const int PlayScreenwriterMaxLenght = 30;

        // Cast
        

        public const int CastFullNameMinLenght = 4;
        public const int CastFullNameMaxLenght = 30;
        public const string CastPhoneNumberRegEx = @"(?:\+\d{2}-\d{2}-\d{3}-\d{4})";


        // Ticket
        public const double TicketPriceMinValue = 1;
        public const double TicketPriceMaxValue = 100;
        public const sbyte TicketRowNumberMinValue = 1;
        public const sbyte TicketRowNumberMaxValue = 10;


    }
}