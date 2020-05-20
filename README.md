# Overview
A Todo app that is a sample of development using ASP.NET MVC and Entity Framework, etc.

# Technology Used/Focused
- ASP.NET MVC (Not ASP.NET Core MVC)
- Entity Framework
  - Code First
  - Automatic migration
    - Initial data registration
      - Initial data of User and Role are input.
- Authentication, Authorization function by Membership Framework
  - MembershipProvider
  - RoleProvider
- Bootstrap
  - Apply the design as a whole
  - Header navigation bar, hamburger button/menu

# Implementation Function
- ToDo data management function
  - Registration, Display(List, Details), Edit, Delete of ToDo data.
  - ToDo management for each user
    - Display the ToDo data only to the user who created the ToDo data.
- Authentication, Authorization function
  - SignIn, SignOut
  - [Note] SignUp is not implemented.
  - Password hashing
- User management function
  - Registration, Display(List, Details), Edit, Delete of User data.
  - Only Administrators role has access.
- [Note] Role management function(CRUD function for Role data) is not implemented.
  - Roles are displayed together on the user management screen.
  - You can select roles when registering / editing a user.
