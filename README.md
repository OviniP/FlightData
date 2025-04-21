# FlightData
This project aims to automate the analysis of aircraft flight data to identify inconsistencies and improve data quality. Traditionally performed manually by staff using Excel, this process is now streamlined through a software solution that programmatically detects logical errors and anomalies in flight records — significantly enhancing efficiency, accuracy, and scalability.

## Setup
1. Clone or download the solution.
2. Place the flight data CSV file in a directory accessible by the user running the .NET application.
3. Update the DataFilePath setting in the configuration file with the path to the data folder.
4. Run the API.

## Development Concerns
* The API is currently publicly accessible. In a real-world environment, appropriate security measures (e.g., authentication, authorization, and HTTPS enforcement) should be implemented to protect the system.

* The CSV data format is currently assumed to be correct. However, incorporating robust data validation during the loading process is recommended to ensure integrity and prevent errors caused by malformed or unexpected input.

## Architecture
Clean Architecture Followed.

![image](https://github.com/user-attachments/assets/9c2a78eb-cc71-4bfb-9962-1c79ed19ae20)
