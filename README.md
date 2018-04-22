# UsersMicroservice 
This is a simple microservice created for users management.  
Technology: ASP .NET Core API with basic CRUD operations for users and admins.

## "Users" http methods

### Show user 

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


### Add user

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


### Update user

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
**Response Codes :** Success (200 OK), Bad Request (400), Not Found (404)  
**Annotation :** You can update Name, Surname, PhoneNumber & Password! 
**Annotation :** the error 400 will be returned in case of exception!  


### Delete user

**Title :** Delete old user.  
**URL :** /api/Users/:email  
**Method :** DELETE  
**URL Params :** Required: email = [string]   
**Data Params :** NONE  
**Response Codes :** Success (200 OK), Bad Request (400), Not Found (404)
**Annotation :** the error 400 will be returned in case of exception!  


## "Authentication" http methods

### Check if authorized 

**Title :** Check if provided token is valid (and so email & password).  
**URL :** /api/Authentication/:token  
**Method :** GET  
**URL Params :** Required: token = [string]   
**Data Params :**  NONE  
**Response Body :** TRUE (if token is valid) or FALSE (if not)  
**Response Codes :** Success (200 OK), Not Found (404)  

### Create token

**Title :** Create a token using provided email & password.  
**URL :** /api/Authentication/:email/:password  
**Method :** POST  
**URL Params :** Required: email = [string] and password = [string]  
**Data Params :**  NONE  
**Response Body :** string containing JWF token  
**Response Codes :** Success (200 OK), Bad Request (400)  
**Annotation :** the error 400 will be returned in case of NULL value of password or email! 

### Update expiration date

**Title :** If token is proper update expiration date (now + 1 hour).  
**URL :** /api/Authentication/:token  
**Method :** PUT  
**URL Params :** Required: token = [string]   
**Data Params :**  NONE  
**Response Body :** TRUE (if date was updated) or FALSE (if not)  
**Response Codes :** Success (200 OK), Bad Request (400)  
**Annotation :** the error 400 will be returned in case of exception!


  
