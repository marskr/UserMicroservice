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
  "Email": [String],   
  "Name": [String],  
  "Surname": [String],   
  "PhoneNumber": [Int],  
  "HashPassword": [String],  
  "UserAccountStatus": [String],  
  "AccountStatusChangeDate": [DateTime],  
  "Salt": [String],  
  "AuthToken": [String],   
  "AuthTokenExpiration": [DateTime],  
  "PermissionId": [Int]  
}      
**Response Codes :** Success (200 OK), Not Found (404)  


# Add user

**Title :** Add new user.  
**URL :** /api/Users  
**Method :** POST  
**URL Params :**  NONE  
**Data Params :**  
{  
  "Email": [String],   
  "Name": [String],  
  "Surname": [String],   
  "PhoneNumber": [Int],  
  "HashPassword": [String],  
  "UserAccountStatus": [String],  
  "AccountStatusChangeDate": [DateTime],  
  "Salt": [String],  
  "AuthToken": [String],   
  "AuthTokenExpiration": [DateTime],  
  "PermissionId": [Int]  
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
  "Email": [String],   
  "Name": [String],  
  "Surname": [String],   
  "PhoneNumber": [Int],  
  "HashPassword": [String],  
  "UserAccountStatus": [String],  
  "AccountStatusChangeDate": [DateTime],  
  "Salt": [String],  
  "AuthToken": [String],   
  "AuthTokenExpiration": [DateTime],  
  "PermissionId": [Int]  
}      
**Response Codes :** Success (200 OK), Not Found (404)  
**Annotation :** You can update Name, Surname, PhoneNumber & Password!  


# Delete user

**Title :** Delete old user.  
**URL :** /api/Users/:email  
**Method :** DELETE  
**URL Params :** Required: email = [string]   
**Data Params :** NONE  
**Response Codes :** Success (200 OK), Not Found (404)
  
