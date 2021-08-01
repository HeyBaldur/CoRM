namespace Seed.Models.V1.DTOs
{
    public class UserRequestDto
    {
        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Verification if the user has confirm it is not a robot
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// Photo URL of the user
        /// </summary>
        public string PhotoUrl { get; set; }
        
        /// <summary>
        /// Access password of the user
        /// </summary>
        public string Password { get; set; }
    }
}
