# SurveillanceStation.Net
A C# wrapper around the Synology Surveillance Station API

## WIP
- Add unit tests
- Create Nuget package

# SurveillanceStation.Net

SurveillanceStation.Net is a .NET client library for interacting with the Synology Surveillance Station API. It provides a simple and intuitive interface for .NET applications to integrate with Synology's surveillance system.

## Features

- Full coverage of Synology Surveillance Station API (version 9.2.1)
- Asynchronous methods for all API operations
- Strong typing for request and response data
- Easy-to-use client interface
- Comprehensive error handling

## Installation

You can install SurveillanceStation.Net via NuGet Package Manager:

```
Install-Package SurveillanceStation.Net
```

Or via .NET CLI:

```
dotnet add package SurveillanceStation.Net
```

## Usage

Here's a quick example of how to use SurveillanceStation.Net:

```csharp
using SurveillanceStation.Net;

// Create a new client
var client = new SurveillanceStationClient("https://your-nas-address:5001");

// Login
await client.LoginAsync("username", "password");

// List cameras
var cameras = await client.ListCamerasAsync();
foreach (var camera in cameras.Cameras)
{
    Console.WriteLine($"Camera: {camera.Name}, ID: {camera.CamId}");
}

// Take a snapshot
var snapshot = await client.TakeAndDownloadSnapshotAsync("1");
File.WriteAllBytes("snapshot.jpg", snapshot);

// Logout
await client.LogoutAsync();
```

## API Coverage

SurveillanceStation.Net covers the following areas of the Surveillance Station API:

- Authentication
- Camera Management
- PTZ Controls
- Recording and Snapshot Management
- Bookmark Management
- E-Map Integration
- License Plate Recognition

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Thanks to Synology for providing the Surveillance Station API documentation.

## Disclaimer

This project is not officially associated with or endorsed by Synology Inc. Synology and Surveillance Station are trademarks of Synology Inc., registered in the United States and other countries.
