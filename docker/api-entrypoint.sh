#!/bin/bash
set -euo pipefail

# If a connection string is provided, attempt to apply migrations before starting
if [ -n "${ConnectionStrings__DefaultConnection-}" ]; then
  echo "[entrypoint] Connection string detected. Running EF Core database migrations..."
  # Try to run migrations. We use --no-build because the SDK image already has the source
  # mounted and dotnet watch will build the project when starting. If you prefer, remove --no-build.
  dotnet-ef database update --no-build --project Smart-trafic-controller-api.csproj --startup-project Smart-trafic-controller-api.csproj || \
    { echo "[entrypoint] Migration command failed. Proceeding to start the app (Program.cs also runs migrations)."; }
else
  echo "[entrypoint] No ConnectionStrings__DefaultConnection env var set — skipping migrations."
fi

## Ensure appsettings.json exists in the app folder. This file is often gitignored
## (contains secrets). Create an empty JSON object if it's missing so the app
## can load a file without error; environment variables can still override
## configuration at runtime.
if [ ! -f /app/appsettings.json ]; then
  echo "[entrypoint] /app/appsettings.json not found — creating an empty appsettings.json."
  printf '{}' > /app/appsettings.json
  chmod 644 /app/appsettings.json || true
fi

# Start the app using dotnet watch
exec dotnet watch run --project Smart-trafic-controller-api.csproj --urls "http://0.0.0.0:5010"
