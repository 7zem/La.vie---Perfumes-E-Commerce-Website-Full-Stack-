namespace Perfumes.DAL.Constants
{
    public static class AppConstants
    {
        // Order Statuses
        public const string ORDER_STATUS_PENDING = "Pending";
        public const string ORDER_STATUS_PROCESSING = "Processing";
        public const string ORDER_STATUS_SHIPPED = "Shipped";
        public const string ORDER_STATUS_DELIVERED = "Delivered";
        public const string ORDER_STATUS_CANCELLED = "Cancelled";
        public const string ORDER_STATUS_REFUNDED = "Refunded";

        // Payment Statuses
        public const string PAYMENT_STATUS_PENDING = "Pending";
        public const string PAYMENT_STATUS_PROCESSING = "Processing";
        public const string PAYMENT_STATUS_COMPLETED = "Completed";
        public const string PAYMENT_STATUS_FAILED = "Failed";
        public const string PAYMENT_STATUS_REFUNDED = "Refunded";
        public const string PAYMENT_STATUS_CANCELLED = "Cancelled";

        // User Roles
        public const string ROLE_CUSTOMER = "Customer";
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_MANAGER = "Manager";
        public const string ROLE_SUPPORT = "Support";

        // Product Genders
        public const string GENDER_MALE = "Male";
        public const string GENDER_FEMALE = "Female";
        public const string GENDER_UNISEX = "Unisex";

        // Product Seasons
        public const string SEASON_SPRING = "Spring";
        public const string SEASON_SUMMER = "Summer";
        public const string SEASON_AUTUMN = "Autumn";
        public const string SEASON_WINTER = "Winter";
        public const string SEASON_ALL_YEAR = "All Year";

        // Product Concentrations
        public const string CONCENTRATION_EDT = "Eau de Toilette";
        public const string CONCENTRATION_EDP = "Eau de Parfum";
        public const string CONCENTRATION_PARFUM = "Parfum";
        public const string CONCENTRATION_COLOGNE = "Cologne";

        // Default Values
        public const int DEFAULT_PAGE_SIZE = 20;
        public const int MAX_PAGE_SIZE = 100;
        public const int MIN_RATING = 1;
        public const int MAX_RATING = 5;
    }
} 