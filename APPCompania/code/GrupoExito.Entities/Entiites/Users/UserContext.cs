using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Users;

namespace GrupoExito.Entities
{
    public class UserContext
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Gender { get; set; }

        public string DateOfBirth { get; set; }

        public bool AcceptTerms { get; set; }

        public bool IsAnonymous
        {
            get
            {
                return string.IsNullOrEmpty(Id);
            }
        }

        public double Points { get; set; }

        public bool Prime { get; set; }

        public UserAddress Address { get; set; }

        public Store Store { get; set; }

        public string DependencyId
        {
            get
            {
                if (Address != null)
                {
                    return Address.DependencyId;
                }
                else if (Store != null)
                {
                    return Store.Id.ToString();
                }

                return string.Empty;
            }
        }

        public string ActiveOrder { get; set; }

        public string PaymentMethodPrime { get; set; }
        public string EndDatePrime { get; set; }
        public string StartDatePrime { get; set; }
        public int ActiveOrderCount { get; set; }
        public string PeriodicityPrime { get; set; }
        public Segment UserType { get; set; }

        public bool UserActivate { get; set; }
    }
}
