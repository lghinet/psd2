#!/bin/bash

payload='{
 "redirect_uris": ["https://localhost:5009/signin-bt"],
 "company_name":"TPP Corp.",
 "client_name":"Third Party Provider Application DEMO",
 "company_url":"https://google.com",
 "contact_person":"Contact TPP",
 "email_address":"contact.tpp@test.com",
 "phone_number":"+40700000000",
 "contact_type":"ADMINISTRATIVE",
 "isPkce": true
}'

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X POST "https://api.apistorebt.ro/bt/sb/oauth/register/TppOauthBT" \
-H 'Accept: application/json' \
-H 'Content-Type: application/json' \
-d "${payload}"