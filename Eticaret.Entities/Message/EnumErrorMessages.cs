using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Entities.Message
{
    public enum EnumErrorMessages
    {
        UserNameAlreadyExits = 101,
        EmailAlreadyExits = 102,
        UserrIsActive = 121,
        UserNameOrPassWrong = 122,
        CheckEmail = 151,
        UserAlreadyActivate = 152,
        ActivateIdDoesNotExits = 153,
        UserNotFound = 154,
        ProfileCouldNotUpdated = 155,
        UserCouldNotRemove = 156,
        UserCouldNotFind = 157,
        UserCouldNotInserted = 158,
        UserCouldNotUpdated = 159,
    }
}
