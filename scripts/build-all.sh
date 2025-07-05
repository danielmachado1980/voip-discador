#!/bin/bash
(cd src/Webphone && npm install && npm run build)
(cd src/VoipDiscador.Api && dotnet publish -c Release -o out)
