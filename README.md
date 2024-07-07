# MOVEit Transfer File Monitor

This .NET console application monitors a local folder for new files and uploads them to MOVEit Transfer via its REST API.

## Requirements

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- MOVEit Transfer account credentials (username and password)

## Setup Instructions

**Run the following commands in your terminal to clone the repository**
   ```
   git clone https://github.com/bobistefxA54/MoveItFileMonitor
   cd MoveItFileMonitor
   cd MoveItFileMonitor
   ```
   NOTE: You need to run the cd command twice because the initial folder in the repository contains the solution and the project folder and you need to then go into the project folder itself to run the application

**Run these 2 commands in the terminal while in the 'MoveItFileMonitor' project folder**
   ```
   dotnet build
   dotnet run -- "<username>" "<password>" "<local_folder_path>"
   ```
   Example: dotnet run -- "bobistefx" "foobar" "D:\UploadTheseFiles"

**You can close the application by clicking on any button as soon as you are done**
