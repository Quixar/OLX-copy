using OLX_copy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.Services
{
    public class CurrentUserService
    {
        private User _currentUser = null!;
        private UserAccess _currentUserAccess = null!;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
            }
        }

        public UserAccess CurrentUserAccess
        {
            get => _currentUserAccess;
            set
            {
                _currentUserAccess = value;
            }
        }
    }
}
