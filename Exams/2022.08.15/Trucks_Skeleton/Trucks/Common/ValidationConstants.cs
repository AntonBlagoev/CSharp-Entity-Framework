namespace Trucks.Common
{
    public static class ValidationConstants
    {
        // Truck
        public const int TruckRegistrationNumberLenght = 8;
        public const int TruckVinNumberLenght = 17;
        public const int TruckTankCapacityMinValue = 950;
        public const int TruckTankCapacityMaxValue = 1420;
        public const int TruckCargoCapacityMinValue = 5000;
        public const int TruckCargoCapacityMaxValue = 29000;
        public const string TruckRegistrationNumberRegEx = @"[A-Z]{2}\d{4}[A-Z]{2}";



        // Client
        public const int ClientNameMinLenght = 3;
        public const int ClientNameMaxLenght = 40;
        public const int ClientNationalityMinLenght = 2;
        public const int ClientNationalityMaxLenght = 40;

        // Despatcher
        public const int DespatcherNameMinLenght = 2;
        public const int DespatcherNameMaxLenght = 40;


        // ClientTruck





    }
}
