#!/bin/bash

curl --request POST \
  --url https://api.apistorebt.ro/bt/sb/bt-psd2-aisp/v1/consents \
  --header 'accept: application/json' \
  --header 'content-type: application/json' \
  --header 'psu-geo-location: 46.931808,26.369690' \
  --header 'psu-ip-address: 128.0.57.43' \
  --header 'x-request-id: 5d947bcf-156b-4e5a-8cbd-a846ba4c2432' \
  --data '{"access":{"availableAccounts":"allAccounts"},"recurringIndicator":true,"validUntil":"2020-11-01","combinedServiceIndicator":false,"frequencyPerDay":4}'