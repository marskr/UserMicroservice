# UserMicroservice 
This is a simple microservice created for users management.  
Technology: ASP .NET Core API with basic CRUD operations for users and admins.


# Show user 

**Title :** Show user by email.  
**URL :** /api/Users/:email  
**Method :** GET  
**URL Params :** Required: email = [string]   
**Data Params :**  
{  
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


# Add user

**Title :** Add new user.  
**URL :** /api/Users  
**Method :** POST  
**URL Params :**  NONE  
**Data Params :**  
{      
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
  
