# UserMicroservice 
This is a simple microservice created for users management.
Technology: ASP .NET Core API with basic CRUD operations for users and admins.


# Show user 

**Title :** Show user by email. __
**URL :** /api/Users/:email __ 
**Method :** GET __
**URL Params :** Required: email = [string] __
**Data Params :** __
{ __
  "id": [int], __
  "email": [string], __
  "name": [string], __
  "surname": [string], __
  "password": [string], __
  "salt": [string], __
  "authToken": [string], __
  "authTokenExpiration": [DateTime], __
  "permissionId": [int] __
} __
**Response Codes :** Success (200 OK), Not Found (404) 


# Add user

**Title :** Add new user. __
**URL :** /api/Users __
**Method :** POST __
**URL Params :**  NONE __
**Data Params :** __
{ __
  "id": [int], 
  "email": [string],
  "name": [string],
  "surname": [string],
  "password": [string],
  "salt": [string],
  "authToken": [string],
  "authTokenExpiration": [DateTime],
  "permissionId": [int]
}
**Response Codes :** Success (200 OK), Bad Request (400), Not Found (404) 
**Annotation :** the error 404 will be returned in case of EXISTANCE of user with provided new email!!!
**Annotation :** Id will be provided automatically by application!


# Update user

**Title :** Update user.
**URL :** /api/Users/:email
**Method :** PUT
**URL Params :** Required: email = [string] 
**Data Params :** 
{
  "id": [int],
  "email": [string],
  "name": [string],
  "surname": [string],
  "password": [string],
  "salt": [string],
  "authToken": [string],
  "authTokenExpiration": [DateTime],
  "permissionId": [int]
}
**Response Codes :** Success (200 OK), Not Found (404)


# Delete user

**Title :** Delete old user.
**URL :** /api/Users/:email
**Method :** DELETE
**URL Params :** Required: email = [string] 
**Data Params :** NONE
**Response Codes :** Success (200 OK), Not Found (404)
