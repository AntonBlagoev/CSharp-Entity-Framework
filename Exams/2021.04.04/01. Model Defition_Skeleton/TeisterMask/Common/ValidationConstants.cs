namespace TeisterMask.Common
{
    public class ValidationConstants
    {
        // Employee
        public const int EmployeeUsernameMinLength = 3;
        public const int EmployeeUsernameMaxLength = 40;
        public const string EmployeeUsernameRegEx = @"^[A-Za-z0-9]{3,40}$";
        public const string EmployeeEmailRegEx = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string EmployeePhoneRegEx = @"(?:\d{3}-\d{3}-\d{4})";

        // Project
        public const int ProjectNameMinLength = 2;
        public const int ProjectNameMaxLength = 40;

        // Task
        public const int TaskNameMinLength = 2;
        public const int TaskNameMaxLength = 40;
        public const int TaskLabelTypeMinValue = 0;
        public const int TaskLabelTypeMaxValue = 4;
        public const int TaskExecutionTypeMinValue = 0;
        public const int TaskExecutionTypeMaxValue = 3;


    }
}
// 
