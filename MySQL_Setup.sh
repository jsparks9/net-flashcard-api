#!/bin/bash

CONTAINER_NAME=flashcard-db-mssql
MSSQL_SA_PASSWORD=Pwd123456
MSSQL_USR=sa
MSSQL_EXEC_PATH="/opt/mssql-tools18/bin/sqlcmd"
CONNECT_="MSYS_NO_PATHCONV=1 docker exec $CONTAINER_NAME $MSSQL_EXEC_PATH -S localhost,1433 -U ${MSSQL_USR} -P ${MSSQL_SA_PASSWORD} -C"

echo Current Time: $(date +"%T")

# Run the SQL Server container
docker run -d \
  --name ${CONTAINER_NAME} \
  -e 'ACCEPT_EULA=Y' \
  -e "SA_PASSWORD=${MSSQL_SA_PASSWORD}" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2019-latest

# Wait for SQL Server to become available
until eval $CONNECT_ -Q "\"SELECT 1;\"" > /dev/null 2>&1; do
    >&2 echo "SQL Server is unavailable - waiting"
    sleep 1
done

if eval $CONNECT_ -Q "\"CREATE DATABASE card_db;\""; then
    echo "Database card_db created successfully."
else
    >&2 echo "Failed to create database card_db."
fi

echo Current Time: $(date +"%T")
echo "Completed"
read -p "Press [Enter] key to exit..."