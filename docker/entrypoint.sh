#!/usr/bin/env bash
set -e

# entrypoint.sh - ensure appsettings.json exists, create/apply EF migrations if needed, then start the API

echo "[entrypoint] Starting entrypoint script"

# Move to project folder
cd /app/Smart-traffic-controller-api || { echo "Project folder not found"; exit 1; }

# Ensure appsettings.json exists (minimal file with empty object)
if [ ! -f appsettings.json ]; then
  echo "[entrypoint] appsettings.json not found - creating minimal file"
  echo "{}" > appsettings.json
else
  echo "[entrypoint] appsettings.json already exists - leaving as is"
fi

# Ensure dotnet-ef is available (should be installed in image)
if ! command -v dotnet-ef >/dev/null 2>&1; then
  echo "[entrypoint] dotnet-ef not found on PATH - attempting to use dotnet tool path"
  export PATH="$PATH:/root/.dotnet/tools"
fi

# Try to create initial migration if none exist. If this fails (design-time services etc.), continue.
if [ ! -d Migrations ]; then
  echo "[entrypoint] No Migrations folder found - attempting to add InitialCreate migration"
  set +e
  dotnet ef migrations add InitialCreate -p ./Smart-traffic-controller-api.csproj -s ./Smart-traffic-controller-api.csproj -o Migrations
  MIG_ADD_EXIT=$?
  set -e
  if [ "$MIG_ADD_EXIT" -ne 0 ]; then
    echo "[entrypoint] dotnet ef migrations add failed (exit $MIG_ADD_EXIT). Proceeding without failing container startup. You can run migrations manually later." >&2
  else
    echo "[entrypoint] InitialCreate migration added"
  fi
else
  echo "[entrypoint] Migrations folder already present - skipping migration add"
fi

# Try to apply migrations to the database. Continue even if it fails (DB might not be available yet).
set +e
echo "[entrypoint] Attempting to run 'dotnet ef database update'"
dotnet ef database update -p ./Smart-traffic-controller-api.csproj -s ./Smart-traffic-controller-api.csproj
DB_UPDATE_EXIT=$?
set -e
if [ "$DB_UPDATE_EXIT" -ne 0 ]; then
  echo "[entrypoint] 'dotnet ef database update' exited with $DB_UPDATE_EXIT. Database may not be ready; continuing to start the app. You can re-run 'dotnet ef database update' manually later." >&2
else
  echo "[entrypoint] Database updated with migrations"
fi

echo "[entrypoint] Starting dotnet watch run"
exec dotnet watch run --project ./Smart-traffic-controller-api.csproj --urls "http://0.0.0.0:5010"
