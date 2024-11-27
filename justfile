default:
    echo 'Hello, world!'

run-docker:
     pwsh -Command "& {docker run --pull always --rm -it -p 443:443 -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -e ('ASPNETCORE_Kestrel__Certificates__Default__Password=' + $env:CERT_PASS) -v ($env:USERPROFILE + '\.aspnet\https:/https/') ghcr.io/mbjd05/ashamed-backend:latest}"
