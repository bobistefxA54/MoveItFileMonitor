# MOVEit Transfer File Monitor

This .NET console application monitors a local folder for new files and uploads them to MOVEit Transfer via its REST API.

## Requirements

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- MOVEit Transfer account credentials (username and password)

## Setup Instructions

1. **Clone the Repository**
   Run the following commands in your terminal
   ```
   git clone https://github.com/bobistefxA54/MoveItFileMonitor
   cd MoveItFileMonitor
   ```

2. **Run the Application**
   Now run these 2 commands in the terminal while in the 'MoveItFileMonitor' folder
   ```
   dotnet build
   dotnet run -- "<username>" "<password>" "<local_folder_path>"
   ```
   Eg. dotnet run -- "bobistefx" "<foobar>" "<D:\UploadTheseFiles>"

3. **Now you can close the application by clicking on any button as soon as you are done**
