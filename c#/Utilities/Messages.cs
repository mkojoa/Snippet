﻿namespace eticketing_mvc.Utilities
{
    public class Messages
    {
        public static string AuthenticationRequired => "Authentication is required to complete this request.";
        public static string CreateScheduleBusError => "Bus does not exist or has already been assigned a Schedule";
        public static string UpdateScheduleBusError => "Bus does not exist or has already been assigned a Schedule";
        public static string IdError => "ID cannot not be less than one (1)";

        public static string ProcessingError =>
            "An error occured while processing your request. Contact your administrator";

        public static string EntityCreationError(string entityName)
        {
            return $"An error occured while creating new {entityName}. Contact your administrator.";
        }

        public static string EntityCreationSuccess(string entityName)
        {
            return $"New {entityName} Created Successfully.";
        }

        public static string EntityUpdationSuccess(string entityName)
        {
            return $"{entityName} Updated Successfully.";
        }
        public static string EntityUpdationError(string entityName)
        {
            return $"An error occured while delteting {entityName}. Contact your administrator.";
        }

        public static string EntityDeletionSuccess(string entityName)
        {
            return $"{entityName} Deleted Successfully.";
        }
        public static string EntityDeletionError(string entityName)
        {
            return $"An error occured while delteting {entityName}. Contact your administrator.";
        }
    }
}