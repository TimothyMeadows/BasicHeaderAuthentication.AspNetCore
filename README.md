# Basic Header Authentication

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) [![nuget](https://img.shields.io/nuget/v/BasicHeaderAuthentication.AspNetCore.svg)](https://www.nuget.org/packages/BasicHeaderAuthentication.AspNetCore/)

This package is designed to provide basic header authentication to .NET core 2.x / 3x. This is intended for usage with API's where a single header can be used as a "key" that has been provided to a consumer.

#### *Warning: This header is transmitted in plain-text. This means it's subject to man in the middle attacks. If you are securing important information such as medical data, credit cards, or personal information. You should consider a more secure method for authentication. If you can't. Please look into adding a method of forward secrecy to your authenticator code in your application.*

## IBasicHeaderAuthenticator

It's your applications responsibility to check if a value supplied in the authentication header is valid. This is done by creating a class that inherits IBasicHeaderAuthenticator and registering it at startup as shown below. You return null if validation fails which will terminate the request as Unauthorized. You return a ClaimsPrincipal class if validation passes. If you return an empty ClaimsPrincipal as shown below. It will be auto populated with a Name claim containing the header value as ```User.Identity.Name```.

*Note: This example only checks if a value supplied in the request exists, and is not null, or empty. You should use this method to check if the value exists in a database, or dictionary / list etc..*

```csharp
public class BasicHeaderAuthenticator : IBasicHeaderAuthenticator
{
    public async Task<ClaimsPrincipal> SignIn(string value)
    {
        return string.IsNullOrEmpty(value) ? null : new ClaimsPrincipal();
    }
}
```

*Note: Make sure this line is before AddAuthentication, and AddBasicHeaderAuthentication*

```csharp
services.AddTransient<IBasicHeaderAuthenticator, BasicHeaderAuthenticator>();
```

## Configuration

```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = BasicHeaderAuthenticationOptions.DefaultScheme;
    options.DefaultChallengeScheme = BasicHeaderAuthenticationOptions.DefaultScheme;
}).AddBasicHeaderAuthentication(options =>
{
    options.HeaderKey = "X-AuthKey"; // You can use any header key you wish.
});
```

## Authorization

You can use the ```[Authorize(AuthenticationSchemes = BasicHeaderAuthenticationOptions.DefaultScheme)]``` attribute out of the box. However, If you wish to use the ```[Authorize]``` attribute without having to supply an authenticator scheme each time. You can add the following code after AddBasicHeaderAuthentication.

```csharp
services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(BasicHeaderAuthenticationOptions.DefaultScheme)
    .RequireAuthenticatedUser()
    .Build();
});
```
