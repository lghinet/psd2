#!/bin/bash

payload='{
 "redirect_uris": ["https://localhost:5009/signin-bt", "https://psd2.azurewebsites.net/signin-bt"],
 "company_name":"TPP Corp.",
 "client_name":"Third Party Provider Application DEMO",
 "company_url":"https://google.com"
}'

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X POST "https://api.apistorebt.ro/bt/sb/oauth/register/TppOauthBT" \
-H 'Accept: application/json' \
-H 'Content-Type: application/json' \
-d "${payload}"