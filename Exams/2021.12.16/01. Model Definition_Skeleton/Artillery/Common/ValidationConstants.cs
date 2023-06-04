namespace Artillery.Common
{
    public class ValidationConstants
    {
        // Country
        public const int CountryNameMinLenght = 4;
        public const int CountryNameMaxLenght = 60;
        public const int CountryArmySizeMinValue = 50000;
        public const int CountryArmySizeMaxValue = 10000000;

        // Manufacturer

        public const int ManufacturerNameMinLenght = 4;
        public const int ManufacturerNameMaxLenght = 40;
        public const int ManufacturerFoundedMinLenght = 10;
        public const int ManufacturerFoundedMaxLenght = 100;

        // Shell
        public const int ShellWeightMinValue = 2;
        public const int ShellWeightMaxValue = 1680;
        public const int ShelCaliberMinLenght = 4;
        public const int ShelCaliberMaxLenght = 30;

        // Gun
        public const int GunWeightMinValue = 100;
        public const int GunWeightMaxValue = 1350000;
        public const double GunBarrelLengthMinValue = 2.0;
        public const double GunBarrelLengthMaxValue = 35.0;
        public const int GunRangeMinValue = 1;
        public const int GunRangeMaxValue = 100000;

        //public const int GunTypeMinValue = 0;
        //public const int GunTypeMaxValue = 5;





    }
}
