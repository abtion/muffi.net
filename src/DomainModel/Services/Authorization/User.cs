namespace DomainModel.Services.Authorization;

using System;
using System.Collections.Generic;

public class User {
    public string Name { get; init; }
    public Guid UserID { get; init; }
    public IEnumerable<Guid> AppRoleIDs { get; init; }

    public User(string name, Guid userID, IEnumerable<Guid> appRoleIDs) {
        Name = name;
        UserID = userID;
        AppRoleIDs = appRoleIDs;
    }
}
