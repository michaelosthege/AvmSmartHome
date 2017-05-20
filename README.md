# AvmSmartHome
Implementation of the [AVM Smart Home API](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/AHA-HTTP-Interface.pdf)

# Installation
You can install the package from NuGet: [AvmSmartHome.NET](https://www.nuget.org/packages/AvmSmartHome.NET/)

The library implements [.NET Standard 1.4](https://docs.microsoft.com/en-us/dotnet/articles/standard/library) and is supported on (and above):
+ .NET Core 1.0
+ .NET Framework 4.6.1
+ Mono 4.6
+ Xamarin.iOS 10.0
+ Xamarin.Android 7.0
+ Universal Windows Platform 10


# Usage
```csharp
SessionInfo session = new SessionInfo("myiotuser", "myiotpwd", "fritz.box");
await session.AuthenticateAsync();
string[] switches = await session.GetSwitchesAsync();

// Steckdose auswählen
string ain = switches[0];

string name = await session.GetSwitchNameAsync(ain);
double power = await session.GetSwitchPowerAsync(ain);
double energy = await session.GetSwitchEnergyAsync(ain);
double temp = await session.GetSwitchTemperatureAsync(ain);

string message = $"Switch {ain}" +
    $"\r\n name   = {name}" +
    $"\r\n P [mW] = {power}" +
    $"\r\n E [Wh] = {energy}" +
    $"\r\n T [°C] = {temp}";
Debug.WriteLine(message);
}
```

Example output:

```
Switch 087610251891
 name   = FRITZ!DECT 200 #2
 P [mW] = 3070
 E [Wh] = 17236
 T [°C] = 23.5
```

# Testing
For running the unit tests, create a user in your Fritz!Box with the credentials found in [`TestCredentials.cs`](https://github.com/michaelosthege/AvmSmartHome/blob/master/AvmSmartHome.NET.Test/TestCredentials.cs), or modify the credentials to match with an existing user. The user needs *Smart Home* permissions only.

Then you can run the unit tests in Visual Studio 2017 (Community).