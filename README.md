# Process Payment

### Run Migration
  ```
    dotnet ef migrations add InitialCreate -c DataContext -p Infrastructure -s API/ -o Data/Migrations  (vscode)
    Add-Migration InitialCreate -c DataContext  -p Infrastructure -s API/ -o Data/Migrations (visual studio)
  ```

.netcore 5 project - <b>ProcessPayment</b>
