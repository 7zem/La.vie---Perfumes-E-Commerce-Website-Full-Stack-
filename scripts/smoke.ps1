$ErrorActionPreference = 'Stop'

# Kill any running API instances to avoid file locks
Get-Process Perfumes.API -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue

$env:ASPNETCORE_ENVIRONMENT = 'Development'
$port = 5080

# Start API
$proc = Start-Process dotnet -ArgumentList @('run','--project','Perfumes.API','--no-build','--urls',"http://localhost:$port") -PassThru

Start-Sleep -Seconds 3

function Test-Endpoint {
  param([string]$Url)
  try {
    $code = & curl.exe -sS -o NUL -w "%{http_code}" $Url
  } catch {
    $code = '000'
  }
  "$(Get-Date -Format s) $code $Url"
}

Test-Endpoint "http://localhost:$port/swagger"
Test-Endpoint "http://localhost:$port/api/product"
Test-Endpoint "http://localhost:$port/api/category"
Test-Endpoint "http://localhost:$port/api/brand"
Test-Endpoint "http://localhost:$port/api/weatherforecast"

# Stop API
Stop-Process -Id $proc.Id -Force


